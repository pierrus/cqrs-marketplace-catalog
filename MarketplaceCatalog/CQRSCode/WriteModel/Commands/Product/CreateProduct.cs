using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    ///
    /// Creates or find a product
    /// Creates or updates a product
    ///
    public class CreateProduct : ICommand 
	{
        public readonly String Name;
        
        public readonly String Description;

        public readonly String EAN;

        public readonly String UPC;

        public CreateProduct(Guid id, String name, String description, String ean = null, String upc = null) 
		{
            Id = id;
            Name = name;
            Description = description;
            EAN = ean;
            UPC = upc;
        }

        public Guid Id { get; set; }

        public int ExpectedVersion { get; set; }
	}
}