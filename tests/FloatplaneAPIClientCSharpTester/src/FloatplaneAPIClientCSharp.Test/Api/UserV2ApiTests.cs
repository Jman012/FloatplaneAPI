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
	///  Class for testing UserV2Api
	/// </summary>
	/// <remarks>
	/// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
	/// Please update the test case below to test the API endpoint.
	/// </remarks>
	public class UserV2ApiTests : IDisposable
	{
		private UserV2Api instance;

		public UserV2ApiTests()
		{
			instance = new UserV2Api();
			ApiTestHelper.SetStrictSerializerSettings(instance, instance.Client);
		}

		public void Dispose()
		{
			// Cleanup when everything is done.
		}

		/// <summary>
		/// Test GetSecurity
		/// </summary>
		[Fact]
		public void GetSecurityTest()
		{
			var response = instance.GetSecurityWithHttpInfo();
			Assert.Null(response.ErrorText);
			Assert.IsType<UserSecurityV2Response>(response.Data);
		}

		/// <summary>
		/// Test GetUserInfo
		/// </summary>
		[Fact]
		public void GetUserInfoTest()
		{
			List<string> id = new List<string>()
			{
				ApiTestSampleData.SampleUserJamamp.Id,
				ApiTestSampleData.SampleUserBmlzootown.Id,
			};
			var response = instance.GetUserInfoWithHttpInfo(id);
			Assert.Null(response.ErrorText);
			Assert.IsType<UserInfoV2Response>(response.Data);
			Assert.True(response.Data?.Users.Any());
		}

		[Fact]
		public void GetUserInfoNonExistentTest()
		{
			// We actually don't get an error. Just an empty list.
			List<string> id = new List<string>()
			{
				ApiTestSampleData.NonExistentIdentifer,
			};
			var response = instance.GetUserInfoWithHttpInfo(id);
			Assert.Null(response.ErrorText);
			Assert.IsType<UserInfoV2Response>(response.Data);
			Assert.Equal(0, response.Data?.Users.Count());
		}

		/// <summary>
		/// Test GetUserInfoByName
		/// </summary>
		[Fact]
		public void GetUserInfoByNameTest()
		{
			List<string> username = new List<string>()
			{
				ApiTestSampleData.SampleUserJamamp.Name,
				ApiTestSampleData.SampleUserBmlzootown.Name,
			};
			var response = instance.GetUserInfoByNameWithHttpInfo(username);
			Assert.Null(response.ErrorText);
			Assert.IsType<UserNamedV2Response>(response.Data);
			Assert.True(response.Data?.Users.Any());
		}

		[Fact]
		public void GetUserInfoByNameNonExistentTest()
		{
			// We actually don't get an error. Just an empty list.
			List<string> username = new List<string>()
			{
				ApiTestSampleData.NonExistentIdentifer,
			};
			var response = instance.GetUserInfoByNameWithHttpInfo(username);
			Assert.Null(response.ErrorText);
			Assert.IsType<UserNamedV2Response>(response.Data);
			Assert.Equal(0, response.Data?.Users.Count());
		}

		/// <summary>
		/// Test UserCreatorBanStatus
		/// </summary>
		[Fact]
		public void UserCreatorBanStatusTest()
		{
			string creator = ApiTestSampleData.LttCreatorId;
			var response = instance.UserCreatorBanStatusWithHttpInfo(creator);
			Assert.Null(response.ErrorText);
			Assert.IsType<bool>(response.Data);
		}

		[Fact]
		public void UserCreatorBanStatusNonExistentTest()
		{
			string creator = ApiTestSampleData.NonExistentIdentifer;
			var response = instance.UserCreatorBanStatusWithHttpInfo(creator);
			Assert.Null(response.ErrorText);
			Assert.IsType<bool>(response.Data);
			Assert.Equal(false, response.Data);
		}
	}
}