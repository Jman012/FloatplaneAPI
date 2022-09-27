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
// uncomment below to import models
//using FloatplaneAPIClientCSharp.Model;

namespace FloatplaneAPIClientCSharp.Test.Api
{
	/// <summary>
	///  Class for testing RedirectV3Api
	/// </summary>
	/// <remarks>
	/// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
	/// Please update the test case below to test the API endpoint.
	/// </remarks>
	public class RedirectV3ApiTests : IDisposable
	{
		private RedirectV3Api instance;

		public RedirectV3ApiTests()
		{
			instance = new RedirectV3Api();
			ApiTestHelper.SetStrictSerializerSettings(instance, instance.Client);
		}

		public void Dispose()
		{
			// Cleanup when everything is done.
		}

		/// <summary>
		/// Test RedirectYTLatest
		/// </summary>
		[Fact]
		public void RedirectYTLatestTest()
		{
			// TODO uncomment below to test the method and replace null with proper value
			//string channelKey = null;
			//var response = instance.RedirectYTLatest(channelKey);
			//Assert.IsType<ErrorModel>(response);
		}
	}
}