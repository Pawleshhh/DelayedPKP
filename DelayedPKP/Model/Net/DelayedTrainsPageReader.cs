using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{

    using System.Net;

    /// <summary>
    /// Class that provides information about delayed trains.
    /// </summary>
    public class DelayedTrainsPageReader
    {

        #region Constructors

        /// <summary>
        /// Creates an instance of this class setting <see cref="DefaultStation"/> as "Warszawa
        /// </summary>
        public DelayedTrainsPageReader() : this("Warszawa") { }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="defaultStation">Default station</param>
        public DelayedTrainsPageReader(string defaultStation)
        {
            DefaultStation = defaultStation;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets default station.
        /// </summary>
        public string DefaultStation { get; }

        /// <summary>
        /// Field to contain Polish culture information.
        /// </summary>
        private readonly CultureInfo CultureInfo = CultureInfo.GetCultureInfo("pl-PL");

        #endregion

        #region Methods

        /// <summary>
        /// Gets collection of the stations.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NodeNotFoundException"/>
        /// <exception cref="NodeAttributeNotFoundException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public IEnumerable<Station> GetStations()
        {
            return GetStations(DefaultStation);
        }

        /// <summary>
        /// Gets collection of the stations.
        /// </summary>
        /// <param name="station">Wanted station.</param>
        /// <returns></returns>
        /// <exception cref="NodeNotFoundException"/>
        /// <exception cref="NodeAttributeNotFoundException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="WebException"/>
        /// <exception cref="ArgumentException"/>
        public IEnumerable<Station> GetStations(string station)
        {
            //Get html document of the stations
            var doc = PkpPageInfo.GetHtmlDocumentOfStations(station);

            //Get table with all found stations
            HtmlNode table = GetTable(doc.DocumentNode);

            //Check if table is null or not
            if (table == null)
                throw new NodeNotFoundException("Table was not found");

            //Get all rows from the table
            foreach (HtmlNode row in table.SelectNodes("tr")
                ?? throw new NodeNotFoundException("Rows were not found"))
            {

                //Get all cells from the current row
                foreach (HtmlNode cell in row.SelectNodes("td") 
                    ?? throw new NodeNotFoundException("Cells were not found"))
                {
                    //Remove unnecessary white spaces.
                    string trimed = cell.InnerText.Trim();
                    
                    //Get value from onlick attribute
                    string id = cell.GetAttributeValue("onclick", "Not found");

                    //If it's not found then throw the exception
                    if (id == "Not found") throw new NodeAttributeNotFoundException("Onclick was not found");

                    //Get the train id from the value
                    id = id.Substring(id.IndexOf("id=") + 3);

                    //return station
                    yield return new Station(trimed, id);
                }
            }
        }

        /// <summary>
        /// Gets <see cref="DelayInfoCollection{TBy, TData}"/> in regard of the given station
        /// </summary>
        /// <exception cref="NodeNotFoundException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="RegexMatchTimeoutException"/>
        /// <exception cref="FormatException"/>
        /// <exception cref="OverflowException"/>
        /// <exception cref="WebException"/>
        public DelayInfoCollection<Station, IDelayInfo<Station>> GetStationDelayInfos(Station station)
        {
            //get html document of the trains by the given station with their delay time.
            var doc = PkpPageInfo.GetHtmlDocumentOfDelayedTrains(station.StationID);

            //get table that contains trains with delay time.
            HtmlNodeCollection tables = GetMultipleTables(doc.DocumentNode);

            //Helper variables
            string trainName = string.Empty, id = string.Empty, host = string.Empty, from = string.Empty, destination = string.Empty;
            DateTime date = default;
            TimeSpan? planned = default, delay = null;

            Action<HtmlNodeCollection> setVariables = cells =>
            {
                //Set helper variables with relevant data
                id = GetIDFromHtmlNode(cells[0]);
                trainName = ClearInnerText(cells[0].InnerText);
                host = ClearInnerText(cells[1].InnerText);
                date = GetDateTimeFromText(cells[2].InnerText, "yyyy-M-d");
                from = GetRelationsFromText(cells[3].InnerText, out destination);
                TryGetPlannedTimeFromText(cells[4].InnerText, out planned);
                TryGetDelayTimeFromText(cells[5].InnerText, out delay);
            };

            //Make function to get arrival data
            Func<HtmlNodeCollection, IDelayInfo<Station>> setArrivalData = cells =>
            {
                //Set helper variables with relevant data
                setVariables(cells);

                //Return delay info by given variables
                return DelayInfo<Station>.GetSingleDelayInfo(station, new Train(trainName, id, host), station, from, destination, date, planned, delay, default, default);
            };

            //Make function to get departure data
            Func<HtmlNodeCollection, IDelayInfo<Station>> setDepartureData = cells =>
            {
                //Set helper variables with relevant data
                setVariables(cells);

                //Return delay info by given variables
                return DelayInfo<Station>.GetSingleDelayInfo(station, new Train(trainName, id, host), station, from, destination, date, default, default, planned, delay);
            };

            //Get the arrival data
            IEnumerable<IDelayInfo<Station>> arrivalColl = GetDelayInfosFromTable(tables[0], setArrivalData);

            //Get the departure data
            IEnumerable<IDelayInfo<Station>> departureColl = GetDelayInfosFromTable(tables[1], setDepartureData);

            //return arrival data and departure data as one collection
            return new DelayInfoCollection<Station, IDelayInfo<Station>>(station, CompleteStationDelayInfo(arrivalColl, departureColl));
        }

        /// <summary>
        /// Gets <see cref="DelayInfoCollection{TBy, TData}"/> in regard of the given train.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="FormatException"/>
        /// <exception cref="RegexMatchTimeoutException"/>
        /// <exception cref="NodeNotFoundException"/>
        /// <exception cref="WebException"/>
        public DelayInfoCollection<Train, IDelayInfo<Train>> GetTrainDelayInfos(Train train)
        {
            //get html document of the trains with their delay time.
            var doc = PkpPageInfo.GetHtmlDocumentOfTrainPage(train.ID);

            //get table that contains delay time of the given train.
            HtmlNode table = GetTable(doc.DocumentNode);

            //Helper variables
            string stationName, stationID, from, destination;
            DateTime date;
            TimeSpan? plannedArrival, plannedDeparture, arrivalDelay, departureDelay;

            //Make function that will set data from the table.
            Func<HtmlNodeCollection, IDelayInfo<Train>> setData = cells =>
            {
                //Set helper variables with relevant data
                stationName = ClearInnerText(cells[3].InnerText);
                stationID = GetIDFromHtmlNode(cells[3]);

                date = GetDateTimeFromText(cells[1].InnerText, "d.M.yyyy");

                from = GetRelationsFromText(cells[2].InnerText, out destination);

                TryGetPlannedTimeFromText(cells[4].InnerText, out plannedArrival);
                TryGetDelayTimeFromText(cells[5].InnerText, out arrivalDelay);

                TryGetPlannedTimeFromText(cells[4].InnerText, out plannedDeparture);
                TryGetDelayTimeFromText(cells[7].InnerText, out departureDelay);

                //Return delay info by given variables
                return DelayInfo<Train>.GetSingleDelayInfo(train, train, new Station(stationName, stationID), from, destination,
                                          date, plannedArrival, arrivalDelay, plannedDeparture, departureDelay);
            };

            //Get data from the table
            IEnumerable<IDelayInfo<Train>> collection = GetDelayInfosFromTable(table, setData);

            return new DelayInfoCollection<Train, IDelayInfo<Train>>(train, collection);
        }

        #region Getting DelayInfo - helper methods

        /// <summary>
        /// Gets delay info from the given table
        /// </summary>
        /// <param name="table">Html node of the table with data.</param>
        /// <param name="setData">Function that sets downloaded data.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="NodeNotFoundException"/>
        private IEnumerable<IDelayInfo<TBy>> GetDelayInfosFromTable<TBy>(HtmlNode table, Func<HtmlNodeCollection, IDelayInfo<TBy>> setData)
            where TBy : IEquatable<TBy>, IComparable<TBy>
        {
            if (setData == null) throw new ArgumentNullException("setData", "This delegate parameter cannot be null");
            if (table == null) throw new ArgumentNullException("table", "Html node that was supposed to be a table is null");
            return GetDelayInfoFromTableRow(table, setData);
        }

        /// <summary>
        /// Gets delay info from the given multiple tables.
        /// </summary>
        /// <param name="table">Html node collection of the tables with data.</param>
        /// <param name="setData">Function that sets downloaded data.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="NodeNotFoundException"/>
        private IEnumerable<IDelayInfo<TBy>> GetDelayInfosFromMultipleTables<TBy>(HtmlNodeCollection tables, Func<HtmlNodeCollection, IDelayInfo<TBy>> setData)
            where TBy : IEquatable<TBy>, IComparable<TBy>
        {
            if (tables == null) throw new ArgumentNullException("tables", "tables parameter cannot be null");
            if (setData == null) throw new ArgumentNullException("setData", "This delegate parameter cannot be null");

            //Make list where data from table will be saved
            List<IDelayInfo<TBy>> collection = new List<IDelayInfo<TBy>>();

            foreach(HtmlNode table in tables)
            {
                collection.AddRange(GetDelayInfoFromTableRow(table, setData));
            }

            return collection;
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{T}"/> of <see cref="IDelayInfo{TBy}"/> from every table's row
        /// </summary>
        /// <param name="table">Html node of the table with data.</param>
        /// <param name="setData">Function that sets downloaded data.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="NodeNotFoundException"/>
        private IEnumerable<IDelayInfo<TBy>> GetDelayInfoFromTableRow<TBy>(HtmlNode table, Func<HtmlNodeCollection, IDelayInfo<TBy>> setData)
            where TBy : IEquatable<TBy>, IComparable<TBy>
        {
            if (table == null) throw new ArgumentNullException("table", "table parameter cannot be null");
            if (setData == null) throw new ArgumentNullException("setData", "This delegate parameter cannot be null");

            //Get rows from the table
            foreach (HtmlNode row in table.SelectNodes("tr")
                ?? throw new NodeNotFoundException("Html node collection of table rows is null"))
            {
                //Get cells from the table's row
                HtmlNodeCollection cells = row.SelectNodes("td") ?? 
                    throw new NodeNotFoundException("Cells were not found");

                //Set and save data from current row
                var data = setData(cells);

                //Return this data
                yield return data;
            }
        }

        /// <summary>
        /// Gets completed collection of delayed trains by station.
        /// </summary>
        /// <param name="arrivalColl">Collection of arrival information.</param>
        /// <param name="departureColl">Collection of departure information.</param>
        /// <exception cref="ArgumentNullException"/>
        private IEnumerable<IDelayInfo<Station>> CompleteStationDelayInfo(IEnumerable<IDelayInfo<Station>> arrivalColl,
            IEnumerable<IDelayInfo<Station>> departureColl)
        {
            if (arrivalColl == null) throw new ArgumentNullException("arrivalColl", "This collection cannot be null");
            if (departureColl == null) throw new ArgumentNullException("departureColl", "This collection cannot be null");

            //Get list of arrival and departure info
            List<IDelayInfo<Station>> arrivalList = arrivalColl.ToList();
            List<IDelayInfo<Station>> departureList = departureColl.ToList();

            //List which will contain completed data
            List<IDelayInfo<Station>> completed = new List<IDelayInfo<Station>>();

            foreach (IDelayInfo<Station> arr_delayInfo in arrivalList)
            {
                foreach (IDelayInfo<Station> dep_delayInfo in departureList)
                {
                    //If arrival data has the same train as departure data has and
                    //arrival data has the same relation as departure data has
                    if (arr_delayInfo.Train == dep_delayInfo.Train && 
                        arr_delayInfo.Relation == dep_delayInfo.Relation) 
                    {
                        //Set departure data to arrival data
                        arr_delayInfo.PlannedDeparture = dep_delayInfo.PlannedDeparture;
                        arr_delayInfo.DepartureDelay = dep_delayInfo.DepartureDelay;
                        departureList.Remove(dep_delayInfo); // and remove this departure data from departure list.
                        break;
                    }
                }

                //Add completed single arrival data to completed list
                completed.Add(arr_delayInfo);
            }

            //Add rest of the departure list (may contain nothing)
            completed.AddRange(departureList);

            //Return completed list
            return completed;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets table as a html node
        /// </summary>
        /// <param name="node">Node where table is contained</param>
        /// <exception cref="NodeNotFoundException"/>
        private HtmlNode GetTable(HtmlNode node)
        {
            return GetTable(node, 0, "//tbody");
        }

        /// <summary>
        /// Gets table as a html node
        /// </summary>
        /// <param name="node">Node where table is contained</param>
        /// <param name="index">Index of a table if there is more than one table.</param>
        /// <exception cref="NodeNotFoundException"/>
        private HtmlNode GetTable(HtmlNode node, int index)
        {
            return GetTable(node, index, "//tbody");
        }

        /// <summary>
        /// Gets table as a html node
        /// </summary>
        /// <param name="node">Node where table is contained</param>
        /// <param name="index">Index of a table if there is more than one table.</param>
        /// <param name="htmlTag">Html tag of a table.</param>
        /// <exception cref="NodeNotFoundException"/>
        private HtmlNode GetTable(HtmlNode node, int index, string htmlTag)
        {
            return GetMultipleTables(node, htmlTag)[index];
        }

        /// <summary>
        /// Gets collection of tables from the given html node
        /// </summary>
        /// <exception cref="NodeNotFoundException"/>
        private HtmlNodeCollection GetMultipleTables(HtmlNode node)
        {
            return GetMultipleTables(node, "//tbody");
        }

        /// <summary>
        /// Gets collection of tables from the given html node
        /// </summary>
        /// <param name="htmlTag">Html tag of a table</param>
        /// <exception cref="NodeNotFoundException"/>
        private HtmlNodeCollection GetMultipleTables(HtmlNode node, string htmlTag)
        {
            HtmlNodeCollection tables = node.SelectNodes(htmlTag)
                ?? throw new NodeNotFoundException("Tables were not found");

            return tables;
        }

        /// <summary>
        /// Gets ID from the given HTML node.
        /// </summary>
        /// <param name="node">HTML node that contains id in its attribute</param>
        /// <returns></returns>
        /// <exception cref="NodeNotFoundException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public string GetIDFromHtmlNode(HtmlNode node)
        {
            string id = node.SelectSingleNode("span").
                SelectSingleNode("a").GetAttributeValue("href", "Unknown") ?? throw new NodeNotFoundException("Node not found");

            return GetIDFromText(id);
        }

        /// <summary>
        /// Gets ID from the given text.
        /// </summary>
        /// <param name="text">Text that contains a train's ID.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        private string GetIDFromText(string text)
        {
            text = text.Substring(text.LastIndexOf('=') + 1);

            return text;
        }

        /// <summary>
        /// Helper method to get planned time from text.
        /// </summary>
        /// <param name="text">Text that contains planned time.</param>
        public bool TryGetPlannedTimeFromText(string text, out TimeSpan? timeSpan)
        {
            try
            {
                TimeSpan temp = TimeSpan.Parse(ClearInnerText(text));

                timeSpan = temp;
            }
            catch
            {
                timeSpan = null; return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method to get delay time from text.
        /// </summary>
        /// <param name="text">Text that contains delay time.</param>
        private bool TryGetDelayTimeFromText(string text, out TimeSpan? timeSpan)
        {
            try
            {
                //Get the text without unnecessary white spaces.
                string del = ClearInnerText(text);
                //Get substring of the text without the "min" word
                del = del.Substring(0, del.IndexOf("m") - 1);

                int min = int.Parse(del);

                timeSpan = TimeSpan.FromMinutes(min);
            }
            catch { timeSpan = null; return false; }

            return true;
        }

        /// <summary>
        /// Helper method to get the relation from text.
        /// </summary>
        /// <param name="text">Text that contains relation.</param>
        /// <param name="desination">Parameter where destination is returned.</param>
        /// <returns>Returns station the train arrives from.</returns>
        /// <exception cref="ArgumentException"/>
        private string GetRelationsFromText(string text, out string desination)
        {
            //Get two stations from the text
            string[] relations = ClearInnerText(text).
                Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            //Return final station
            desination = relations[1];

            //Return the first station
            return relations[0];
        }

        /// <summary>
        /// Helper method to get the date of the information.
        /// </summary>
        /// <param name="text">Text that contains the date.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        private DateTime GetDateTimeFromText(string text, string format)
        {
            return DateTime.ParseExact(ClearInnerText(text)
                   , format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Helper method to remove all unnecessary white spaces.
        /// </summary>
        /// <param name="text">Text to normalize.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="RegexMatchTimeoutException"/>
        private string ClearInnerText(string text)
        {
            string v = Regex.Replace(text, @"\s+", " ");

            return v.Trim();
        }

        #endregion

        #endregion

    }
}
