using StarBucks_Backend.Models;
using StarBucks_Backend.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StarBucks_Backend.Implementations
{
    public class ProductImplementation : IProductService
    {
        private readonly string _productFilePath = "D:\\project 1\\.vs\\StarBucks Project\\products.csv";
        private readonly string _salesFilePath = "D:\\project 1\\.vs\\StarBucks Project\\salesReport.csv";

        public IEnumerable<Products> GetProducts()
        {
            var products = new List<Products>();
            using (var reader = new StreamReader(_productFilePath))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var values = line?.Split(',');

                    if (values?.Length != 6)
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                        continue;
                    }

                    try
                    {
                        int id = int.Parse(values[0]);
                        string category = values[1];
                        string productName = values[2];
                        string details = values[3];
                        decimal price = decimal.Parse(values[4]);
                        int stock = int.Parse(values[5]);

                        products.Add(new Products
                        {
                            ID = id,
                            Category = category,
                            ProductName = productName,
                            Details = details,
                            Price = price,
                            Stock = stock
                        });
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Format error in line: {line}. Error: {ex.Message}");
                    }
                }
            }
            return products;
        }

        public Products GetProductById(int id)
        {
            var products = GetProducts();
            return products.FirstOrDefault(p => p.ID == id);
        }

        public void AddProducts(Products product)
        {
            var csvLine = $"{product.ID},{product.Category},{product.ProductName},{product.Details},{product.Price},{product.Stock}";

            using (var writer = new StreamWriter("D:\\project 1\\.vs\\StarBucks Project\\products.csv", true))
            {
                writer.WriteLine(csvLine);
            }
        }

        public string BuyProduct(int productId, int quantity)
        {
            var products = GetProducts().ToList();
            var product = products.FirstOrDefault(p => p.ID == productId);

            if (product == null)
            {
                return "Product not found";
            }

            if (product.Stock < quantity)
            {
                return "Insufficient stock";
            }

            product.Stock -= quantity;

            using (var writer = new StreamWriter(_productFilePath, false))
            {
                writer.WriteLine("ID,Category,ProductName,Details,Price,Stock");
                foreach (var prod in products)
                {
                    writer.WriteLine($"{prod.ID},{prod.Category},{prod.ProductName},{prod.Details},{prod.Price},{prod.Stock}");
                }
            }

            var sale = new Sales
            {
                ProductName = product.ProductName,
                Quantity = quantity,
                Rate = product.Price,
                Amount = product.Price * quantity,
                Date = DateTime.Now
            };

            var saleCsvLine = $"{sale.ProductName},{sale.Quantity},{sale.Rate},{sale.Amount},{sale.Date}";
            using (var writer = new StreamWriter(_salesFilePath, true))
            {
                writer.WriteLine(saleCsvLine);
            }

            return "Purchase successful";
        }

        public string DeleteProduct(int productId)
        {
            var products = GetProducts().ToList();
            var product = products.FirstOrDefault(p => p.ID == productId);

            if (product == null)
            {
                return "Product not found";
            }
            products.Remove(product);
            using (var writer = new StreamWriter(_productFilePath))
            {
                writer.WriteLine("ID,Category,ProductName,Details,Price,Stock"); // Write header
                foreach (var prod in products)
                {
                    writer.WriteLine($"{prod.ID},{prod.Category},{prod.ProductName},{prod.Details},{prod.Price},{prod.Stock}");
                }
            }
            return $"{product.ProductName} succesfully removed";
        }
    }
}