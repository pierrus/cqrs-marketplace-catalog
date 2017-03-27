using CQRSCode.ReadModel.Dtos;
using CQRSCode.ReadModel.Repository;
using CQRSlite.Domain;
using CQRSlite.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CQRSTests.Storage
{
  public class MongoRepository
  {
    [Fact]
    public void SingleProduct()
    {
      Repository<ProductDto> productRepository = new Repository<ProductDto>(new CQRSCode.ReadModel.Repository.MongoOptions { ConnectionString = "mongodb://localhost:27017", Database = "marketplacecatalog" });

      Guid productId = Guid.NewGuid();
      Guid categoryId = Guid.NewGuid();
      Guid offerId = Guid.NewGuid();

      var newProduct = new ProductDto (productId, categoryId, "myproduct",
                            "my nice product", true,
                            true, 1);

      newProduct.Offers = new List<OfferDto>() { new OfferDto(offerId, Guid.NewGuid(), "mymerchant", true, true, 1, 456, 2, "mysku") };

      productRepository.Insert(newProduct);

      var myProduct = productRepository.GetById(productId);

      Assert.NotNull(myProduct);
      Assert.Equal(1, myProduct.Offers.Count);
    }

    [Fact]
    public void ProductAdvancedCriterias()
    {
      Repository<ProductDto> productRepository = new Repository<ProductDto>(new CQRSCode.ReadModel.Repository.MongoOptions { ConnectionString = "mongodb://localhost:27017", Database = "marketplacecatalog" });

      Guid productId = Guid.NewGuid();
      Guid categoryId = Guid.NewGuid();
      Guid offerId = Guid.NewGuid();
      Guid myRandomSku = Guid.NewGuid();

      var newProduct = new ProductDto (productId, categoryId, "myproduct",
                            "my nice product", true,
                            true, 1);

      newProduct.Offers = new List<OfferDto>() { new OfferDto(offerId, Guid.NewGuid(), "mymerchant", true, true, 1, 456, 2, myRandomSku.ToString()) };

      productRepository.Insert(newProduct);

      IList<ProductDto> products = productRepository.SearchFor(prod => prod.CategoryId == categoryId);
      Assert.Equal(1, products.Count);

      products = productRepository.SearchFor(prod => prod.Offers.Any(o => o.SKU == "notasku"));
      Assert.Equal(0, products.Count);
      
      products = productRepository.SearchFor(prod => prod.Offers.Any(o => o.SKU == myRandomSku.ToString()));
      Assert.Equal(1, products.Count);
    }

    [Fact]
    public void ProductSummaryAdvancedCriterias()
    {
      Repository<ProductSummaryDto> productRepository = new Repository<ProductSummaryDto>(new CQRSCode.ReadModel.Repository.MongoOptions { ConnectionString = "mongodb://localhost:27017", Database = "marketplacecatalog" });

      Guid productId = Guid.NewGuid();
      Guid categoryId = Guid.NewGuid();
      Guid offerId = Guid.NewGuid();

      productRepository.Insert(new ProductSummaryDto { Id = Guid.NewGuid(), CategoryId = categoryId, Name = "myproduct", Description = "my nice product", Prices = new List<decimal>() { 1, 2, 3 } });
      productRepository.Insert(new ProductSummaryDto { Id = Guid.NewGuid(), CategoryId = categoryId, Name = "myproduct", Description = "my nice product", Prices = new List<decimal>() { 1, 2, 3 } });
      productRepository.Insert(new ProductSummaryDto { Id = Guid.NewGuid(), CategoryId = categoryId, Name = "myproduct", Description = "my nice product", Prices = new List<decimal>() { 1, 2, 3 } });
      productRepository.Insert(new ProductSummaryDto { Id = Guid.NewGuid(), CategoryId = categoryId, Name = "myproduct", Description = "my nice product", Prices = new List<decimal>() { 1, 2, 3 } });

      IList<ProductSummaryDto> products = productRepository.SearchFor(prod => prod.CategoryId == categoryId);

      Assert.Equal(4, products.Count);
      Assert.All(products, p => Assert.Equal("myproduct", p.Name));
    }
  }
}
