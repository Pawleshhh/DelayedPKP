using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DelayedPKP
{

    /// <summary>
    /// Interface for the controls that are connected with a frame.
    /// </summary>
    public interface IConnectedWithFrame
    {

        Frame Frame { get; set; }

    }
}
