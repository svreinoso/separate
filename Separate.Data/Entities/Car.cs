using DataAnnotationsExtensions;

namespace Separate.Data.Entities
{
    public class Car
    {
        public int Id { get; set; }
        [Min(1, ErrorMessage = "The model is required")]
        public int ModelId { get; set; }
        public Model Model { get; set; }
        [Min(1, ErrorMessage = "The Year is required")]
        public int ReleaseYear { get; set; }
        [Min(1, ErrorMessage = "The Price is required")]
        public int Price { get; set; }
        public string Description { get; set; }
    }
}
