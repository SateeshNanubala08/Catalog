using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Entities
{
    public class Items
    {
        [Key]
        public int CatId { get; set; }
        [Required]
        public String CatName { get; set; }

        [Range(1, 1000)]
        public Double Price { get; set; }
         
        public DateTime CDate { get; set; }
        public string Remarks { get; set; }
    }
}
