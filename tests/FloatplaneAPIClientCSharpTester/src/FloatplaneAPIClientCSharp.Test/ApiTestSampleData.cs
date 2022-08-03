using System;
using System.Collections.Generic;
using System.Linq;

namespace FloatplaneAPIClientCSharp
{
	internal class ApiTestSampleData
	{
		public static readonly string SampleUserId = "5fb69b4f8573b6cd8cc7f3b0"; // Jamamp
		public static readonly string LttCreatorId = "59f94c0bdd241b70349eb72b"; // LinusTechTips
		public static readonly string LttCreatorName = "linustechtips"; // LinusTechTips

		/**
		 * Recent post that is video-only
		 * https://www.floatplane.com/post/akb4ollEJb
		 * 2022-08-02
		 * SC: This case is a breath of fresh air - Fractal Pop Air
		 */
		public static readonly string Post_Video_New = "akb4ollEJb";
		/**
		 * Oldest post that is video-only
		 * https://www.floatplane.com/post/98J06sQfmp
		 * 2017-10-02
		 * THIS IS A TEST
		 */
		public static readonly string Post_Video_Old = "98J06sQfmp";
		/**
		 * Recent post with video and audio
		 * https://www.floatplane.com/post/vcVKQzYhYl
		 * 2022-07-21
		 * TJM: Get Out (2017) Movie Review
		 */
		public static readonly string Post_VideoAudio_New = "vcVKQzYhYl";
		/**
		 * Oldest post with video and audio
		 * https://www.floatplane.com/post/z3UFumAkdb
		 * 2021-08-23
		 * FP Exclusive: Interview w/ Ahmad Byagowi on the Time Card
		 */
		public static readonly string Post_VideoAudio_Old = "z3UFumAkdb";
		/**
		 * Recent post with video and picture
		 * https://www.floatplane.com/post/fEdRPKXVo2
		 * 2022-05-16
		 * Today's video will be late! In the meantime...
		 */
		public static readonly string Post_VideoPicture_New = "fEdRPKXVo2";
		/**
		 * Oldest post with video and picture
		 * https://www.floatplane.com/post/f5Yoe9BoLs
		 * 2020-07-27
		 * Get this BEFORE your PS5 - Vizio P-series Quantum X 2021
		 */
		public static readonly string Post_VideoPicture_Old = "f5Yoe9BoLs";
		/**
		 * Recent post with video audio and picture
		 * None yet
		 */
		public static readonly string Post_VideoAudioPicture_New = "";
		/**
		 * Oldest post with video audio and picture
		 * None yet
		 */
		public static readonly string Post_VideoAudioPicture_Old = "";
		/**
		 * Recent post with audio only
		 * None yet
		 */
		public static readonly string Post_Audio_New = "";
		/**
		 * Oldest post with audio only
		 * None yet
		 */
		public static readonly string Post_Audio_Old = "";
		/**
		 * Recent post with picture only
		 * https://www.floatplane.com/post/fOGeGyd3vW
		 * 2022-07-23
		 * Waiting for something? Need a pet distraction?
		 */
		public static readonly string Post_Picture_New = "fOGeGyd3vW";
		/**
		 * Oldest post with picture only
		 * https://www.floatplane.com/post/xyMCzMgL5j
		 * 2021-02-01
		 * FP Exclusive: BTS Model Y Review Pictures - Intro Production Day
		 */
		public static readonly string Post_Picture_Old = "xyMCzMgL5j";
		/**
		 * Recent post with text only
		 * https://www.floatplane.com/post/eF8I0C5JTO
		 * 2022-07-29
		 * Wanted: HDR Feedback
		 */
		public static readonly string Post_Text_New = "eF8I0C5JTO";
		/**
		 * Oldest post with text only
		 * https://www.floatplane.com/post/sZ31X97U6b
		 * 2021-01-23
		 * Missing videos for floatplane members
		 */
		public static readonly string Post_Text_Old = "sZ31X97U6b";

		public static readonly IReadOnlyList<string> AllPosts = new List<string>()
		{
			Post_Video_New,
			Post_Video_Old,
			Post_VideoAudio_New,
			Post_VideoAudio_Old,
			Post_VideoPicture_New,
			Post_VideoPicture_Old,
			Post_VideoAudioPicture_New,
			Post_VideoAudioPicture_Old,
			Post_Audio_New,
			Post_Audio_Old,
			Post_Picture_New,
			Post_Picture_Old,
			Post_Text_New,
			Post_Text_Old,
		}.Where(p => !string.IsNullOrEmpty(p)).ToList();


		/**
		 * Recent video attachment
		 * From https://www.floatplane.com/post/akb4ollEJb
		 * 2022-08-02
		 * SC: This case is a breath of fresh air - Fractal Pop Air
		 */
		public static readonly string Video_New = "rV9hLpfIOo";
		/**
		 * Oldest video attachment
		 * From https://www.floatplane.com/post/98J06sQfmp
		 * 2017-10-02
		 * THIS IS A TEST
		 */
		public static readonly string Video_Old = "MdjXc244BA";
		/**
		 * Recent picture attachment
		 * From https://www.floatplane.com/post/fOGeGyd3vW
		 * 2022-07-23
		 * Waiting for something? Need a pet distraction?
		 */
		public static readonly string Picture_New = "Br0WZmtFGz";
		/**
		 * Oldest picture attachment
		 * From https://www.floatplane.com/post/f5Yoe9BoLs
		 * 2020-07-27
		 * Get this BEFORE your PS5 - Vizio P-series Quantum X 2021
		 */
		public static readonly string Picture_Old = "EA80PShv93";
		/**
		 * Recent audio attachment
		 * From https://www.floatplane.com/post/vcVKQzYhYl
		 * 2022-07-21
		 * TJM: Get Out (2017) Movie Review
		 */
		public static readonly string Audio_New = "gaS0cGfybt";
		/**
		 * Oldest audio attachment
		 * From https://www.floatplane.com/post/z3UFumAkdb
		 * 2021-08-23
		 * FP Exclusive: Interview w/ Ahmad Byagowi on the Time Card
		 */
		public static readonly string Audio_Old = "WZsXgPNdgV";

		public static readonly IReadOnlyList<string> AllAttachments = new List<string>()
		{
			Video_New,
			Video_Old,
			Audio_New,
			Audio_Old,
			Picture_New,
			Picture_Old,
		}.Where(p => !string.IsNullOrEmpty(p)).ToList();
	}
}
