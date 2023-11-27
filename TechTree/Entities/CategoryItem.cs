using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechTree.Entities
{
    public class CategoryItem
    {

        private DateTime _created = DateTime.MinValue;
        public int Id { get; set; }

        [Required]
        [StringLength(200,MinimumLength =2)]
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int MediaTypeId { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public virtual ICollection<SelectListItem> MediaTypes { get; set; }

        [NotMapped]
        public int ContentId { get; set; }

        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}" )]
        public DateTime DateTimeItemReleased 
        { 
            get
            {
                return  (_created == DateTime.MinValue)?DateTime.Now.Date:_created;
            }
            set
            {
                _created = value;
            }
        }
    }
}
