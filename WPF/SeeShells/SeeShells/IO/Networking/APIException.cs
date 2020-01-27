using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SeeShells.IO.Networking
{
    /// <summary>
    /// Wrapper Exception for when something goes wrong during API operation
    /// </summary>
    public class APIException : IOException
    {
        public APIException(string message) : base(message) { }
        public APIException(string message, Exception innerException) : base(message, innerException) { }
    }
}
