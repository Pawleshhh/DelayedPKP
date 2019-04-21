using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{
    public struct Station : IEquatable<Station>, IComparable<Station>
    {

        #region Constructors
        
        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="name">Name of the station.</param>
        /// <param name="id">ID of the station</param>
        public Station(string name, string id)
        {
            StationName = name ?? throw new ArgumentNullException("name");
            //Replace unnecessary single quote with empty string.
            StationID = id.Replace("'", "") ?? throw new ArgumentNullException("id");

            StationAdress = new Uri(PkpPageInfo.DelayedTrainsAdress.AbsoluteUri + StationID);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the station
        /// </summary>
        public string StationName { get; }

        /// <summary>
        /// Gets the name of the station
        /// </summary>
        public string StationID { get; }

        /// <summary>
        /// Gets the adress of the station.
        /// </summary>
        public Uri StationAdress { get; }

        #endregion

        #region IEquatable & IComparable

        public int CompareTo(Station other)
        {
            if (Equals(other)) return 0;

            return StationName.CompareTo(other.StationName);
        }

        public bool Equals(Station other)
        {
            return StationID == other.StationID;
        }

        #endregion

        #region Overridden from object

        public override bool Equals(object obj)
        {
            if (!(obj is Station)) return false;

            return Equals((Station)obj);
        }

        public override int GetHashCode()
        {
            return StationName.GetHashCode() * 7 + StationID.GetHashCode();
        }

        public override string ToString()
        {
            return StationName;
        }

        #endregion

        #region Operators

        public static bool operator ==(Station s1, Station s2)
        {
            return s1.Equals(s2);
        }

        public static bool operator !=(Station s1, Station s2)
        {
            return !s1.Equals(s2);
        }

        #endregion

    }
}
