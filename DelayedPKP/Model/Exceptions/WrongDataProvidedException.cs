using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{

    /// <summary>
    /// Exception that occurs when data that has been provided is not correct.
    /// </summary>
    public class WrongDataProvidedException : Exception
    {

        public WrongDataProvidedException() : base() { }

        public WrongDataProvidedException(string message) : base(message) { }

        public WrongDataProvidedException(string message, Exception inner) : base(message, inner) { }

    }
}
