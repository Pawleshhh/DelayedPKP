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
                double arrival = ArrivalDelay.Value.TotalMinutes;
                double departure = DepartureDelay.Value.TotalMinutes;

                return GetArrivalDelayLevel(arrival) | GetDepartureDelayLevel(departure);
            }
        }

        /// <summary>
        /// Gets and sets time of the planned arrival
        /// </summary>
        public TimeSpan? PlannedArrival { get; set; }

        /// <summary>
        /// Gets and sets delay time
        /// </summary>
        public TimeSpan? ArrivalDelay { get; set; }

        /// <summary>
        /// Gets and sets planned departure of this train.
        /// </summary>
        public TimeSpan? PlannedDeparture { get; set; }

        /// <summary>
        /// Gets and sets departure delay of this train.
        /// </summary>
        public TimeSpan? DepartureDelay { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Helper method to indicate delay level of the arrival.
        /// </summary>
        /// <param name="arrival">The delay time of the arrival</param>
        /// <returns></returns>
        private DelayLevel GetArrivalDelayLevel(double arrival)
        {
            if (arrival < 0) return DelayLevel.MinusArrival;

            if (arrival == 0) return DelayLevel.ZeroArrival;

            if (arrival > 0 && arrival < 20) return DelayLevel.LowArrival;

            if (arrival >= 20 && arrival < 60) return DelayLevel.MediumArrival;

            if (arrival >= 60) return DelayLevel.HighArrival;

            throw new ArgumentException("Arrival time is not recognized.", "arrival");
        }

        /// <summary>
        /// Helper method to indicate delay level of the departure.
        /// </summary>
        /// <param name="arrival">The delay time of the departure</param>
        /// <returns></returns>
        private DelayLevel GetDepartureDelayLevel(double departure)
        {
            if (departure < 0) return DelayLevel.MinusArrival;

            if (departure == 0) return DelayLevel.ZeroDeparture;

            if (departure > 0 && departure < 20) return DelayLevel.LowArrival;

            if (departure >= 20 && departure < 60) return DelayLevel.MediumArrival;

            if (departure >= 60) return DelayLevel.HighArrival;

            throw new ArgumentException("Departure time is not recognized.", "departure");
        }

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

            if (Date == other.Date && PlannedArrival != null) return PlannedArrival.Value.CompareTo(other.PlannedArrival);
            else if (Date == other.Date && PlannedDeparture != null) return PlannedDeparture.Value.CompareTo(other.PlannedDeparture);
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
            return "Delay time of the " + Train.ToString();
        }

        #endregion

        #region Static helper methods

        /// <summary>
        /// Gets new delay info by given variables.
        /// </summary>
        public static IDelayInfo<TBy> GetSingleDelayInfo(TBy byData, Train train, Station station, string from, string destination,
                                                        DateTime date, TimeSpan? plannedArr, TimeSpan? arrDelay,
                                                        TimeSpan? plannedDepar, TimeSpan? deparDelay)
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
