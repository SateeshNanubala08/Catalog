using Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Catalog.Dtos.Dtos;

namespace Catalog
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Items items)
        {
            return new ItemDto(items.CatId, items.CatName, items.CDate, items.Price, items.Remarks);
        }
    }
}
