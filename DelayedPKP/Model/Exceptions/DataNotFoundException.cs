using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{
    /// <summary>
    /// Exception that occurs when the data has not been found.
    /// </summary>
    public class DataNotFoundException : Exception
    {

        public DataNotFoundException() :base("Data was not found") { }

        public DataNotFoundException(Exception inner) : base("Data was not found", inner) { }

        public DataNotFoundException(string message) : base(message) { }

        public DataNotFoundException(string message, Exception inner) : base(message, inner) { }

    }
}
