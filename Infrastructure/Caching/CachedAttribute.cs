using System;

namespace Infrastructure.Caching
{
    public class CachedAttribute: Attribute
    {
        public string Key { get; set; }
        public int DurationInMinutes { get; set; }
    }
}