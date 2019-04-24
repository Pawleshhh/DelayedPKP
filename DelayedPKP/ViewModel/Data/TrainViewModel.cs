using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{

    using Model;

    public class TrainViewModel : ObservedClass,
        IEquatable<TrainViewModel>, IComparable<TrainViewModel>
    {

        #region Constructors

        public TrainViewModel(string name, string id, string host)
        {
            train = new Train(name, id, host);
        }

        public TrainViewModel(Train train)
        {
            this.train = train;
        }

        #endregion

        #region Model

        Train train;

        #endregion

        #region Properties

        public Train GetCopyOfTrain => train;

        public string Name => train.Name;

        public string ID => train.ID;

        public string Host => train.Host;

        public Uri Adress => train.Adress;

        #endregion

        #region Methods

        public int CompareTo(TrainViewModel other)
        {
            return train.CompareTo(other.train);
        }

        public bool Equals(TrainViewModel other)
        {
            return train.Equals(other.train);
        }

        #endregion

        #region Overridden form object

        public override bool Equals(object obj)
        {
            if (!(obj is TrainViewModel)) return false;

            return Equals((TrainViewModel)obj);
        }

        public override int GetHashCode()
        {
            return train.GetHashCode();
        }

        public override string ToString()
        {
            return train.ToString();
        }

        #endregion

    }
}
