using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharpMember.Core.Data.Models
{
    public class GlobalSettings
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// The message that users can see when they login the system.
        /// </summary>
        //public string Notification { get; set; }
    }
}
