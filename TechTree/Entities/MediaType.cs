using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechTree.Interfaces;

namespace TechTree.Entities
{
    public class MediaType:IPrimaryProperties
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200,MinimumLength =2)]
        public string Title { get; set; }

        [Required]
        public string ThumbnailImagePath { get; set; }

        [ForeignKey("MediaTypeId")]         // this tells to eneity framework that the MediaTypeId property represents foreign key from mediatype class
        public virtual ICollection<CategoryItem> CategoryItems { get; set; }
    }
}
