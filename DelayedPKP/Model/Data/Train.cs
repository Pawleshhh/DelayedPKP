using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{
    public struct Train : IEquatable<Train>, IComparable<Train>
    {

        #region Constructors

        /// <summary>
        /// Creates struct of a train
        /// </summary>
        /// <param name="name">Name of a train.</param>
        /// <param name="id">ID of a train.</param>
        /// <param name="host">Host of a train.</param>
        public Train(string name, string id, string host)
        {
            Name = name; ID = id; Host = host;

            string adress = PkpPageInfo.TrainAdress + ID;

            Adress = new Uri(adress);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of this train
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ID of this train.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Host of this train.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Adress of this train on the page.
        /// </summary>
        public Uri Adress { get; }

        #endregion

        #region Methods

        public int CompareTo(Train other)
        {
            if (Equals(other)) return 0;

            return ID.CompareTo(other.ID);
        }

        public bool Equals(Train other)
        {
            return ID == other.ID;
        }

        #endregion

        #region Overridden from object

        public override bool Equals(object obj)
        {
            if (!(obj is Train)) return false;

            return Equals((Train)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() * 7 + Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} ({ID})";
        }

        #endregion

        #region Operators

        public static bool operator ==(Train t1, Train t2)
        {
            return t1.ID == t2.ID;
        }

        public static bool operator !=(Train t1, Train t2)
        {
            return t1.ID != t2.ID;
        }

        #endregion
    }
}
