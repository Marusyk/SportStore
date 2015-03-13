﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class FakeProductsRepository : IProductsRepository
    {
        private static IQueryable<Product> fakeProducts = new List<Product>{
            new Product { Name = "Football", Price = 25 },
            new Product { Name = "Surf board", Price = 179 },
            new Product { Name = "Running shoes", Price = 95 },
        }.AsQueryable();

        public IQueryable<Product> Products
        {
            get { return fakeProducts; }
        }

        public void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
