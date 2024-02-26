using CarvedRock.Api.ApiModels;

namespace CarvedRock.Api.Interfaces
{
    public interface IProductLogic
    {
        Task<IEnumerable<Product>> GetProductsForCategory(string category);
    }
}
