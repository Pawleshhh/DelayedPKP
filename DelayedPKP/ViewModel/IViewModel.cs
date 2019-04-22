using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{

    /// <summary>
    /// Interface that every view model of this app implements.
    /// </summary>
    public interface IViewModel
    {

        /// <summary>
        /// View model that contains 
        /// </summary>
        IViewModel ParentViewModel { get; }

        /// <summary>
        /// View model of handling exceptions.
        /// </summary>
        ErrorViewModel ErrorViewModel { get; }

    }
}
