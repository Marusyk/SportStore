﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;
using System.Data.Linq;

namespace DomainModel.Concrete
{
    public class SqlProductsRepository : IProductsRepository
    {
        private Table<Product> productTable;

        public SqlProductsRepository(string connectionString)
        {
            productTable = (new DataContext(connectionString)).GetTable<Product>();
        }

        public IQueryable<Product> Products
        {
            get { return productTable; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
                productTable.InsertOnSubmit(product);
            else
            {
                // якщо обновляється існуючий товар, доручити DataContext збереження цього екземпляра
                productTable.Attach(product);
                // також доручити DataContext виявлення змін з моменту останнього збереження
                productTable.Context.Refresh(RefreshMode.KeepCurrentValues, product);
            }
            productTable.Context.SubmitChanges();
        }
    }
}
