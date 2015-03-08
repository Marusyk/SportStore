using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class EmailOrderSubmitter : IOrderSubmitter
    {
        const string MailSubject = "New order submitted!";
        string smtpServer, mailTo, mailFrom;

        public EmailOrderSubmitter(string smtpServer, string mailFrom, string mailTo)
        {
            this.smtpServer = smtpServer;
            this.mailFrom = mailFrom;
            this.mailTo = mailTo;
        }

        public void SubmitOrder(Cart cart)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine("A new order has been submitted");
            body.AppendLine("---");
            body.AppendLine("Items:");
            foreach(var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;
                body.AppendFormat("{0} x {1} (subtotal: {2:c}",
                    line.Quantity,
                    line.Product.Name,
                    subtotal);
            }
            body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue());
            body.AppendLine("---");
            body.AppendLine("Ship to:");
            body.AppendLine(cart.ShippingDetails.Name);
            body.AppendLine(cart.ShippingDetails.Line1);
            body.AppendLine(cart.ShippingDetails.Line2 ?? "");
            body.AppendLine(cart.ShippingDetails.Line3 ?? "");
            body.AppendLine(cart.ShippingDetails.City);
            body.AppendLine(cart.ShippingDetails.State ?? "");
            body.AppendLine(cart.ShippingDetails.Country);
            body.AppendLine(cart.ShippingDetails.Zip);
            body.AppendLine("---");
            body.AppendFormat("Gift wrap: {0}", cart.ShippingDetails.GiftWrap ? "Yes" : "No");

            SmtpClient smtpClient = new SmtpClient(smtpServer);
            smtpClient.Send(new MailMessage(mailFrom, mailTo, MailSubject, body.ToString()));
        }
    }
}
