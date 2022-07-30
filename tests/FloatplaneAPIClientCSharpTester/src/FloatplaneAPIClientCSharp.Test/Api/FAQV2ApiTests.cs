/*
 * Floatplane REST API
 *
 * Homepage: [https://jman012.github.io/FloatplaneAPIDocs](https://jman012.github.io/FloatplaneAPIDocs)    This document describes the REST API layer of [https://www.floatplane.com](https://www.floatplane.com), a content creation and video streaming website created by Floatplane Media Inc. and Linus Media Group, where users can support their favorite creates via paid subscriptions in order to watch their video and livestream content in higher quality and other perks.    While this document contains stubs for all of the Floatplane APIs for this version, many are not filled out because they are related only to content creation, moderation, or administration and are not needed for regular use. These have \"TODO\" as the description, and are automatically removed before document generation. If you are viewing the \"Trimmed\" version of this document, they have been removed for brevity.    ## API Object Organization    - **Users** and **Creators** exist on Floatplane at the highest level   - The highest-level object in Floatplane is the Creator. This is an entity, such as Linus Tech Tips, that produces media for Users.  - A Creator owns one or more **Subscription Plans**  - A User can view a Creator's Content if they are subscribed to them  - A Creator publishes **Content**, in the form of **Blog Posts**   - Content is produced by Creators, and show up for subscribed Users to view when it is released. A piece of Content is meant to be generic, and may contain different types of sub-Content. Currently, the only type is a Blog Post.   - A Blog Post is the main type of Content that a Creator produces. Blog Posts are how a Creator can share text and/or media attachments with their subscribers.  - A Blog Post is comprised of one or more of: video, audio, picture, or gallery **Attachments**   - A media Attachment may be: video, audio, picture, gallery. Attachments are a part of Blog Posts, and are in a particular order.  - A Creator may also have a single **Livestream**    ## API Flow    As of Floatplane version 3.5.1, these are the recommended endpoints to use for normal operations.    1. Login   1. `/api/v3/auth/captcha/info` - Get captcha information   1. `/api/v2/auth/login` - Login with username, password, and optional captcha token   1. `/api/v2/auth/checkFor2faLogin` - Optionally provide 2FA token to complete login   1. `/api/v2/auth/logout` - Logout at a later point in time  1. Home page   1. `/api/v3/user/subscriptions` - Get the user's active subscriptions   1. `/api/v3/content/creator/list` - Using the subscriptions, show a home page with content from all subscriptions    1. Supply all creator identifiers from the subscriptions    1. This should be paginated   1. `/api/v2/creator/info` - Also show a list of creators that the user can select    1. Note that this can search and return multiple creators. The V3 version only works for a single creator at a time.  1. Creator page   1. `/api/v3/creator/info` - Get more details for the creator to display, including if livestreams are available   1. `/api/v3/content/creator` - Show recent content by the creator   1. `/api/v2/plan/info` - Show available plans the user can subscribe to for the creator  1. Content page   1. `/api/v3/content/post` - Show more detailed information about a piece of content, including text description, available attachments, metadata, interactions, etc.   1. `/api/v3/content/related` - List some related content for the user to watch next   1. `/api/v3/comment` - Load comments for the content for the user to read    1. There are several more comment APIs to post, like, dislike, etc.   1. `/api/v2/user/ban/status` - Determine if the user is banned from this creator   1. `/api/v3/content/{video|audio|picture|gallery}` - Load the attached media for the post. This is usually video, but audio, pictures, and galleries are also available.   1. `/api/v2/cdn/delivery` - For video and audio, this is required to get the information to stream or download the content in media players  1. Livestream   1. `/api/v2/cdn/delivery` - Using the type \"livestream\" to load the livestream media in a media player   1. `wss://chat.floatplane.com/sails.io/?...` - To connect to the livestream chat over websocket. TODO: Map out the WebSocket API.  1. User Profile   1. `/api/v3/user/self` - Display username, name, email, and profile pictures    ## API Organization    The organization of APIs into categories in this document are reflected from the internal organization of the Floatplane website bundled code, from `frontend.floatplane.com/{version}/vendor.js`. This is in order to use the best organization from the original developers' point of view.    For instance, Floatplane's authentication endpoints are organized into `Auth.v2.login(...)`, `Auth.v2.logout()`, and `Auth.v3.getCaptchaInfo()`. A limitation in OpenAPI is the lack of nested tagging/structure, so this document splits `Auth` into `AuthV2` and `AuthV3` to emulate the nested structure.    ## Notes    Note that the Floatplane API does support the use of [ETags](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag) for retrieving some information, such as retrieving information about creators, users, etc. Expect an HTTP 304 if the content has not changed, and to re-use cached responses. This is useful to ease the strain on Floatplane's API server.    The date-time format used by Floatplane API is not standard ISO 8601 format. The dates/times given by Floatplane include milliseconds. Depending on your code generator, you may need to override the date-time format to something similar to `yyyy-MM-dd'T'HH:mm:ss.SSSZ`, for both encoding and decoding.
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
    ///  Class for testing FAQV2Api
    /// </summary>
    /// <remarks>
    /// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
    /// Please update the test case below to test the API endpoint.
    /// </remarks>
    public class FAQV2ApiTests : IDisposable
    {
        private FAQV2Api instance;

        public FAQV2ApiTests()
        {
            instance = new FAQV2Api();
        }

        public void Dispose()
        {
            // Cleanup when everything is done.
        }

        /// <summary>
        /// Test an instance of FAQV2Api
        /// </summary>
        [Fact]
        public void InstanceTest()
        {
            // TODO uncomment below to test 'IsType' FAQV2Api
            //Assert.IsType<FAQV2Api>(instance);
        }

        /// <summary>
        /// Test GetFaqSections
        /// </summary>
        [Fact]
        public void GetFaqSectionsTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //var response = instance.GetFaqSections();
            //Assert.IsType<List<FaqSectionModel>>(response);
        }
    }
}
