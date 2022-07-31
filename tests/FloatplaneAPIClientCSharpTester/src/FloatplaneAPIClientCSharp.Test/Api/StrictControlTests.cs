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
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

using FloatplaneAPIClientCSharp.Client;
using FloatplaneAPIClientCSharp.Api;
using FloatplaneAPIClientCSharp.Model;

namespace FloatplaneAPIClientCSharp.Test.Api
{
	/// <summary>
	/// Class for testing that the strict serialization settings are performing as expected
	/// by forcing failures and expecting exceptions. If this passes, then we can be sure that
	/// other integration tests are performing strictly and that we're not missing anything.
	/// </summary>
	public class StrictControlTests : IDisposable
	{
		private UserV3ApiModified instance;

		public StrictControlTests()
		{
			instance = new UserV3ApiModified();
			ApiTestHelper.SetStrictSerializerSettings(instance, instance.Client);
		}

		public void Dispose()
		{
			// Cleanup when everything is done.
		}

		/// <summary>
		/// Test that extraneous properties in the JSON that aren't in the Model
		/// properly throw an error.
		/// </summary>
		[Fact]
		public void ExtraneousJsonPropertyTest()
		{
			// This should test that JsonSerializerSetting.MissingMemberHandling = .Error is working.
			var response = instance.GetSelfWithHttpInfoModified<UserSelfV3ResponseWithMissingIdProperty>();
			Assert.NotNull(response.ErrorText);
			Assert.NotEmpty(response.ErrorText);
			Assert.Null(response.Data);
		}

		/// <summary>
		/// Test that extraneous properties in the Model that aren't in the JSON
		/// properly throw an error.
		/// </summary>
		[Fact]
		public void ExtraneousModelPropertyTest()
		{
			var response = instance.GetSelfWithHttpInfoModified<UserSelfV3ResponseWithExtraModelProperty>();
			Assert.NotNull(response.ErrorText);
			Assert.NotEmpty(response.ErrorText);
			Assert.Null(response.Data);
		}

		internal class UserSelfV3ResponseWithExtraModelProperty : UserSelfV3Response
		{
			[DataMember(Name = "extraneousModelProperty", EmitDefaultValue = true)]
			public string ExtraneousModelProperty { get; set; }
		}

		// internal class UserSelfV3ResponseWithExtraModelProperty
		// {
		// 	/// <summary>
		// 	/// Initializes a new instance of the <see cref="UserSelfV3ResponseWithExtraModelProperty" /> class.
		// 	/// </summary>
		// 	[JsonConstructorAttribute]
		// 	protected UserSelfV3ResponseWithExtraModelProperty() { }

		// 	/// <summary>
		// 	/// Extraneous model property that isn't in JSON
		// 	/// </summary>
		// 	[DataMember(Name = "extraneousModelProperty", IsRequired = true, EmitDefaultValue = true)]
		// 	public string ExtraneousModelProperty { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets Id
		// 	/// </summary>

		// 	[DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
		// 	public string Id { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets Username
		// 	/// </summary>
		// 	[DataMember(Name = "username", IsRequired = true, EmitDefaultValue = true)]
		// 	public string Username { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets ProfileImage
		// 	/// </summary>
		// 	[DataMember(Name = "profileImage", IsRequired = true, EmitDefaultValue = true)]
		// 	public ImageModel ProfileImage { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets Email
		// 	/// </summary>
		// 	[DataMember(Name = "email", EmitDefaultValue = true)]
		// 	public string Email { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets DisplayName
		// 	/// </summary>
		// 	[DataMember(Name = "displayName", EmitDefaultValue = true)]
		// 	public string DisplayName { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets Creators
		// 	/// </summary>
		// 	[DataMember(Name = "creators", EmitDefaultValue = true)]
		// 	public List<Object> Creators { get; set; }

