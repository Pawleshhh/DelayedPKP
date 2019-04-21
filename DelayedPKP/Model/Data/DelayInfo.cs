using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{

    /// <summary>
    /// Class that contains information about delayed trains.
    /// </summary>
    public class DelayInfo<TBy> : IDelayInfo<TBy>
        where TBy : IEquatable<TBy>, IComparable<TBy>
    {

        #region Construtors

        #endregion

        #region Properties
        
        public TBy ByData { get; set; }

        /// <summary>
        /// Gets and sets station of this information.
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        /// Gets and sets train of this delay.
        /// </summary>
        public Train Train { get; set; }

        /// <summary>
        /// Gets and sets date of this information.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets the initial station 
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets and sets the final station
        /// </summary>
        public string Destination { get; set; }
        
        /// <summary>
        /// Gets the concated <see cref="From"/> with <see cref="Destination"/>
        /// </summary>
        public string Relation => $"{From}-{Destination}";

        /// <summary>
        /// Gets the level of delay time of this info
        /// </summary>
        public DelayLevel DelayLevel
        {
            get
            {
                if (ArrivalDelay.TotalMinutes < 0 || DepartureDelay.TotalMinutes < 0)
                    return DelayLevel.Minus;
                else if ((ArrivalDelay.TotalMinutes > 0 && ArrivalDelay.TotalMinutes < 20) ||
                          (DepartureDelay.TotalMinutes > 0 && DepartureDelay.TotalMinutes < 20))
                    return DelayLevel.Low;
                else if ((ArrivalDelay.TotalMinutes >= 20 && ArrivalDelay.TotalMinutes < 60) ||
                         (DepartureDelay.TotalMinutes >= 20 && DepartureDelay.TotalMinutes < 60))
                    return DelayLevel.Medium;
                else if (ArrivalDelay.TotalMinutes >= 60 || DepartureDelay.TotalMinutes >= 60)
                    return DelayLevel.High;
                else
                    return DelayLevel.Zero;
            }
        }

        /// <summary>
        /// Gets and sets time of the planned arrival
        /// </summary>
        public TimeSpan PlannedArrival { get; set; }

        /// <summary>
        /// Gets and sets delay time
        /// </summary>
        public TimeSpan ArrivalDelay { get; set; }

        /// <summary>
        /// Gets and sets planned departure of this train.
        /// </summary>
        public TimeSpan PlannedDeparture { get; set; }

        /// <summary>
        /// Gets and sets departure delay of this train.
        /// </summary>
        public TimeSpan DepartureDelay { get; set; }

        #endregion

        #region Methods

        #endregion

        #region IEquatable & IComparable

        public int CompareTo(IDelayInfo<TBy> other)
        {
            return CompareTo((IDelayInfoBase)other);
        }

        public bool Equals(IDelayInfo<TBy> other)
        {
            return Equals((IDelayInfoBase)other);
        }

        public bool Equals(IDelayInfoBase other)
        {
            return Train == other.Train && Date == other.Date &&
                 From == other.From && Destination == other.Destination;
        }

        public int CompareTo(IDelayInfoBase other)
        {
            if (Equals(other)) return 0;

            if (Date == other.Date) return PlannedArrival.CompareTo(other.PlannedArrival);
            else return Date.CompareTo(other.Date);
        }

        #endregion

        #region Overridden from object

        public override bool Equals(object obj)
        {
            if (!(obj is DelayInfo<TBy>)) return false;

            return Equals((DelayInfo<TBy>)obj);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode() * 7 + PlannedArrival.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Date.ToString("dd-MM-yyyy")}|{Relation}|{Station}|{PlannedArrival}|" +
                $"{ArrivalDelay.TotalMinutes} min|{PlannedDeparture}|{DepartureDelay.TotalMinutes} min";
        }

        #endregion

        #region Static helper methods

        /// <summary>
        /// Gets new delay info by given variables.
        /// </summary>
        public static IDelayInfo<TBy> GetSingleDelayInfo(TBy byData, Train train, Station station, string from, string destination,
                                                        DateTime date, TimeSpan plannedArr, TimeSpan arrDelay,
                                                        TimeSpan plannedDepar, TimeSpan deparDelay)
        {
            return new DelayInfo<TBy>()
            {
                Train = train, Station = station, ByData = byData,
                Date = date, From = from, Destination = destination,
                PlannedArrival = plannedArr, ArrivalDelay = arrDelay,
                PlannedDeparture = plannedDepar, DepartureDelay = deparDelay
            };
        }

        #endregion
    }

}
