using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Net;
using DMGasPrice.Service.Models;

namespace DMGasPrice.Service.Helpers
{
    public class GasPriceCache : Dictionary<int, GasPrice>
    {
        private const string MESSAGE_TEMPLATE = "For: {0}";
        private const string URL = "http://tomorrowsgaspricetoday.com/gas-prices.html";

        #region singleton

        private static GasPriceCache _instance = null;
        public static GasPriceCache Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new GasPriceCache();
                }
                return _instance;
            }
        }

        #endregion

        #region methods

        public void Refresh()
        {
            // first download the price list
            string priceListHtml = new WebClient().DownloadString(URL);

            // then load html into document for processing
            HtmlDocument priceListDocument = new HtmlDocument();
            priceListDocument.LoadHtml(priceListHtml);

            // go thru all the cities in the list and store the gas price
            HtmlNodeCollection prices = priceListDocument.DocumentNode.SelectNodes("//ul[@class='sub_nav']//a");
            foreach (HtmlNode price in prices)
            {
                // parse and store useful attributes
                string url = price.Attributes["href"].Value;
                int key = Utils.ExtractCityKey(url);
                string cityName = Utils.ExtractCityName(price.InnerText);
                GasPrice state = new GasPrice { Key = key, CityName = cityName };

                // download gas price page
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadPriceCompleted);
                client.DownloadStringAsync(new Uri(url), state);
            }
        }

        private void DownloadPriceCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            GasPrice price = e.UserState as GasPrice;

            // make sure service call went thru
            if (!e.Cancelled && null == e.Error && null != price)
            {
                string html = e.Result;

                // process the gas price data
                // load html document
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // parase page and get gas price
                HtmlNodeCollection dataNodes = document.DocumentNode.SelectNodes("//div[@class='predication_gasoline_litre_pro_01']");
                HtmlNodeCollection messageNode = document.DocumentNode.SelectNodes("//div[@class='underText']");
                HtmlNodeCollection predictionNode = document.DocumentNode.SelectNodes("//div[@class='predication_gasoline_litre_arrow']//img");
                if (dataNodes.Count > 1 && messageNode.Count > 0 && predictionNode.Count > 0)
                {
                    price.Price = Utils.ExtractPrice(dataNodes[0].InnerText);
                    price.For = Utils.ExtractDate(messageNode[0].InnerText);

                    double priceChange = Utils.ExtractPrice(dataNodes[1].InnerText);
                    bool isDown = predictionNode[0].GetAttributeValue("src", string.Empty).Contains("down");
                    price.PriceChange = isDown ? -priceChange : priceChange;

                    // add gas price to cache
                    this[price.Key] = price;
                }
            }
        }

        #endregion

    }
}