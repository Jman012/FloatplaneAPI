using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using FloatplaneAPIClientCSharp.Client;

namespace FloatplaneAPIClientCSharp.Test
{
	internal class ApiTestHelper
	{
		/// <summary>
		/// A contract resolver that treats all model properties as
		/// `[JsonObject(ItemRequired=Required.Always)]` so that
		/// an error is thrown if the JSON is missing a property.
		/// </summary>
		public class RequireObjectPropertiesContractResolver : DefaultContractResolver
		{
			protected override JsonObjectContract CreateObjectContract(Type objectType)
			{
				var contract = base.CreateObjectContract(objectType);
				contract.ItemRequired = Required.AllowNull;
				return contract;
			}

			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				var property = base.CreateProperty(member, memberSerialization);
				property.Required = Required.AllowNull;
				return property;
			}
		}

		protected static readonly IContractResolver RequireObjectPropertiesContractResolverInstance = new RequireObjectPropertiesContractResolver();

		/// <summary>
		/// We want to use very strict serializer settings when performing regression tests against the
		/// Floatplane API when possible. This lets us know when new properties are added in FP API responses
		/// for existing endpoints (like when tags were introduced), or when breaking changes are made (rare).
		/// </summary>
		public static readonly JsonSerializerSettings StrictSerializerSettings = new JsonSerializerSettings
		{
			// Throw errors if the JSON is missing a Model's property.
			ContractResolver = ApiTestHelper.RequireObjectPropertiesContractResolverInstance,
			// Use DateTimeOffset over DateTime.
			DateParseHandling = DateParseHandling.DateTimeOffset,
			DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
			// FP's specific date time offset format.
			DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.SSSZ",
			// Always write properties to JSON even if they're a default value.
			DefaultValueHandling = DefaultValueHandling.Include,
			// Throw erros if the JSON has extra properties not in the Model.
			MissingMemberHandling = MissingMemberHandling.Error,
		};

		/// <summary>
		/// The `ErrorModel` in the OpenAPI spec is unfortunately not well suited to strict serialization like
		/// the above serializer. This is because there are a lot of variations of the response that are not
		/// dependent upon the endpoint nor even the HTTP Status Code, but rather the actual error message/type.
		/// So, we want to use less strict settings for ErrorModel, because having a singular model for errors
		/// is more important than having strict JSON Schema specifications.
		/// </summary>
		/// <remarks>
		/// The main difference with the strict settings is that it won't complain when properties are missing
		/// from the JSON that are in the model. Thus, let's just re-use the strict settings but change the contract resolve.
		/// </remarks>
		public static JsonSerializerSettings ErrorModelSerializerSettings
		{
			get
			{
				return _ErrorModelSerializerSettings.Value;
			}
		}

		private static readonly Lazy<JsonSerializerSettings> _ErrorModelSerializerSettings = new Lazy<JsonSerializerSettings>(() => {
			var errorModelSettings = ApiTestHelper.StrictSerializerSettings;
			errorModelSettings.ContractResolver = new JsonSerializerSettings().ContractResolver;
			return errorModelSettings;
		});


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
			return JsonConvert.DeserializeObject<FloatplaneAPIClientCSharp.Model.ErrorModel>((string)ex.ErrorContent, ApiTestHelper.ErrorModelSerializerSettings);
		}

		public static void ValidateErrorModel(FloatplaneAPIClientCSharp.Model.ErrorModel errorModel)
		{
			Xunit.Assert.NotEmpty(errorModel.Message);
			Xunit.Assert.NotEmpty(errorModel.Errors);
		}
	}

}
