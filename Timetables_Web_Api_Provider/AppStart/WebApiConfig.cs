using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Timetables.Server.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
			((Newtonsoft.Json.Serialization.DefaultContractResolver)config.Formatters.JsonFormatter.SerializerSettings.ContractResolver).IgnoreSerializableAttribute = true;
		}
	}
}
