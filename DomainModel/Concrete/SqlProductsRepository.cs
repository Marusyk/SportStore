using System;
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
    }
}
