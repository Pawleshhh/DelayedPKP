using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{

    using Model;

    public class DelayInfoViewModel<TBy> : ObservedClass, IDelayInfoViewModel<TBy>,
        IEquatable<DelayInfoViewModel<TBy>>, IComparable<DelayInfoViewModel<TBy>>
        where TBy : IEquatable<TBy>, IComparable<TBy>
    {

        #region Constructors

        public DelayInfoViewModel(TBy byData, StationViewModel station, TrainViewModel train,
            DateTime date, string from, string destination,
            TimeSpan? plannedArrival, TimeSpan? arrivalDelay,
            TimeSpan? plannedDeparture, TimeSpan? departureDelay)
        {
            Station = station;
            Train = train;

            delayInfo = new DelayInfo<TBy>()
            {
                ByData = byData,
                Station = new Station(station.StationName, station.StationID),
                Train = new Train(train.Name, train.ID, train.Host),
                Date = date, From = from, Destination = destination,
                PlannedArrival = plannedArrival, ArrivalDelay = arrivalDelay,
                PlannedDeparture = plannedDeparture, DepartureDelay = departureDelay
            };
        }

        public DelayInfoViewModel(IDelayInfo<TBy> delayInfo) :
            this(delayInfo.ByData, new StationViewModel(delayInfo.Station), new TrainViewModel(delayInfo.Train),
                delayInfo.Date, delayInfo.From, delayInfo.Destination,
                delayInfo.PlannedArrival, delayInfo.ArrivalDelay,
                delayInfo.PlannedDeparture, delayInfo.DepartureDelay)
        { }

        #endregion

        #region Model

        private IDelayInfo<TBy> delayInfo;

        #endregion

        #region Properties

        /// <summary>
        /// Data that this delay info is shown in regard of.
        /// </summary>
        public TBy ByData { get; }

        /// <summary>
        /// Gets station
        /// </summary>
        public StationViewModel Station { get; }

        /// <summary>
        /// Gets train
        /// </summary>
        public TrainViewModel Train { get; }

        /// <summary>
        /// Gets date of this delay info.
        /// </summary>
        public DateTime Date => delayInfo.Date;

        /// <summary>
        /// Gets the first station's name.
        /// </summary>
        public string From => delayInfo.From;

        /// <summary>
        /// Gets the last station's name.
        /// </summary>
        public string Destination => delayInfo.Destination;

        /// <summary>
        /// Gets relation [From - Destination]
        /// </summary>
        public string Relation => delayInfo.Relation;

        /// <summary>
        /// Gets the level of the delay times.
        /// </summary>
        public DelayLevel DelayLevel => delayInfo.DelayLevel;

        /// <summary>
        /// Gets planned time of the arrival.
        /// </summary>
        public TimeSpan? PlannedArrival => delayInfo.PlannedArrival;

        /// <summary>
        /// Gets delay time of the arrival.
        /// </summary>
        public TimeSpan? ArrivalDelay => delayInfo.ArrivalDelay;

        /// <summary>
        /// Gets planned time of the departure.
        /// </summary>
        public TimeSpan? PlannedDeparture => delayInfo.PlannedDeparture;

        /// <summary>
        /// Gets delay time of the departure.
        /// </summary>
        public TimeSpan? DepartureDelay => delayInfo.DepartureDelay;

        #endregion

        #region Methods

        public int CompareTo(DelayInfoViewModel<TBy> other)
        {
            return delayInfo.CompareTo(other.delayInfo);
        }

        public bool Equals(DelayInfoViewModel<TBy> other)
        {
            return delayInfo.Equals(other.delayInfo);
        }

        #endregion

        #region Overridden from object

        public override bool Equals(object obj)
        {
            if (!(obj is DelayInfoViewModel<TBy>)) return false;

            return Equals((DelayInfoViewModel<TBy>)obj);
        }

        public override int GetHashCode()
        {
            return delayInfo.GetHashCode() * 5;
        }

        public override string ToString()
        {
            return delayInfo.ToString();
        }

        #endregion

    }
}

