using System;
using System.Collections.Generic;
using System.Text;

namespace Separate.Models
{
    public class CarDto
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int ReleaseYear { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ModelName { get; set; }
        public string BrandName { get; set; }
    }
}
