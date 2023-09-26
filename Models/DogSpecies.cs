using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class DogSpecies : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? DogSpeciesId { get; set; }
        public string DogSpeciesName { get; set; }
        public bool? IsDeleted { get; set; }

        public ICollection<DogItem> dogItems { get; set; } = new List<DogItem>();

    }
}
