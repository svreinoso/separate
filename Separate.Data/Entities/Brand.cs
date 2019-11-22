using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Separate.Data.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Logo is required.")]
        public string Logo { get; set; }
        public List<Model> Models { get; set; }
    }
}
