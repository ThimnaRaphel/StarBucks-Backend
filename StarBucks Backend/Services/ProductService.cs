using StarBucks_Backend.Models;

namespace StarBucks_Backend.Services
{
    public interface IProductService
    {
       IEnumerable<Products> GetProducts();
       Products GetProductById(int id);
       void AddProducts(Products product);
       string BuyProduct(int ProductId, int Quantity);
       string DeleteProduct(int id);
    }
}
