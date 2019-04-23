using DelayedPKP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.ViewModel
{
    public class StationBoardViewModel : ObservedClass, IViewModel
    {

        #region Constructors

        public StationBoardViewModel(IViewModel parent) { }

        public StationBoardViewModel(string pattern, IEnumerable<StationViewModel> collection)
        {
            LoadStations(pattern, collection);
        }

        public StationBoardViewModel(string pattern, List<StationViewModel> list)
        {
            LoadStations(pattern, list);
        }

        #endregion

        #region Model

        private List<StationViewModel> stationColl = new List<StationViewModel>();

        #endregion

        #region Properties

        public ObservableCollection<StationViewModel> Collection { get; private set; }

        public string ProvidedPattern { get; private set; }

        public IViewModel ParentViewModel { get; private set; }

        public ErrorViewModel ErrorViewModel => ParentViewModel.ErrorViewModel ?? null;

        #endregion

        #region Methods

        public void LoadStations(string pattern, IEnumerable<StationViewModel> collection)
        {
            LoadStations(pattern, collection.ToList());
        }

        public void LoadStations(string pattern, List<StationViewModel> list)
        {
            stationColl = list;
            ProvidedPattern = pattern;
            SetStationCollection(list);
        }

        private void SetStationCollection(IEnumerable<StationViewModel> collection)
        {
            Collection = new ObservableCollection<StationViewModel>(collection);
        }

        #endregion

        #region Commands

        #endregion

    }
}
