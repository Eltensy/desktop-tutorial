//using System.Linq;
//using System.Collections.Generic;


//namespace wpf_taskmaster_test.Data
//{
//    public class ProductRepository
//    {
//        private AppDbContext context;

//        public ProductRepository()
//        {
//            context = new AppDbContext();
//            context.Database.EnsureCreated(); // Creates the database if it doesn't exist.
//        }

//        public List<Product> GetAllProducts()
//        {
//            return context.Products.ToList();
//        }

//        public void AddProduct(Product product)
//        {
//            context.Products.Add(product);
//            context.SaveChanges();
//        }

//        // Implement other CRUD operations as needed.
//    }

//}
