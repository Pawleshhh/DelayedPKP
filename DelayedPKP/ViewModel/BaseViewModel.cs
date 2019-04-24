namespace DelayedPKP.ViewModel
{
    using HtmlAgilityPack;
    using Model;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Base View Model of this app.
    /// </summary>
    public class BaseViewModel : ObservedClass, IViewModel
    {

        #region Constructors

        /// <summary>
        /// Creates instatnce of the base view model.
        /// </summary>
        public BaseViewModel()
        {
            pageReader = new DelayedTrainsPageReader();
            InitializeRest();
        }

        /// <summary>
        /// Creates instance of the base view model.
        /// </summary>
        /// <param name="station">Default station if none will be provided.</param>
        public BaseViewModel(string station)
        {
            pageReader = new DelayedTrainsPageReader(station);
            InitializeRest();
        }

        private void InitializeRest()
        {
            ErrorViewModel = new ErrorViewModel(this);
            ErrorViewModel.ExceptionOccurred += ErrorViewModel_ExceptionOccurred;
            ErrorViewModel.ExceptionHanled += ErrorViewModel_ExceptionHanled;

            StationBoard = new StationBoardViewModel(this);
            InfoBoard_ByStation = new InfoBoardViewModel<Station>(this);
            InfoBoard_ByTrain = new InfoBoardViewModel<Train>(this);
        }

        #endregion

        #region Model

        /// <summary>
        /// Page reader to get data from the site.
        /// </summary>
        DelayedTrainsPageReader pageReader;

        #endregion

        #region Other view models

        IViewModel IViewModel.ParentViewModel => null;

        /// <summary>
        /// Gets the error view model of this view model.
        /// </summary>
        public ErrorViewModel ErrorViewModel { get; private set; }

        #endregion

        #region Boards

        /// <summary>
        /// Gets stations that have been loaded.
        /// </summary>
        public StationBoardViewModel StationBoard
        { get; private set; }

        /// <summary>
        /// Gets collection of delay time by station.
        /// </summary>
        public InfoBoardViewModel<Station> InfoBoard_ByStation
        { get; private set; }

        /// <summary>
        /// Gets collection of delay time by train.
        /// </summary>
        public InfoBoardViewModel<Train> InfoBoard_ByTrain
        { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the path of the app icon.
        /// </summary>
        public string IconPath { get; set; } = "/DelayedPKP;component/Images/Icons/logo_pkp.png";

        /// <summary>
        /// Gets and sets the title of the app.
        /// </summary>
        public string AppTitle { get; set; } = "Delayed PKP";

        /// <summary>
        /// Gets and sets the station name that was provided.
        /// </summary>
        public string StationText { get; set; }

        /// <summary>
        /// Gets and sets the visibility of the search control.
        /// </summary>
        public bool SearchControlVisibility { get; set; } = true;

        /// <summary>
        /// Indicates whether the station board (page) is on or not.
        /// </summary>
        public bool IsStationBoardOn { get; private set; } = false;

        /// <summary>
        /// Indicates whether the info board (page) by station is on or not.
        /// </summary>
        public bool IsInfoBoardByStationOn { get; private set; } = false;

        /// <summary>
        /// Indicates whether the info board (page) by train is on or not.
        /// </summary>
        public bool IsInfoBoardByTrainOn { get; private set; } = false;

        /// <summary>
        /// Gets and sets visibility of the main frame.
        /// </summary>
        public bool IsMainFrameVisible { get; set; } = false;

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the current data.
        /// </summary>
        public void Refresh()
        {

            if(IsStationBoardOn)
            {
                LoadStations(StationBoard.ProvidedPattern);
            }
            else if (IsInfoBoardByStationOn)
            {
                LoadInfoBoardByStation(new StationViewModel(InfoBoard_ByStation.ByData));
            }
            else if (IsInfoBoardByTrainOn)
            {
                LoadInfoBoardByTrain(new TrainViewModel(InfoBoard_ByTrain.ByData));
            }

        }

        /// <summary>
        /// Loads stations to the station board.
        /// </summary>
        public void LoadStations(string stationPattern)
        {
            if (ErrorViewModel.IsExceptionCaughtCurrently)
                ErrorViewModel.ClearException();

            DoAndHandleExceptions(() =>
            {
                StationBoard.LoadStations(stationPattern, pageReader.GetStations(stationPattern).
                    Select(station => new StationViewModel(station)));
            });

            IsStationBoardOn = true;
            IsInfoBoardByStationOn = IsInfoBoardByTrainOn = false;
        }

        /// <summary>
        /// Loads delay info to the info board by station.
        /// </summary>
        public void LoadInfoBoardByStation(StationViewModel station)
        {
            DoAndHandleExceptions(() =>
            {
                InfoBoard_ByStation.LoadBoard(pageReader.GetStationDelayInfos(station.GetCopyOfStation));
            });

            IsInfoBoardByStationOn = true;
            IsStationBoardOn = IsInfoBoardByTrainOn = false;
        }

        /// <summary>
        /// Loads delay info to the info board by train.
        /// </summary>
        /// <param name="train"></param>
        public void LoadInfoBoardByTrain(TrainViewModel train)
        {
            DoAndHandleExceptions(() =>
            {
                InfoBoard_ByTrain.LoadBoard(pageReader.GetTrainDelayInfos(train.GetCopyOfTrain));
            });

            IsInfoBoardByTrainOn = true;
            IsStationBoardOn = IsInfoBoardByStationOn = false;
        }

        private void ErrorViewModel_ExceptionOccurred(object sender, EventArgs e)
        {
            IsMainFrameVisible = false;
        }

        private void ErrorViewModel_ExceptionHanled(object sender, EventArgs e)
        {
            IsMainFrameVisible = true;
        }

        /// <summary>
        /// Helper method that does specific action and handles exceptions that may occur.
        /// </summary>
        /// <param name="action">Action to do.</param>
        private void DoAndHandleExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (NodeNotFoundException ex)
            {
                ErrorViewModel.CallException(ex, "Data was not found");
            }
            catch (NodeAttributeNotFoundException ex)
            {
                ErrorViewModel.CallException(ex, "Data was not found");
            }
            catch (HtmlWebException ex)
            {
                ErrorViewModel.CallException(ex, "No connection with the Internet.");
            }
            catch (ArgumentException ex)
            {
                ErrorViewModel.CallException(ex, "The data you provided may be invaild. Try again.");
            }
            catch (Exception ex)
            {
                ErrorViewModel.CallException(ex, "Some problems occurred. Sorry, try again.");
            }
        }

        #endregion

        #region Commands

        private ICommand loadStations;

        /// <summary>
        /// Gets the command of loading the station board.
        /// </summary>
        public ICommand LoadStationsCommand
        {
            get
            {
                if (loadStations == null)
                    loadStations = new RelayCommand(
                        o =>
                        {
                            LoadStations(StationText);
                        });

                return loadStations;
            }
        }

        private ICommand loadInfoBoardByStation;

        /// <summary>
        /// Gets the command of loading the info board by station.
        /// </summary>
        public ICommand LoadInfoBoardByStationCommand
        {
            get
            {
                if (loadInfoBoardByStation == null)
                    loadInfoBoardByStation = new RelayCommand(
                        o =>
                        {
                            if (!(o is StationViewModel))
                            {
                                ErrorViewModel.CallException(new ArgumentException(), "Some problems occurred. Sorry, try again.");
                                return;
                            }

                            LoadInfoBoardByStation((StationViewModel)o);
                        });

                return loadInfoBoardByStation;
            }
        }

        private ICommand loadInfoBoardByTrain;

        /// <summary>
        /// Gets the command of loading the info board by train.
        /// </summary>
        public ICommand LoadInfoBoardByTrainCommand
        {
            get
            {
                if (loadInfoBoardByTrain == null)
                    loadInfoBoardByTrain = new RelayCommand(
                        o =>
                        {
                            if (!(o is TrainViewModel))
                            {
                                ErrorViewModel.CallException(new ArgumentException(), "Some problems occurred. Sorry, try again.");
                                return;
                            }

                            LoadInfoBoardByTrain((TrainViewModel)o);
                        });

                return loadInfoBoardByTrain;
            }
        }

        private ICommand refresh;

        /// <summary>
        /// Gets the command of refreshing current data.
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                if (refresh == null)
                    refresh = new RelayCommand(
                        o =>
                        {
                            Refresh();
                        });

                return refresh;
            }
        }

        private ICommand confirmError;

        public ICommand ConfirmErrorCommand
        {
            get
            {
                if (confirmError == null)
                    confirmError = new RelayCommand(
                        o =>
                        {
                            ErrorViewModel.ClearException();
                        });

                return confirmError;
            }
        }

        #endregion

    }
}
