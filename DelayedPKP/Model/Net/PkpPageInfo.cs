using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedPKP.Model
{

    using System.Net;
    using System.Threading;

    /// <summary>
    /// Static class that contains information about the page of delayed trains.
    /// </summary>
    public static class PkpPageInfo
    {

        /// <summary>
        /// The main adress of the page.
        /// </summary>
        public static Uri MainAdress
        { get; } = new Uri("https://infopasazer.intercity.pl");

        /// <summary>
        /// The adress of the page where users choose a station.
        /// </summary>
        public static Uri StationsAdress
        { get; } = new Uri(MainAdress, "index.php?p=stations&q=");

        /// <summary>
        /// The adress of the delayed trains.
        /// </summary>
        public static Uri DelayedTrainsAdress
        { get; } = new Uri(MainAdress, "index.php?p=station&id=");

        /// <summary>
        /// The adress of the specific train.
        /// </summary>
        public static Uri TrainAdress
        { get; } = new Uri(MainAdress, "?p=train&id=");

        /// <summary>
        /// Gets html document of the stations to choose.
        /// </summary>
        /// <param name="station">Station to search.</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="WebException"/>
        public static HtmlDocument GetHtmlDocumentOfStations(string station)
        {
            //Check if station is null or empty
            if (string.IsNullOrEmpty(station))
                throw new ArgumentException("Location cannot be null or empty.", "station");

            //Normalize the name of a station due to the page rules.
            station = station.Replace(' ', '+');

            return GetHtmlDocument_TrainPages(station, StationsAdress);
        }

        /// <summary>
        /// Gets html document of delayed trains.
        /// </summary>
        /// <param name="id">ID of a station.</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="WebException"/>
        public static HtmlDocument GetHtmlDocumentOfDelayedTrains(string id)
        {
            //Check if id is null or empty
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("The id cannot be null or empty.", "id");

            return GetHtmlDocument_TrainPages(id, DelayedTrainsAdress);
        }

        /// <summary>
        /// Gets html document of specific delayed train.
        /// </summary>
        /// <param name="id">Train's id.</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="WebException"/>
        public static HtmlDocument GetHtmlDocumentOfTrainPage(string id)
        {
            //Check if id is null or empty
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("The id cannot be null or empty.", "id");

            return GetHtmlDocument_TrainPages(id, TrainAdress);
        }

        /// <summary>
        /// Gets html document by the given link.
        /// </summary>
        /// <param name="link">Link to get html document.</param>
        /// <returns></returns>
        /// <exception cref="WebException"/>
        public static HtmlDocument GetHtmlDocument(Uri link)
        {
            //Create an instance of HtmlWeb to set AutoDetectEncoding as true
            var web = new HtmlWeb()
            {
                AutoDetectEncoding = true
            };

            //Load HTML document from the given URI.
            var doc = web.Load(link);

            //Return this document
            return doc;
        }

        //public async static Task<HtmlDocument> GetHtmlDocument(Uri link)
        //{
        //    //Create an instance of HtmlWeb to set AutoDetectEncoding as true
        //    var web = new HtmlWeb()
        //    {
        //        AutoDetectEncoding = true
        //    };

        //    return await Task.Run(() =>
        //    {
        //        //Load HTML document from the given URI.
        //        var doc = web.LoadFromWebAsync(link.ToString());

        //        //Return this document
        //        return doc;
        //    });
        //}

        #region Helper methods

        /// <summary>
        /// Helper method to get html document by specific adress.
        /// </summary>
        /// <param name="t">Text that will be added to the adress.</param>
        /// <param name="adress">Adress.</param>
        /// <exception cref="WebException"/>
        private static HtmlDocument GetHtmlDocument_TrainPages(string t, Uri adress)
        {
            string _adress = adress + t;

            return GetHtmlDocument(new Uri(_adress));
        }

        #endregion


    }
}
