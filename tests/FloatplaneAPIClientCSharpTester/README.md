# FloatplaneAPIClientCSharp - the C# library for the Floatplane REST API

Homepage: [https://jman012.github.io/FloatplaneAPIDocs](https://jman012.github.io/FloatplaneAPIDocs)

This document describes the REST API layer of [https://www.floatplane.com](https://www.floatplane.com), a content creation and video streaming website created by Floatplane Media Inc. and Linus Media Group, where users can support their favorite creates via paid subscriptions in order to watch their video and livestream content in higher quality and other perks.

While this document contains stubs for all of the Floatplane APIs for this version, many are not filled out because they are related only to content creation, moderation, or administration and are not needed for regular use. These have \"TODO\" as the description, and are automatically removed before document generation. If you are viewing the \"Trimmed\" version of this document, they have been removed for brevity.

## API Object Organization

- **Users** and **Creators** exist on Floatplane at the highest level
 - The highest-level object in Floatplane is the Creator. This is an entity, such as Linus Tech Tips, that produces media for Users.
- A Creator owns one or more **Subscription Plans**
- A User can view a Creator's Content if they are subscribed to them
- A Creator publishes **Content**, in the form of **Blog Posts**
 - Content is produced by Creators, and show up for subscribed Users to view when it is released. A piece of Content is meant to be generic, and may contain different types of sub-Content. Currently, the only type is a Blog Post.
 - A Blog Post is the main type of Content that a Creator produces. Blog Posts are how a Creator can share text and/or media attachments with their subscribers.
- A Blog Post is comprised of one or more of: video, audio, picture, or gallery **Attachments**
 - A media Attachment may be: video, audio, picture, gallery. Attachments are a part of Blog Posts, and are in a particular order.
- A Creator may also have a single **Livestream**

## API Flow

As of Floatplane version 3.5.1, these are the recommended endpoints to use for normal operations.

1. Login
 1. `/api/v3/auth/captcha/info` - Get captcha information
 1. `/api/v2/auth/login` - Login with username, password, and optional captcha token
 1. `/api/v2/auth/checkFor2faLogin` - Optionally provide 2FA token to complete login
 1. `/api/v2/auth/logout` - Logout at a later point in time
1. Home page
 1. `/api/v3/user/subscriptions` - Get the user's active subscriptions
 1. `/api/v3/content/creator/list` - Using the subscriptions, show a home page with content from all subscriptions
  1. Supply all creator identifiers from the subscriptions
  1. This should be paginated
 1. `/api/v2/creator/info` - Also show a list of creators that the user can select
  1. Note that this can search and return multiple creators. The V3 version only works for a single creator at a time.
1. Creator page
 1. `/api/v3/creator/info` - Get more details for the creator to display, including if livestreams are available
 1. `/api/v3/content/creator` - Show recent content by the creator
 1. `/api/v2/plan/info` - Show available plans the user can subscribe to for the creator
1. Content page
 1. `/api/v3/content/post` - Show more detailed information about a piece of content, including text description, available attachments, metadata, interactions, etc.
 1. `/api/v3/content/related` - List some related content for the user to watch next
 1. `/api/v3/comment` - Load comments for the content for the user to read
  1. There are several more comment APIs to post, like, dislike, etc.
 1. `/api/v2/user/ban/status` - Determine if the user is banned from this creator
 1. `/api/v3/content/{video|audio|picture|gallery}` - Load the attached media for the post. This is usually video, but audio, pictures, and galleries are also available.
 1. `/api/v2/cdn/delivery` - For video and audio, this is required to get the information to stream or download the content in media players
1. Livestream
 1. `/api/v2/cdn/delivery` - Using the type \"livestream\" to load the livestream media in a media player
 1. `wss://chat.floatplane.com/sails.io/?...` - To connect to the livestream chat over websocket. TODO: Map out the WebSocket API.
1. User Profile
 1. `/api/v3/user/self` - Display username, name, email, and profile pictures

## API Organization

The organization of APIs into categories in this document are reflected from the internal organization of the Floatplane website bundled code, from `frontend.floatplane.com/{version}/vendor.js`. This is in order to use the best organization from the original developers' point of view.

For instance, Floatplane's authentication endpoints are organized into `Auth.v2.login(...)`, `Auth.v2.logout()`, and `Auth.v3.getCaptchaInfo()`. A limitation in OpenAPI is the lack of nested tagging/structure, so this document splits `Auth` into `AuthV2` and `AuthV3` to emulate the nested structure.

## Notes

Note that the Floatplane API does support the use of [ETags](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag) for retrieving some information, such as retrieving information about creators, users, etc. Expect an HTTP 304 if the content has not changed, and to re-use cached responses. This is useful to ease the strain on Floatplane's API server.

The date-time format used by Floatplane API is not standard ISO 8601 format. The dates/times given by Floatplane include milliseconds. Depending on your code generator, you may need to override the date-time format to something similar to `yyyy-MM-dd'T'HH:mm:ss.SSSZ`, for both encoding and decoding.

This C# SDK is automatically generated by the [OpenAPI Generator](https://openapi-generator.tech) project:

- API version: 3.9.9
- SDK version: 1.0.0
- Build package: org.openapitools.codegen.languages.CSharpNetCoreClientCodegen
    For more information, please visit [https://github.com/Jman012/FloatplaneAPI/](https://github.com/Jman012/FloatplaneAPI/)

<a name="frameworks-supported"></a>
## Frameworks supported

<a name="dependencies"></a>
## Dependencies

- [RestSharp](https://www.nuget.org/packages/RestSharp) - 106.13.0 or later
- [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) - 13.0.1 or later
- [JsonSubTypes](https://www.nuget.org/packages/JsonSubTypes/) - 1.8.0 or later
- [System.ComponentModel.Annotations](https://www.nuget.org/packages/System.ComponentModel.Annotations) - 5.0.0 or later

The DLLs included in the package may not be the latest version. We recommend using [NuGet](https://docs.nuget.org/consume/installing-nuget) to obtain the latest version of the packages:
```
Install-Package RestSharp
Install-Package Newtonsoft.Json
Install-Package JsonSubTypes
Install-Package System.ComponentModel.Annotations
```

NOTE: RestSharp versions greater than 105.1.0 have a bug which causes file uploads to fail. See [RestSharp#742](https://github.com/restsharp/RestSharp/issues/742).
NOTE: RestSharp for .Net Core creates a new socket for each api call, which can lead to a socket exhaustion problem. See [RestSharp#1406](https://github.com/restsharp/RestSharp/issues/1406).

<a name="installation"></a>
## Installation
Run the following command to generate the DLL
- [Mac/Linux] `/bin/sh build.sh`
- [Windows] `build.bat`

Then include the DLL (under the `bin` folder) in the C# project, and use the namespaces:
```csharp
using FloatplaneAPIClientCSharp.Api;
using FloatplaneAPIClientCSharp.Client;
using FloatplaneAPIClientCSharp.Model;
```
<a name="packaging"></a>
## Packaging

A `.nuspec` is included with the project. You can follow the Nuget quickstart to [create](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package#create-the-package) and [publish](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package#publish-the-package) packages.

This `.nuspec` uses placeholders from the `.csproj`, so build the `.csproj` directly:

```
nuget pack -Build -OutputDirectory out FloatplaneAPIClientCSharp.csproj
```

Then, publish to a [local feed](https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds) or [other host](https://docs.microsoft.com/en-us/nuget/hosting-packages/overview) and consume the new package via Nuget as usual.

<a name="usage"></a>
## Usage

To use the API client with a HTTP proxy, setup a `System.Net.WebProxy`
```csharp
Configuration c = new Configuration();
System.Net.WebProxy webProxy = new System.Net.WebProxy("http://myProxyUrl:80/");
webProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
c.Proxy = webProxy;
```

<a name="getting-started"></a>
## Getting Started

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using FloatplaneAPIClientCSharp.Api;
using FloatplaneAPIClientCSharp.Client;
using FloatplaneAPIClientCSharp.Model;

namespace Example
{
    public class Example
    {
        public static void Main()
        {

            Configuration config = new Configuration();
            config.BasePath = "https://www.floatplane.com";
            // Configure API key authorization: CookieAuth
            config.ApiKey.Add("sails.sid", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.ApiKeyPrefix.Add("sails.sid", "Bearer");

            var apiInstance = new AuthV2Api(config);
            var checkFor2faLoginRequest = new CheckFor2faLoginRequest(); // CheckFor2faLoginRequest | 

            try
            {
                // Check For 2FA Login
                AuthLoginV2Response result = apiInstance.CheckFor2faLogin(checkFor2faLoginRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling AuthV2Api.CheckFor2faLogin: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }

        }
    }
}
```

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://www.floatplane.com*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AuthV2Api* | [**CheckFor2faLogin**](docs/AuthV2Api.md#checkfor2falogin) | **POST** /api/v2/auth/checkFor2faLogin | Check For 2FA Login
*AuthV2Api* | [**Login**](docs/AuthV2Api.md#login) | **POST** /api/v2/auth/login | Login
*AuthV2Api* | [**Logout**](docs/AuthV2Api.md#logout) | **POST** /api/v2/auth/logout | Logout
*AuthV3Api* | [**GetCaptchaInfo**](docs/AuthV3Api.md#getcaptchainfo) | **GET** /api/v3/auth/captcha/info | Get Captcha Info
*CDNV2Api* | [**GetDeliveryInfo**](docs/CDNV2Api.md#getdeliveryinfo) | **GET** /api/v2/cdn/delivery | Get Delivery Info
*CommentV3Api* | [**DislikeComment**](docs/CommentV3Api.md#dislikecomment) | **POST** /api/v3/comment/dislike | Dislike Comment
*CommentV3Api* | [**GetCommentReplies**](docs/CommentV3Api.md#getcommentreplies) | **GET** /api/v3/comment/replies | Get Comment Replies
*CommentV3Api* | [**GetComments**](docs/CommentV3Api.md#getcomments) | **GET** /api/v3/comment | Get Comments
*CommentV3Api* | [**LikeComment**](docs/CommentV3Api.md#likecomment) | **POST** /api/v3/comment/like | Like Comment
*CommentV3Api* | [**PostComment**](docs/CommentV3Api.md#postcomment) | **POST** /api/v3/comment | Post Comment
*ConnectedAccountsV2Api* | [**ListConnections**](docs/ConnectedAccountsV2Api.md#listconnections) | **GET** /api/v2/connect/list | List Connections
*ContentV3Api* | [**DislikeContent**](docs/ContentV3Api.md#dislikecontent) | **POST** /api/v3/content/dislike | Dislike Content
*ContentV3Api* | [**GetBlogPost**](docs/ContentV3Api.md#getblogpost) | **GET** /api/v3/content/post | Get Blog Post
*ContentV3Api* | [**GetContentTags**](docs/ContentV3Api.md#getcontenttags) | **GET** /api/v3/content/tags | Get Content Tags
*ContentV3Api* | [**GetCreatorBlogPosts**](docs/ContentV3Api.md#getcreatorblogposts) | **GET** /api/v3/content/creator | Get Creator Blog Posts
*ContentV3Api* | [**GetMultiCreatorBlogPosts**](docs/ContentV3Api.md#getmulticreatorblogposts) | **GET** /api/v3/content/creator/list | Get Multi Creator Blog Posts
*ContentV3Api* | [**GetPictureContent**](docs/ContentV3Api.md#getpicturecontent) | **GET** /api/v3/content/picture | Get Picture Content
*ContentV3Api* | [**GetRelatedBlogPosts**](docs/ContentV3Api.md#getrelatedblogposts) | **GET** /api/v3/content/related | Get Related Blog Posts
*ContentV3Api* | [**GetVideoContent**](docs/ContentV3Api.md#getvideocontent) | **GET** /api/v3/content/video | Get Video Content
*ContentV3Api* | [**LikeContent**](docs/ContentV3Api.md#likecontent) | **POST** /api/v3/content/like | Like Content
*CreatorSubscriptionPlanV2Api* | [**GetCreatorSubInfoPublic**](docs/CreatorSubscriptionPlanV2Api.md#getcreatorsubinfopublic) | **GET** /api/v2/plan/info | Get Creator Sub Info Public
*CreatorV2Api* | [**GetCreatorInfoByName**](docs/CreatorV2Api.md#getcreatorinfobyname) | **GET** /api/v2/creator/named | Get Info By Name
*CreatorV2Api* | [**GetInfo**](docs/CreatorV2Api.md#getinfo) | **GET** /api/v2/creator/info | Get Info
*CreatorV3Api* | [**GetCreator**](docs/CreatorV3Api.md#getcreator) | **GET** /api/v3/creator/info | Get Creator
*CreatorV3Api* | [**GetCreators**](docs/CreatorV3Api.md#getcreators) | **GET** /api/v3/creator/list | Get Creators
*EdgesV2Api* | [**GetEdges**](docs/EdgesV2Api.md#getedges) | **GET** /api/v2/edges | Get Edges
*FAQV2Api* | [**GetFaqSections**](docs/FAQV2Api.md#getfaqsections) | **GET** /api/v2/faq/list | Get Faq Sections
*LoyaltyRewardsV3Api* | [**ListCreatorLoyaltyReward**](docs/LoyaltyRewardsV3Api.md#listcreatorloyaltyreward) | **POST** /api/v3/user/loyaltyreward/list | List Creator Loyalty Reward
*PaymentsV2Api* | [**ListAddresses**](docs/PaymentsV2Api.md#listaddresses) | **GET** /api/v2/payment/address/list | List Addresses
*PaymentsV2Api* | [**ListInvoices**](docs/PaymentsV2Api.md#listinvoices) | **GET** /api/v2/payment/invoice/list | List Invoices
*PaymentsV2Api* | [**ListPaymentMethods**](docs/PaymentsV2Api.md#listpaymentmethods) | **GET** /api/v2/payment/method/list | List Payment Methods
*PollV3Api* | [**JoinLiveRoom**](docs/PollV3Api.md#joinliveroom) | **POST** /api/v3/poll/live/joinroom | Poll Join Live Room
*PollV3Api* | [**LeaveLiveRoom**](docs/PollV3Api.md#leaveliveroom) | **POST** /api/v3/poll/live/leaveLiveRoom | Poll Leave Live Room
*PollV3Api* | [**VotePoll**](docs/PollV3Api.md#votepoll) | **POST** /api/v3/poll/votePoll | Vote Poll
*RedirectV3Api* | [**RedirectYTLatest**](docs/RedirectV3Api.md#redirectytlatest) | **POST** /api/v3/redirect-yt-latest/{channelKey} | Redirect to YouTube Latest Video
*SocketV3Api* | [**DisconnectSocket**](docs/SocketV3Api.md#disconnectsocket) | **POST** /api/v3/socket/disconnect | Disconnect
*SocketV3Api* | [**SocketConnect**](docs/SocketV3Api.md#socketconnect) | **POST** /api/v3/socket/connect | Connect
*SubscriptionsV3Api* | [**ListUserSubscriptionsV3**](docs/SubscriptionsV3Api.md#listusersubscriptionsv3) | **GET** /api/v3/user/subscriptions | List User Subscriptions
*UserV2Api* | [**GetSecurity**](docs/UserV2Api.md#getsecurity) | **GET** /api/v2/user/security | Get Security
*UserV2Api* | [**GetUserInfo**](docs/UserV2Api.md#getuserinfo) | **GET** /api/v2/user/info | Info
*UserV2Api* | [**GetUserInfoByName**](docs/UserV2Api.md#getuserinfobyname) | **GET** /api/v2/user/named | Get Info By Name
*UserV2Api* | [**UserCreatorBanStatus**](docs/UserV2Api.md#usercreatorbanstatus) | **GET** /api/v2/user/ban/status | User Creator Ban Status
*UserV3Api* | [**GetActivityFeedV3**](docs/UserV3Api.md#getactivityfeedv3) | **GET** /api/v3/user/activity | Get Activity Feed
*UserV3Api* | [**GetExternalLinksV3**](docs/UserV3Api.md#getexternallinksv3) | **GET** /api/v3/user/links | Get External Links
*UserV3Api* | [**GetSelf**](docs/UserV3Api.md#getself) | **GET** /api/v3/user/self | Get Self
*UserV3Api* | [**GetUserNotificationSettingsV3**](docs/UserV3Api.md#getusernotificationsettingsv3) | **GET** /api/v3/user/notification/list | Get User Notification Settings
*UserV3Api* | [**UpdateUserNotificationSettingsV3**](docs/UserV3Api.md#updateusernotificationsettingsv3) | **POST** /api/v3/user/notification/update | Update User Notification Settings


<a name="documentation-for-models"></a>
## Documentation for Models

 - [Model.AudioAttachmentModel](docs/AudioAttachmentModel.md)
 - [Model.AudioAttachmentModelWaveform](docs/AudioAttachmentModelWaveform.md)
 - [Model.AuthLoginV2Request](docs/AuthLoginV2Request.md)
 - [Model.AuthLoginV2Response](docs/AuthLoginV2Response.md)
 - [Model.BlogPostModelV3](docs/BlogPostModelV3.md)
 - [Model.BlogPostModelV3Creator](docs/BlogPostModelV3Creator.md)
 - [Model.BlogPostModelV3CreatorCategory](docs/BlogPostModelV3CreatorCategory.md)
 - [Model.BlogPostModelV3CreatorOwner](docs/BlogPostModelV3CreatorOwner.md)
 - [Model.CdnDeliveryV2Response](docs/CdnDeliveryV2Response.md)
 - [Model.CdnDeliveryV2ResponseResource](docs/CdnDeliveryV2ResponseResource.md)
 - [Model.CdnDeliveryV2ResponseResourceData](docs/CdnDeliveryV2ResponseResourceData.md)
 - [Model.CdnDeliveryV2ResponseResourceDataQualityLevelParamsValue](docs/CdnDeliveryV2ResponseResourceDataQualityLevelParamsValue.md)
 - [Model.CdnDeliveryV2ResponseResourceDataQualityLevelsInner](docs/CdnDeliveryV2ResponseResourceDataQualityLevelsInner.md)
 - [Model.CheckFor2faLoginRequest](docs/CheckFor2faLoginRequest.md)
 - [Model.ChildImageModel](docs/ChildImageModel.md)
 - [Model.CommentLikeV3PostRequest](docs/CommentLikeV3PostRequest.md)
 - [Model.CommentModel](docs/CommentModel.md)
 - [Model.CommentModelInteractionCounts](docs/CommentModelInteractionCounts.md)
 - [Model.CommentReplyModel](docs/CommentReplyModel.md)
 - [Model.CommentV3PostRequest](docs/CommentV3PostRequest.md)
 - [Model.CommentV3PostResponse](docs/CommentV3PostResponse.md)
 - [Model.CommentV3PostResponseInteractionCounts](docs/CommentV3PostResponseInteractionCounts.md)
 - [Model.ConnectedAccountModel](docs/ConnectedAccountModel.md)
 - [Model.ConnectedAccountModelConnectedAccount](docs/ConnectedAccountModelConnectedAccount.md)
 - [Model.ConnectedAccountModelConnectedAccountData](docs/ConnectedAccountModelConnectedAccountData.md)
 - [Model.ContentCreatorListLastItems](docs/ContentCreatorListLastItems.md)
 - [Model.ContentCreatorListV3Response](docs/ContentCreatorListV3Response.md)
 - [Model.ContentLikeV3Request](docs/ContentLikeV3Request.md)
 - [Model.ContentPictureV3Response](docs/ContentPictureV3Response.md)
 - [Model.ContentPostV3Response](docs/ContentPostV3Response.md)
 - [Model.ContentVideoV3Response](docs/ContentVideoV3Response.md)
 - [Model.ContentVideoV3ResponseLevelsInner](docs/ContentVideoV3ResponseLevelsInner.md)
 - [Model.CreatorModelV2](docs/CreatorModelV2.md)
 - [Model.CreatorModelV3](docs/CreatorModelV3.md)
 - [Model.CreatorModelV3Category](docs/CreatorModelV3Category.md)
 - [Model.DiscordRoleModel](docs/DiscordRoleModel.md)
 - [Model.DiscordServerModel](docs/DiscordServerModel.md)
 - [Model.EdgeModel](docs/EdgeModel.md)
 - [Model.EdgeModelDatacenter](docs/EdgeModelDatacenter.md)
 - [Model.EdgesModel](docs/EdgesModel.md)
 - [Model.EdgesModelClient](docs/EdgesModelClient.md)
 - [Model.ErrorModel](docs/ErrorModel.md)
 - [Model.ErrorModelErrorsInner](docs/ErrorModelErrorsInner.md)
 - [Model.FaqSectionModel](docs/FaqSectionModel.md)
 - [Model.FaqSectionModelFaqsInner](docs/FaqSectionModelFaqsInner.md)
 - [Model.GetCaptchaInfoResponse](docs/GetCaptchaInfoResponse.md)
 - [Model.GetCaptchaInfoResponseV2](docs/GetCaptchaInfoResponseV2.md)
 - [Model.GetCaptchaInfoResponseV2Variants](docs/GetCaptchaInfoResponseV2Variants.md)
 - [Model.GetCaptchaInfoResponseV2VariantsAndroid](docs/GetCaptchaInfoResponseV2VariantsAndroid.md)
 - [Model.GetCaptchaInfoResponseV3](docs/GetCaptchaInfoResponseV3.md)
 - [Model.GetCaptchaInfoResponseV3Variants](docs/GetCaptchaInfoResponseV3Variants.md)
 - [Model.ImageModel](docs/ImageModel.md)
 - [Model.LiveStreamModel](docs/LiveStreamModel.md)
 - [Model.LiveStreamModelOffline](docs/LiveStreamModelOffline.md)
 - [Model.PaymentAddressModel](docs/PaymentAddressModel.md)
 - [Model.PaymentInvoiceListV2Response](docs/PaymentInvoiceListV2Response.md)
 - [Model.PaymentInvoiceListV2ResponseInvoicesInner](docs/PaymentInvoiceListV2ResponseInvoicesInner.md)
 - [Model.PaymentInvoiceListV2ResponseInvoicesInnerSubscriptionsInner](docs/PaymentInvoiceListV2ResponseInvoicesInnerSubscriptionsInner.md)
 - [Model.PaymentInvoiceListV2ResponseInvoicesInnerSubscriptionsInnerPlan](docs/PaymentInvoiceListV2ResponseInvoicesInnerSubscriptionsInnerPlan.md)
 - [Model.PaymentInvoiceListV2ResponseInvoicesInnerSubscriptionsInnerPlanCreator](docs/PaymentInvoiceListV2ResponseInvoicesInnerSubscriptionsInnerPlanCreator.md)
 - [Model.PaymentMethodModel](docs/PaymentMethodModel.md)
 - [Model.PaymentMethodModelCard](docs/PaymentMethodModelCard.md)
 - [Model.PictureAttachmentModel](docs/PictureAttachmentModel.md)
 - [Model.PlanInfoV2Response](docs/PlanInfoV2Response.md)
 - [Model.PlanInfoV2ResponsePlansInner](docs/PlanInfoV2ResponsePlansInner.md)
 - [Model.PostMetadataModel](docs/PostMetadataModel.md)
 - [Model.SocialLinksModel](docs/SocialLinksModel.md)
 - [Model.SubscriptionPlanModel](docs/SubscriptionPlanModel.md)
 - [Model.UserActivityV3Response](docs/UserActivityV3Response.md)
 - [Model.UserActivityV3ResponseActivityInner](docs/UserActivityV3ResponseActivityInner.md)
 - [Model.UserInfoV2Response](docs/UserInfoV2Response.md)
 - [Model.UserInfoV2ResponseUsersInner](docs/UserInfoV2ResponseUsersInner.md)
 - [Model.UserLinksV3ResponseValue](docs/UserLinksV3ResponseValue.md)
 - [Model.UserLinksV3ResponseValueType](docs/UserLinksV3ResponseValueType.md)
 - [Model.UserModel](docs/UserModel.md)
 - [Model.UserNamedV2Response](docs/UserNamedV2Response.md)
 - [Model.UserNamedV2ResponseUsersInner](docs/UserNamedV2ResponseUsersInner.md)
 - [Model.UserNotificationModel](docs/UserNotificationModel.md)
 - [Model.UserNotificationModelUserNotificationSetting](docs/UserNotificationModelUserNotificationSetting.md)
 - [Model.UserNotificationUpdateV3PostRequest](docs/UserNotificationUpdateV3PostRequest.md)
 - [Model.UserSecurityV2Response](docs/UserSecurityV2Response.md)
 - [Model.UserSelfModel](docs/UserSelfModel.md)
 - [Model.UserSelfV3Response](docs/UserSelfV3Response.md)
 - [Model.UserSubscriptionModel](docs/UserSubscriptionModel.md)
 - [Model.UserSubscriptionModelPlan](docs/UserSubscriptionModelPlan.md)
 - [Model.VideoAttachmentModel](docs/VideoAttachmentModel.md)
 - [Model.VotePollRequest](docs/VotePollRequest.md)


<a name="documentation-for-authorization"></a>
## Documentation for Authorization

<a name="CookieAuth"></a>
### CookieAuth

- **Type**: API key
- **API key parameter name**: sails.sid
- **Location**: 

