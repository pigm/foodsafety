using System;
using System.Collections.Generic;
using Realms;

namespace FoodSafety.Common.Models.Models
{
    public class ItemsCarniceria: RealmObject
    {
        public List<EntidadItemCarniceria> Productos { get; set; }
        public int diaCarga { get; set; }
    }
}
