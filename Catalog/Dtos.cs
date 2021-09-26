using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Dtos
{
    public class Dtos
    {

        public record ItemDto (int CatId , string CatName ,DateTime CDate ,  double Price ,String Remarks); 

        public record CreateItemDto([Required] int CatId, [Required] string CatName, double Price, String Remarks);

        public record UpdateItemDto([Required] string CatName, double Price, String Remarks);

        public record DeleteItemDto(String CatId);


    }
}
