using System;
using CQRSCode.ReadModel.Events;
using CQRSlite.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CQRSCode.WriteModel.Domain
{
    public class Product : AggregateRoot
    {
        public String Name { get; set; }

        public String Description { get; set; }        

        public bool Activated { get; set; }

        public bool Visible { get; set; }

        public Guid? CategoryId { get; set; }

        public List<Guid> CategoriesHierarchy { get; set; }
        
        public List<Offer> Offers { get; set; }

        private void Apply(ProductCreated e)
        {
             Activated = true;
             Name = e.Name;
             Description = e.Description;
        }

        private void Apply(ProductCategoryDefined e)
        {
            CategoryId = e.CategoryId;
            CategoriesHierarchy = e.CategoriesHierarchy;
        }

        private void Apply(OfferCreated e)
        {
            this.Offers.Add(new Offer(e.OfferId, e.MerchantId, e.Id, true, e.Stock, e.Price, e.MerchantActivated));
        }

        private void Apply(OfferStockSet e)
        {
            var offer = this.Offers.Where(o => o.Id == e.OfferId).FirstOrDefault();
            offer.Stock = e.Stock;
        }

        private void Apply(OfferMerchantDeactivated e)
        {
            var offer = this.Offers.Where(o => o.Id == e.OfferId).FirstOrDefault();
            offer.MerchantActivated = false;
        }

        private void Apply(OfferMerchantActivated e)
        {
            var offer = this.Offers.Where(o => o.Id == e.OfferId).FirstOrDefault();
            offer.MerchantActivated = true;
        }

        private void Apply(ProductDisplayed e)
        {
            this.Visible = true;
        }

        private void Apply(ProductHidden e)
        {
            this.Visible = false;
        }

        private void Apply(OfferDisplayed e)
        {
            var offer = this.Offers.Where(o => o.Id == e.OfferId).FirstOrDefault();
            offer.Visible = true;
        }

        private void Apply(OfferHidden e)
        {
            var offer = this.Offers.Where(o => o.Id == e.OfferId).FirstOrDefault();
            offer.Visible = false;
        }




        public void SetCategory(Guid categoryId)
        {
            if (CategoryId == categoryId) throw new InvalidOperationException("already in this category");

            if (CategoryId.HasValue)
                ApplyChange(new ProductUnpublishedFromCategory(this.Id, CategoryId.Value));

            ApplyChange(new ProductCategoryDefined(this.Id, categoryId));            

            if (Visible)
            {
                ApplyChange(new ProductPublishedToCategory(this.Id, CategoryId.Value, Visible,
                                                            Name, Description,
                                                            Offers.Count(o => o.Visible),
                                                            Offers.Where(o => o.Visible).Select(o => o.Price).ToList()
                                                        ));
            }
        }

        public void CreateOffer(Guid offerId, Guid merchantId, Int16 stock, Decimal price, Boolean merchantActivated, String merchantName, String sku)
        {
            if (Offers.Any(o => o.MerchantId == merchantId)) throw new InvalidOperationException("already an offer for this merchant");
            ApplyChange(new OfferCreated(this.Id, offerId, merchantId, stock, price, merchantActivated, sku, merchantName, true, false));

            EvaluateProductStatusChange();
            EvaluateOfferStatusChange(offerId);
        }

        public void SetStock(Guid offerId, Int16 stock)
        {
            var offer = this.Offers.Where(o => o.Id == offerId).FirstOrDefault();
            if (offer == null) throw new InvalidOperationException(String.Format("no offer with id {0} on this product", offerId));
            
            ApplyChange(new OfferStockSet(this.Id, offerId, stock));

            EvaluateProductStatusChange();
            EvaluateOfferStatusChange(offerId);
        }

        public void ActivateMerchant(Guid offerId, Guid merchantId)
        {
            var offer = this.Offers.Where(o => o.Id == offerId).FirstOrDefault();
            if (offer == null) throw new InvalidOperationException(String.Format("no offer with id {0} on this product", offerId));
            ApplyChange(new OfferMerchantActivated(this.Id, offerId, merchantId));

            EvaluateProductStatusChange();
            EvaluateOfferStatusChange(offerId);
        }

        public void DeactivateMerchant(Guid offerId, Guid merchantId)
        {
            var offer = this.Offers.Where(o => o.Id == offerId).FirstOrDefault();
            if (offer == null) throw new InvalidOperationException(String.Format("no offer with id {0} on this product", offerId));
            ApplyChange(new OfferMerchantDeactivated(this.Id, offerId, merchantId));

            EvaluateProductStatusChange();
            EvaluateOfferStatusChange(offerId);
        }

        public void ActivateCategory()
        {
            if (Visible)
                ApplyChange(new ProductPublishedToCategory(this.Id, CategoryId.Value, Visible,
                                                            Name, Description,
                                                            Offers.Count(o => o.Visible),
                                                            Offers.Where(o => o.Visible).Select(o => o.Price).ToList()
                                                        ));
        }

        private void Publish()
        {
            ApplyChange(new ProductDisplayed(this.Id));
            
            if (CategoryId.HasValue)
                ApplyChange(new ProductPublishedToCategory(this.Id, CategoryId.Value, Visible,
                                                            Name, Description,
                                                            Offers.Count(o => o.Visible),
                                                            Offers.Where(o => o.Visible).Select(o => o.Price).ToList()
                                                        ));
        }

        private void Unpublish()
        {
            ApplyChange(new ProductHidden(this.Id));

            if (CategoryId.HasValue)
                ApplyChange(new ProductUnpublishedFromCategory(this.Id, CategoryId.Value));
        }

        private void UnpublishOffer(Guid offerId)
        {
            ApplyChange(new OfferHidden(this.Id, offerId));

            var offer = Offers.Where(o => o.Id == offerId).FirstOrDefault();

            ApplyChange(new OfferUnpublishedFromMerchant(this.Id, offerId, offer.MerchantId));
        }

        private void PublishOffer(Guid offerId)
        {
            ApplyChange(new OfferDisplayed(this.Id, offerId));
            
            var offer = Offers.Where(o => o.Id == offerId).FirstOrDefault();

            ApplyChange(new OfferPublishedToMerchant(this.Id, offerId, offer.MerchantId));
        }

        private Product()
        {
            this.Offers = new List<Offer>();
        }

        public Product(Guid id, String name, String description, Boolean isActivated,
                        Boolean isVisible, String ean, String upc)
        {
            Id = id;
            this.Offers = new List<Offer>();
            ApplyChange(new ProductCreated(id, name, description, isActivated, isVisible, ean, upc));
        }

        internal Boolean EvaluateVisibility()
        {
            return Offers.Any(o => o.EvaluateVisibility()) && Activated;
        }

        public void EvaluateProductStatusChange()
        {
            if (!Visible && EvaluateVisibility())
                Publish();
            
            if (Visible && !EvaluateVisibility())
                Unpublish();
        }

        public void EvaluateOfferStatusChange(Guid offerId)
        {
            Offer offer = Offers.Where(o => o.Id == offerId).FirstOrDefault();

            if (!offer.Visible && offer.EvaluateVisibility())
                PublishOffer(offerId);
            
            if (offer.Visible && !offer.EvaluateVisibility())
                UnpublishOffer(offerId);
        }
    }
}