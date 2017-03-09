using System;

namespace CQRSCode.WriteModel.Referential
{
    public class CategoryId
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public String Name { get; set; }
    }
}