using DelayedPKP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{
    public interface IDelayInfoViewModelBase
    {

        StationViewModel Station { get; }

        TrainViewModel Train { get; }

        DateTime Date { get; }

        string From { get; }

        string Destination { get; }

        string Relation { get; }

        DelayLevel DelayLevel { get; }

        TimeSpan? PlannedArrival { get; }

        TimeSpan? ArrivalDelay { get; }

        TimeSpan? PlannedDeparture { get; }

        TimeSpan? DepartureDelay { get; }

    }
}
