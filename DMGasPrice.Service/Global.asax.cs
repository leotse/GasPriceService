using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DMGasPrice.Service.Helpers;
using System.ComponentModel;
using System.Threading;

namespace DMGasPrice.Service
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// map old paths to the new API
			RegisterBackwardCompatiblePaths(routes);

			routes.MapRoute(
				"ListGasPrices",
				"",
				new { controller = "Prices", action = "Index"}
			);

			routes.MapRoute(
				"GetGasPrice",
				"{key}",
				new { controller = "Prices", action = "Details", key = "" }
			);

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			// populate data initially
			GasPriceCache.Instance.Refresh();

			// create a background refresh worker
			TimerProcess process = new TimerProcess();
			Thread thread = new Thread(new ThreadStart(process.Start));
			thread.Start();
		}

		#region helpers

        private static void RegisterBackwardCompatiblePaths(RouteCollection routes)
        {
            /*
                private static final String TORONTO_LONDON_URL = "http://www.leotse.com/GasPrice/Toronto-GTA-London";
                private static final String OTTAWA_URL = "http://www.leotse.com/GasPrice/Ottawa";
                private static final String MONTREAL_URL = "http://www.leotse.com/GasPrice/Montreal";
                private static final String WINNIPEG_URL = "http://www.leotse.com/GasPrice/Winnipeg";
                private static final String KITCHENER_WATERLOO_URL = "http://www.leotse.com/GasPrice/Kitchener-Waterloo";
                private static final String VANCOUVER_URL = "http://www.leotse.com/GasPrice/Vancouver";
                private static final String CALGARY_URL = "http://www.leotse.com/GasPrice/Calgary";
             */

            routes.MapRoute(
                "GasPriceTorontoGTALondon",
                "Toronto-GTA-London",
                new { controller = "Prices", action = "DetailsOld", key = 133, cityName = "Toronto-GTA-London" }
            );

            routes.MapRoute(
                "GasPriceOttawa",
                "Ottawa",
                new { controller = "Prices", action = "DetailsOld", key = 86, cityName = "Ottawa" }
            );

            routes.MapRoute(
                "GasPriceMontreal",
                "Montreal",
                new { controller = "Prices", action = "DetailsOld", key = 244, cityName = "Montreal" }
            );

            routes.MapRoute(
                "GasPriceWinnipeg",
                "Winnipeg",
                new { controller = "Prices", action = "DetailsOld", key = 117, cityName = "Winnipeg" }
            );

            routes.MapRoute(
                "GasPriceKitchenerWaterloo",
                "Kitchener-Waterloo",
                new { controller = "Prices", action = "DetailsOld", key = 107, cityName = "Kitchener-Waterloo" }
            );

            routes.MapRoute(
                "GasPriceVancouver",
                "Vancouver",
                new { controller = "Prices", action = "Details", key = 123, cityName = "Vancouver" }
            );

            routes.MapRoute(
                "GasPriceCalgary",
                "Calgary",
                new { controller = "Prices", action = "Details", key = 119, cityName = "Calgary" }
            );
        }

		#endregion
	}
}