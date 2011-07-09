using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMGasPrice.Service.Models;
using DMGasPrice.Service.Helpers;

namespace DMGasPrice.Service.Controllers
{
    public class PricesController : Controller
    {
        public JsonResult Index()
        {
            List<GasPrice> prices = new List<GasPrice>();
            foreach (int key in GasPriceCache.Instance.Keys)
            {
                prices.Add(GasPriceCache.Instance[key]);
            }
            return Json(GasPriceCache.Instance.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Details(int key)
        {
            if (GasPriceCache.Instance.ContainsKey(key))
            {
                return Json(GasPriceCache.Instance[key], JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailsOld(int key, string cityName)
        {
            if (GasPriceCache.Instance.ContainsKey(key))
            {
                GasPrice price = GasPriceCache.Instance[key];
                return Json(new GasPrice()
                {
                    CityName = cityName,
                    Price = price.Price,
                    PriceChange = price.PriceChange,
                    For = price.For
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}