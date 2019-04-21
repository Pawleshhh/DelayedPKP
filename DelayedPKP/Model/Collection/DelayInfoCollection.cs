using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{
    public class DelayInfoCollection<TBy, TDelayData> : IList<TDelayData>, IReadOnlyList<TDelayData>
        where TDelayData : IDelayInfo<TBy>
        where TBy : IEquatable<TBy>, IComparable<TBy>
    {
        
        #region Constructors

        public DelayInfoCollection(TBy byData)
        {
            ByData = byData;
        }

        public DelayInfoCollection(TBy byData, IEnumerable<TDelayData> collection)
            : this(byData)
        {
            foreach (TDelayData di in collection)
                Add(di);
        }

        #endregion

        #region Indexers

        public TDelayData this[int index]
        {
            get => coll_delayInfo[index];
            set
            {
                if (!CheckIfDataIsCorrect(value.ByData))
                    return;

                coll_delayInfo[index] = value;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Private collection of delay info.
        /// </summary>
        protected List<TDelayData> coll_delayInfo = new List<TDelayData>();

        #endregion

        #region Properties

        public TBy ByData { get; }

        public int Count => coll_delayInfo.Count;

        bool ICollection<TDelayData>.IsReadOnly => throw new NotImplementedException();

        #endregion

        #region Methods

        public void Add(TDelayData item)
        {
            if (!CheckIfDataIsCorrect(item.ByData))
                return;

            coll_delayInfo.Add(item);
        }

        public void Insert(int index, TDelayData item)
        {
            if (!CheckIfDataIsCorrect(item.ByData))
                return;

            coll_delayInfo.Insert(index, item);
        }

        public bool Remove(TDelayData item)
        {
            return coll_delayInfo.Remove(item);
        }

        public void RemoveAt(int index)
        {
            coll_delayInfo.RemoveAt(index);
        }

        public void Clear()
        {
            coll_delayInfo.Clear();
        }

        public bool Contains(TDelayData item)
        {
            return coll_delayInfo.Contains(item);
        }

        public int IndexOf(TDelayData item)
        {
            return coll_delayInfo.IndexOf(item);
        }

        public void CopyTo(TDelayData[] array, int arrayIndex)
        {
            coll_delayInfo.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TDelayData> GetEnumerator()
        {
            return coll_delayInfo.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Helper methods

        protected virtual bool CheckIfDataIsCorrect(IEquatable<TBy> byData) => ByData.Equals(byData);

        #endregion

    }
}
