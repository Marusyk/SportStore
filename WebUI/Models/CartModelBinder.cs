using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class CartModelBinder : IModelBinder
    {
        private const string cartSessionKey = "_cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //деякі засоби привязки моделі можуть обновляти
            //властивості існуючих екземплярів моделі 
            //тут це не потрібно - воно слугує тільки для застосування
            //параметрів методу дії.
            if (bindingContext.Model != null)
                throw new InvalidOperationException("Помилка при обновленні екземплярів");
            Cart cart = (Cart)controllerContext.HttpContext.Session[cartSessionKey];

            if(cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[cartSessionKey] = cart;
            }
            return cart;
        }
    }
}