using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepsWebApp_Tests
{
    /// <summary>
    /// Internal wrapped error
    /// </summary>
    public class Error
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
