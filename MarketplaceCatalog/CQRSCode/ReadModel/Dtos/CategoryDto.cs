using System;

namespace CQRSCode.ReadModel.Dtos
{
    public class CategoryDto : EntityBase
    {
        public int Version { get; set; }

        public String Name { get; set; }

        public Guid? ParentId { get; set; }

        //Was it activated by administrator
        public Boolean IsActivated { get; set; }

        //IsActivated and has it visible products
        public Boolean IsVisible { get; set; }

        //Visible products directly in category
        public Int32 VisibleProducts { get; set; }

        //Including sub-categories
        public Int32 TotalVisibleProducts { get; set; }

        public CategoryDto(Guid id, String name, Boolean isActivated, Boolean isVisible,
                            int visibleProducts, Int32 totalVisibleProducts, int version,
                            Guid? parentId)
        {
            Id = id;
            Name = name;
            IsActivated = isActivated;
            IsVisible = isVisible;
            VisibleProducts = visibleProducts;
            VisibleProducts = totalVisibleProducts;
            Version = version;
            ParentId = parentId;
        }
    }
}