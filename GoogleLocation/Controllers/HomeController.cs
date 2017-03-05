using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using GoogleLocation.Models;
using MaxMind.GeoIP2;

namespace GoogleLocation.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			IList<OfficeLocation> model = GetOfficesFromSubPagesInEpiServer();

			ViewBag.ClientLocation = GetClientLocationByIp();

			return View(model);
		}

		private OfficeLocation GetClientLocationByIp()
		{
			OfficeLocation retval = new OfficeLocation();

			try
			{
				using (var reader = new DatabaseReader("GeoLite2Db/GeoLite2-City.mmdb"))
				{
					// Replace "City" with the appropriate method for your database, e.g.,
					// "Country".
					string clientIp = Request.UserHostAddress;
					var city = reader.City(clientIp);

					Console.WriteLine(city.Country.IsoCode); // 'US'
					Console.WriteLine(city.Country.Name); // 'United States'

					Console.WriteLine(city.MostSpecificSubdivision.Name); // 'Minnesota'
					Console.WriteLine(city.MostSpecificSubdivision.IsoCode); // 'MN'

					Console.WriteLine(city.City.Name); // 'Minneapolis'
					retval.Name = city.City.Name;
					Console.WriteLine(city.Postal.Code); // '55455'

					Console.WriteLine(city.Location.Latitude); // 44.9733
					Console.WriteLine(city.Location.Longitude); // -93.2323

					retval.LocationX = city.Location.Latitude.HasValue ? city.Location.Latitude.Value : 59.3252315;
					retval.LocationY = city.Location.Longitude.HasValue ? city.Location.Longitude.Value : 18.0599355;
					if (!city.Location.Latitude.HasValue || !city.Location.Longitude.HasValue)
					{
						retval.Name = "Osäker position";
						retval.LocationX = 59.3252315;
						retval.LocationY = 18.0599355;
					}
				}
			}
			catch
			{
				retval.Name = "Osäker position";
				retval.LocationX = 59.3252315;
				retval.LocationY = 18.0599355;
			}

			return retval;
		}

		public ActionResult GeoIpTest()
		{
			
			return View();
		}

		private IList<OfficeLocation> GetOfficesFromSubPagesInEpiServer()
		{
			IList<OfficeLocation> officeLocations = new List<OfficeLocation>();

			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 1, Name = "Sollentuna Centrum", LocationX = 59.4283682, LocationY = 17.9507594 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 2, Name = "Stinsen Häggvik", LocationX = 59.4367469, LocationY = 17.9362318 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 3, Name = "Friends Arena", LocationX = 59.3717495, LocationY = 17.9908945 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 4, Name = "Sergels Torg", LocationX = 59.3504345, LocationY = 18.0419199 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 5, Name = "Metamatrix", LocationX = 59.3159672, LocationY = 18.0347773 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 6, Name = "Scandic Nyköping", LocationX = 58.7515653, LocationY = 17.0024674 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 7, Name = "Sofiakyrkan Jönköping", LocationX = 57.7823193, LocationY = 14.1595065 });
			officeLocations.Add(new OfficeLocation() { OfficeLocationID = 8, Name = "Göteborgs Universitet", LocationX = 57.6946717, LocationY = 11.9701839 });

			return officeLocations;
		}
	}
}
