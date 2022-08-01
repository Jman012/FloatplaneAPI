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
	///  Class for testing CreatorSubscriptionPlanV2Api
	/// </summary>
	/// <remarks>
	/// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
	/// Please update the test case below to test the API endpoint.
	/// </remarks>
	public class CreatorSubscriptionPlanV2ApiTests : IDisposable
	{
		private CreatorSubscriptionPlanV2Api instance;

		public CreatorSubscriptionPlanV2ApiTests()
		{
			instance = new CreatorSubscriptionPlanV2Api();
		}

		public void Dispose()
		{
			// Cleanup when everything is done.
		}

		/// <summary>
		/// Test an instance of CreatorSubscriptionPlanV2Api
		/// </summary>
		[Fact]
		public void InstanceTest()
		{
			// TODO uncomment below to test 'IsType' CreatorSubscriptionPlanV2Api
			//Assert.IsType<CreatorSubscriptionPlanV2Api>(instance);
		}

		/// <summary>
		/// Test GetCreatorSubInfoPublic
		/// </summary>
		[Fact]
		public void GetCreatorSubInfoPublicTest()
		{
			string creatorId = ApiTestSampleData.LttCreatorId;
			var response = instance.GetCreatorSubInfoPublicWithHttpInfo(creatorId);
			Assert.Null(response.ErrorText);
			Assert.IsType<PlanInfoV2Response>(response.Data);
		}
	}
}
