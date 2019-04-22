using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{
    public interface IDelayInfoViewModel<TBy> : IDelayInfoViewModelBase where TBy : IEquatable<TBy>, IComparable<TBy>
    {

        TBy ByData { get; }

    }
}
