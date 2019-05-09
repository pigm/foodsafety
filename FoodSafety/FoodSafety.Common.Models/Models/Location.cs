﻿using Realms;

namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Location.
    /// </summary>
    public class Location
    {
        public string region { get; set; }
        public string city { get; set; }
        public string commune { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
    }
}