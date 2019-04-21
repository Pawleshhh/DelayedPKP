using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{
    public interface IDelayInfo<TBy> : IDelayInfoBase, IEquatable<IDelayInfo<TBy>>, IComparable<IDelayInfo<TBy>>
        where TBy : IEquatable<TBy>, IComparable<TBy>
    {
        
        TBy ByData { get; set; }

    }
}
