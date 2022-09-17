using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using FloatplaneAPIClientCSharp.Client;

namespace FloatplaneAPIClientCSharp.Test
{
	internal class ApiTestHelper
	{
		/// <summary>
		/// We want to use very strict serializer settings when performing regression tests against the
		/// Floatplane API when possible. This lets us know when new properties are added in FP API responses
		/// for existing endpoints (like when tags were introduced), or when breaking changes are made (rare).
		/// </summary>
		public static readonly JsonSerializerSettings StrictSerializerSettings = new JsonSerializerSettings
		{
			// Use DateTimeOffset over DateTime.
			DateParseHandling = DateParseHandling.DateTimeOffset,
			DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
			// FP's specific date time offset format.
			DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.SSSZ",
			// Always write properties to JSON even if they're a default value.
			DefaultValueHandling = DefaultValueHandling.Include,
			// Throw errors if the JSON has extra properties not in the Model.
			MissingMemberHandling = MissingMemberHandling.Error,
		};


		public static void SetStrictSerializerSettings(IApiAccessor api, ISynchronousClient client)
		{
			// Get the ApiClient for modifications below.
			var apiClient = client as ApiClient;
			if (apiClient == null)
			{
				throw new Exception("The client is not an ApiClient");
			}

			// Set the strict serializer for all operations.
			apiClient.SerializerSettings = ApiTestHelper.StrictSerializerSettings;

			// Set the `sails.sid` Cookie for authenticated requests from the environment.
			var sailsSid = System.Environment.GetEnvironmentVariable("sailssid");
			if (string.IsNullOrEmpty(sailsSid))
			{
				throw new Exception("The `sailssid` environment variable is not set.");
			}
			api.Configuration.ApiKey["sails.sid"] = sailsSid;

			// Set the User-Agent header to identify traffic.
			// The CFNetwork at the tail end of the user agent is to mark this traffic as coming
			// from non-browser clients, and bypass some captcha checks.
			api.Configuration.DefaultHeaders["User-Agent"] = "Floatplane API Docs Integration and Regression Tests, CFNetwork";
		}

		public static FloatplaneAPIClientCSharp.Model.ErrorModel GetErrorModel(ApiException ex)
		{
			return JsonConvert.DeserializeObject<FloatplaneAPIClientCSharp.Model.ErrorModel>((string)ex.ErrorContent, ApiTestHelper.StrictSerializerSettings);
		}

		public static void ValidateErrorModel(FloatplaneAPIClientCSharp.Model.ErrorModel errorModel)
		{
			// Note: errorModel.Message may be null for HTTP 500 errors.
			Xunit.Assert.NotEmpty(errorModel.Errors);
			Xunit.Assert.NotEmpty(errorModel.Errors.First().Name);
		}
	}

}
