using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepsWebApp.Models
{
    /// <summary>
    /// Internal wrapped error
    /// </summary>
    public class ProcessedError
    {
        /// <summary>
        /// Error custom status code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string  Message { get; set; }
    }
}
