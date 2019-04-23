using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{
    using Model;

    public class StationViewModel : ObservedClass,
        IEquatable<StationViewModel>, IComparable<StationViewModel>
    {

        #region Constructors

        public StationViewModel(string name, string id)
        {
            station = new Station(name, id);
        }

        public StationViewModel(Station station)
        {
            this.station = station;
        }

        #endregion

        #region Model

        private Station station;

        #endregion

        #region Properties

        public Station GetCopyOfStation => station;

        public string StationName => station.StationName;

        public string StationID => station.StationID;

        public Uri StationAdress => station.StationAdress;

        #endregion

        #region Methods

        public int CompareTo(StationViewModel other)
        {
            return station.CompareTo(other.station);
        }

        public bool Equals(StationViewModel other)
        {
            return station.Equals(other.station);
        }

        #endregion

        #region Overridden from object

        public override bool Equals(object obj)
        {
            if (!(obj is StationViewModel)) return false;

            return Equals((StationViewModel)obj);
        }

        public override int GetHashCode()
        {
            return station.GetHashCode();
        }

        public override string ToString()
        {
            return station.ToString();
        }

        #endregion

    }
}
