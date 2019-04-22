using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{
    using DelayedPKP.Model;
    using System.Collections.ObjectModel;

    public class InfoBoardViewModel<TBy> : ObservedClass where TBy : IEquatable<TBy>, IComparable<TBy>
    {

        #region Constructors

        public InfoBoardViewModel() { }

        public InfoBoardViewModel(IEnumerable<IDelayInfo<TBy>> collection)
        {
            LoadBoard(collection);
        }

        public InfoBoardViewModel(DelayInfoCollection<TBy, IDelayInfo<TBy>> collection)
        {
            LoadBoard(collection);
        }

        #endregion

        #region Fields

        protected DelayInfoCollection<TBy, IDelayInfo<TBy>> delayColl;

        #endregion

        #region Properties

        public TBy ByData => delayColl.ByData;

        public ObservableCollection<IDelayInfoViewModel<TBy>> Collection { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads delay info collection
        /// </summary>
        public void LoadBoard(IEnumerable<IDelayInfo<TBy>> collection)
        {
            LoadBoard(ByData, collection);
        }

        /// <summary>
        /// Loads delay info collection
        /// </summary>
        public void LoadBoard(TBy byData, IEnumerable<IDelayInfo<TBy>> collection)
        {
            delayColl = new DelayInfoCollection<TBy, IDelayInfo<TBy>>(byData, collection);
            SetDelayCollection(collection);
        }

        /// <summary>
        /// Loads delay info collection
        /// </summary>
        public void LoadBoard(DelayInfoCollection<TBy, IDelayInfo<TBy>> collection)
        {
            delayColl = new DelayInfoCollection<TBy, IDelayInfo<TBy>>(collection.ByData, collection);
            SetDelayCollection(collection);
        }

        /// <summary>
        /// Sets the collection with data.
        /// </summary>
        private void SetDelayCollection(IEnumerable<IDelayInfo<TBy>> collection)
        {
            Collection = new ObservableCollection<IDelayInfoViewModel<TBy>>(
                collection.Select(d => new DelayInfoViewModel<TBy>(d)));
        }

        #endregion

        #region Commands

        #endregion

    }
}
