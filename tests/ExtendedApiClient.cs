using System;
using RestSharp;

namespace FloatplaneAPIClientCSharp.Client
{
	partial class ApiClient
	{
		/// <summary>
		///
		/// </summary>
		public Action<IRestRequest> RequestInterceptor { get; set; }
		/// <summary>
		///
		/// </summary>
		public Action<IRestRequest, IRestResponse> ResponseInterceptor { get; set; }

		partial void InterceptRequest(IRestRequest request)
		{
			RequestInterceptor?.Invoke(request);
		}

		partial void InterceptResponse(IRestRequest request, IRestResponse response)
		{
			ResponseInterceptor?.Invoke(request, response);
		}
	}
}
