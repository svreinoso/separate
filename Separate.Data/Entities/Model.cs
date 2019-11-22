using DataAnnotationsExtensions;

namespace Separate.Data.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Min(1, ErrorMessage = "The Brand is required")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
