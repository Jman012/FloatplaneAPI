/*
 * Floatplane REST API
 *
 * The version of the OpenAPI document: 3.9.9
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using RestSharp;
using Xunit;

using FloatplaneAPIClientCSharp.Client;
using FloatplaneAPIClientCSharp.Api;
using FloatplaneAPIClientCSharp.Model;

namespace FloatplaneAPIClientCSharp.Test.Api
{
	/// <summary>
	///  Class for testing ConnectedAccountsV2Api
	/// </summary>
	/// <remarks>
	/// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
	/// Please update the test case below to test the API endpoint.
	/// </remarks>
	public class ConnectedAccountsV2ApiTests : IDisposable
	{
		private ConnectedAccountsV2Api instance;

		public ConnectedAccountsV2ApiTests()
		{
			instance = new ConnectedAccountsV2Api();
			ApiTestHelper.SetStrictSerializerSettings(instance, instance.Client);
		}

		public void Dispose()
		{
			// Cleanup when everything is done.
		}

		/// <summary>
		/// Test ListConnections
		/// </summary>
		[Fact]
		public void ListConnectionsTest()
		{
			var response = instance.ListConnectionsWithHttpInfo();
			Assert.Null(response.ErrorText);
			Assert.IsType<List<ConnectedAccountModel>>(response.Data);
		}
	}
}
