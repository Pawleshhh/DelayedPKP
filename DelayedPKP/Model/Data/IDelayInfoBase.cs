using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{
    public interface IDelayInfoBase : IEquatable<IDelayInfoBase>, IComparable<IDelayInfoBase>
    {

        /// <summary>
        /// Gets and sets station of this information.
        /// </summary>
        Station Station { get; set; }

        /// <summary>
        /// Gets and sets train of this delay.
        /// </summary>
        Train Train { get; set; }

        /// <summary>
        /// Gets and sets date of this information.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets the initial station 
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// Gets and sets the final station
        /// </summary>
        string Destination { get; set; }

        /// <summary>
        /// Gets the concated <see cref="From"/> with <see cref="Destination"/>
        /// </summary>
        string Relation { get; }

        /// <summary>
        /// Gets the level of delay time of this info
        /// </summary>
        DelayLevel DelayLevel { get; }

        /// <summary>
        /// Gets and sets time of the planned arrival
        /// </summary>
        TimeSpan? PlannedArrival { get; set; }

        /// <summary>
        /// Gets and sets delay time
        /// </summary>
        TimeSpan? ArrivalDelay { get; set; }

        /// <summary>
        /// Gets and sets planned departure of this train.
        /// </summary>
        TimeSpan? PlannedDeparture { get; set; }

        /// <summary>
        /// Gets and sets departure delay of this train.
        /// </summary>
        TimeSpan? DepartureDelay { get; set; }

    }

    [Flags]
    public enum DelayLevel
    {
        MinusArrival = 0, MinusDeparture = 1, ZeroArrival = 2, ZeroDeparture = 4, LowArrival = 8, LowDeparture = 16,
        MediumArrival = 32, MediumDeparture = 64, HighArrival = 128, HighDeparture = 258
    }

}
