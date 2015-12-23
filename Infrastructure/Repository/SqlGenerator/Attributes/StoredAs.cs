using System;

namespace Infrastructure.Repository.SqlGenerator.Attributes
{
    public class StoredAs : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public StoredAs(string value)
        {
            this.Value = value;
        }
    }
}