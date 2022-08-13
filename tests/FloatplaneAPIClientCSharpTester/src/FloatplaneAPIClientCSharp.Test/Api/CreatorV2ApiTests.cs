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
	///  Class for testing CreatorV2Api
	/// </summary>
	/// <remarks>
	/// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
	/// Please update the test case below to test the API endpoint.
	/// </remarks>
	public class CreatorV2ApiTests : IDisposable
	{
		private CreatorV2Api instance;

		public CreatorV2ApiTests()
		{
			instance = new CreatorV2Api();
			ApiTestHelper.SetStrictSerializerSettings(instance, instance.Client);
		}

		public void Dispose()
		{
			// Cleanup when everything is done.
		}

		/// <summary>
		/// Test an instance of CreatorV2Api
		/// </summary>
		[Fact]
		public void InstanceTest()
		{
			// TODO uncomment below to test 'IsType' CreatorV2Api
			//Assert.IsType<CreatorV2Api>(instance);
		}

		/// <summary>
		/// Test GetCreatorInfoByName
		/// </summary>
		[Fact]
		public void GetCreatorInfoByNameTest()
		{
			List<string> creatorURL = new List<string>()
			{
				ApiTestSampleData.LttCreatorName,
			};
			var response = instance.GetCreatorInfoByNameWithHttpInfo(creatorURL);
			Assert.Null(response.ErrorText);
			Assert.IsType<List<CreatorModelV2Extended>>(response.Data);
			Assert.True(response.Data?.Any());
		}

		[Fact]
		public void GetCreatorInfoByNameNonExistentTest()
		{
			// We actually don't get an error. Just an empty list.
			List<string> creatorURL = new List<string>()
			{
				ApiTestSampleData.NonExistentIdentifer,
			};
			var response = instance.GetCreatorInfoByNameWithHttpInfo(creatorURL);
			Assert.Null(response.ErrorText);
			Assert.IsType<List<CreatorModelV2Extended>>(response.Data);
			Assert.Equal(0, response.Data?.Count());
		}

		/// <summary>
		/// Test GetInfo
		/// </summary>
		[Fact]
		public void GetInfoTest()
		{
			List<string> creatorGUID = new List<string>()
			{
				ApiTestSampleData.LttCreatorId,
			};
			var response = instance.GetInfoWithHttpInfo(creatorGUID);
			Assert.Null(response.ErrorText);
			Assert.IsType<List<CreatorModelV2>>(response.Data);
			Assert.True(response.Data?.Any());
		}

		[Fact]
		public void GetInfoNonExistentTest()
		{
			// We actually don't get an error. Just an empty list.
			List<string> creatorGUID = new List<string>()
			{
				ApiTestSampleData.NonExistentIdentifer,
			};
			var response = instance.GetInfoWithHttpInfo(creatorGUID);
			Assert.Null(response.ErrorText);
			Assert.IsType<List<CreatorModelV2>>(response.Data);
			Assert.Equal(0, response.Data?.Count());
		}
	}
}