		// 	/// <summary>
		// 	/// Gets or Sets ScheduledDeletionDate
		// 	/// </summary>
		// 	[DataMember(Name = "scheduledDeletionDate", EmitDefaultValue = true)]
		// 	public string ScheduledDeletionDate { get; set; }
		// }

		internal class UserSelfV3ResponseWithMissingIdProperty
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="UserSelfV3ResponseWithMissingIdProperty" /> class.
			/// </summary>
			[JsonConstructorAttribute]
			protected UserSelfV3ResponseWithMissingIdProperty() { }

			/*
			/// <summary>
			/// Gets or Sets Id
			/// </summary>

			[DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
			public string Id { get; set; }
			*/

			/// <summary>
			/// Gets or Sets Username
			/// </summary>
			[DataMember(Name = "username", IsRequired = true, EmitDefaultValue = true)]
			public string Username { get; set; }

			/// <summary>
			/// Gets or Sets ProfileImage
			/// </summary>
			[DataMember(Name = "profileImage", IsRequired = true, EmitDefaultValue = true)]
			public ImageModel ProfileImage { get; set; }

			/// <summary>
			/// Gets or Sets Email
			/// </summary>
			[DataMember(Name = "email", EmitDefaultValue = true)]
			public string Email { get; set; }

			/// <summary>
			/// Gets or Sets DisplayName
			/// </summary>
			[DataMember(Name = "displayName", EmitDefaultValue = true)]
			public string DisplayName { get; set; }

			/// <summary>
			/// Gets or Sets Creators
			/// </summary>
			[DataMember(Name = "creators", EmitDefaultValue = true)]
			public List<Object> Creators { get; set; }

			/// <summary>
			/// Gets or Sets ScheduledDeletionDate
			/// </summary>
			[DataMember(Name = "scheduledDeletionDate", EmitDefaultValue = true)]
			public string ScheduledDeletionDate { get; set; }
		}

		internal class UserV3ApiModified : UserV3Api
		{
			/// <summary>
			/// Copied from auto-generated UserV3Api. Modified to allow for generic return models.
			/// Get Self Retrieve more detailed information about the user, including their name and email.
			/// </summary>
			/// <exception cref="FloatplaneAPIClientCSharp.Client.ApiException">Thrown when fails to make API call</exception>
			/// <param name="operationIndex">Index associated with the operation.</param>
			/// <returns>ApiResponse of T</returns>
			public FloatplaneAPIClientCSharp.Client.ApiResponse<T> GetSelfWithHttpInfoModified<T>(int operationIndex = 0)
			{
				FloatplaneAPIClientCSharp.Client.RequestOptions localVarRequestOptions = new FloatplaneAPIClientCSharp.Client.RequestOptions();

				string[] _contentTypes = new string[] { };

				// to determine the Accept header
				string[] _accepts = new string[] {
					"application/json"
				};

				var localVarContentType = FloatplaneAPIClientCSharp.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
				if (localVarContentType != null)
				{
					localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
				}

				var localVarAccept = FloatplaneAPIClientCSharp.Client.ClientUtils.SelectHeaderAccept(_accepts);
				if (localVarAccept != null)
				{
					localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
				}


				localVarRequestOptions.Operation = "UserV3Api.GetSelf";
				localVarRequestOptions.OperationIndex = operationIndex;

				// authentication (CookieAuth) required
				// cookie parameter support
				if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("sails.sid")))
				{
					localVarRequestOptions.Cookies.Add(new Cookie("sails.sid", this.Configuration.GetApiKeyWithPrefix("sails.sid")));
				}

				// make the HTTP request
				var localVarResponse = this.Client.Get<T>("/api/v3/user/self", localVarRequestOptions, this.Configuration);
				if (this.ExceptionFactory != null)
				{
					Exception _exception = this.ExceptionFactory("GetSelf", localVarResponse);
					if (_exception != null)
					{
						throw _exception;
					}
				}

				return localVarResponse;
			}
		}
	}
}
