import requests
import schemathesis
from urllib.parse import urljoin
import os
import pytest
import time
import random
import json

# https://stackoverflow.com/a/51026159/464870
class FPSession(requests.Session):
	def __init__(self, base_url=None):
		super().__init__()
		self.base_url = base_url
		self.headers.update({
			"User-Agent": "Floatplane API Docs Integration and Regression Automated Tests v0.1.0, CFNetwork",
		})
		self.cookies.set("sails.sid", os.environ["SAILS_SID"])

	def request(self, method, url, *args, **kwargs):
		joined_url = urljoin(self.base_url, url)
		return super().request(method, joined_url, *args, **kwargs)

class TestFPAPIFlow():
	schema = schemathesis.from_path("../../floatplane-openapi-specification-trimmed.json", base_url="https://floatplane.com")
	session = FPSession("https://floatplane.com")

	def getValidateAndAssert(self, path: str, status: list[int] = [requests.codes.ok], params: dict = None) -> requests.Response:
		time.sleep(1)
		response = self.session.get(path, params=params)
		print("Request GET " + path + " " + str(params) + ": " + str(response.status_code) +  " and " + str(len(response.content)) + " bytes")
		# print("Response text: " + response.text)
		self.schema[path]["GET"].validate_response(response)
		assert response.status_code in status, response.text
		if response.status_code != requests.codes.ok:
			print("WARN: Response was not 200 OK. Instead: " + str(response.status_code))
		return response

	"""
	First level of tests.
	Dependencies: None
	Retrieves: Subscriptions, top-level lists (list of creators), user information, etc.
	"""
	subscribedCreatorIds: set[str] = set()
	subscribedLivestreamIds: set[str] = set()
	creatorIds: set[str] = set()
	creatorUrlNames: set[str] = set()
	creatorOwnerIds: set[str] = set()

	@pytest.mark.dependency()
	def test_CreatorSubscriptionPlanV2(self):
		print()
		print("SubscriptionsV3 List User Subscriptions")
		response = self.getValidateAndAssert("/api/v3/user/subscriptions")
		self.subscribedCreatorIds.update([sub["creator"] for sub in response.json()])

	@pytest.mark.dependency()
	def test_LoadCreators(self):
		print()
		print("CreatorV3 Get Creators")
		response = self.getValidateAndAssert("/api/v3/creator/list")
		self.creatorIds.update([creator["id"] for creator in response.json()])
		self.creatorUrlNames.update([creator["urlname"] for creator in response.json()])
		self.creatorOwnerIds.update([creator["owner"] for creator in response.json()])
		self.subscribedLivestreamIds.update([creator["liveStream"]["id"] for creator in response.json() if creator["id"] in self.subscribedCreatorIds])

	@pytest.mark.dependency()
	def test_EdgesV2(self):
		print()
		print("EdgesV2 Get Edges")
		response = self.getValidateAndAssert("/api/v2/edges")

	@pytest.mark.dependency()
	def test_PaymentsV2(self):
		print()
		print("PaymentsV2 List Payment Methods")
		response = self.getValidateAndAssert("/api/v2/payment/method/list")

		print("PaymentsV2 List Address")
		response = self.getValidateAndAssert("/api/v2/payment/address/list")

		print("PaymentsV2 List Invoices")
		response = self.getValidateAndAssert("/api/v2/payment/invoice/list")

	@pytest.mark.dependency()
	def test_FAQV2(self):
		print()
		print("FAQV2 Get FAQs")
		response = self.getValidateAndAssert("/api/v2/faq/list")

	@pytest.mark.dependency()
	def test_ConnectedAccountsV2(self):
		print()
		print("ConnectedAccountsV2 List Connections")
		response = self.getValidateAndAssert("/api/v2/connect/list")

	@pytest.mark.dependency()
	def test_UserV2BanStatus(self):
		print()
		print("V2 Get User Creator Ban Status")
		for creatorId in self.subscribedCreatorIds:
			response = self.getValidateAndAssert("/api/v2/user/ban/status", params={"creator": creatorId})

	"""
	Second level of tests.
	Dependencies: Creator names/ids
	Retrieves: Creator information, Subscription Plan information
	"""

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_CreatorSubscriptionPlans(self):
		print()
		print("CreatorSubscriptionPlanV2 Get Creator Sub Info Public")
		for creatorId in self.creatorIds:
			response = self.getValidateAndAssert("/api/v2/plan/info", params={"creatorId": creatorId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_CreatorV2GetInfo(self):
		print()
		print("CreatorV2 Get Info")
		for creatorId in self.creatorIds:
			response = self.getValidateAndAssert("/api/v2/creator/info", params={"creatorGUID[0]": creatorId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_CreatorV2GetInfoByName(self):
		print()
		print("CreatorV2 Get Info By Name")
		for creatorUrlName in self.creatorUrlNames:
			response = self.getValidateAndAssert("/api/v2/creator/named", params={"creatorURL[0]": creatorUrlName})
			assert len(response.json()) > 0

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_CreatorV3GetCreator(self):
		print()
		print("CreatorV3 Get Creator")
		for creatorId in self.creatorIds:
			response = self.getValidateAndAssert("/api/v3/creator/info", params={"id": creatorId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_CreatorV3GetCreatoryName(self):
		print()
		print("CreatorV3 Get Creator by Name")
		for creatorUrlName in self.creatorUrlNames:
			response = self.getValidateAndAssert("/api/v3/creator/named", params={"creatorURL[0]": creatorUrlName})
			assert len(response.json()) > 0

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_CreatorV3ListCreatorChannels(self):
		print()
		print("CreatorV3 List Creator Channels")
		for creatorId in self.creatorIds:
			response = self.getValidateAndAssert("/api/v3/creator/channels/list", params={"ids[0]": creatorId})
			assert len(response.json()) > 0

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_ContentV3GetContentTags(self):
		print()
		print("V3 Get Content Tags")
		for creatorId in self.creatorIds:
			response = self.getValidateAndAssert("/api/v3/content/tags", params={"creatorIds[0]": creatorId})

	"""
	Third level of tests.
	Dependencies: Creator names/ids
	Retrieves: Content Lists
	"""
	blogPostIds: set[(str, str)] = set()

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_ContentV3GetCreatorBlogPosts(self):
		print()
		print("V3 Get Creator Blog Posts")
		limit = 2 # Get 2 per page
		for creatorId in self.creatorIds:
			# Get latest and earliest
			for sort in ["DESC", "ASC"]:
				# Get various post type combinations
				for type in [{}, {"hasVideo": True}, {"hasAudio": True}, {"hasPicture": True}, {"hasText": True}]:
					# Get posts
					params = dict({"id": creatorId, "limit": limit, "sort": sort}, **type)
					response = self.getValidateAndAssert("/api/v3/content/creator", params=params)
					self.blogPostIds.update([(creatorId, blogPost["id"]) for blogPost in response.json()])

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_LoadCreators", "TestFPAPIFlow::test_CreatorSubscriptionPlanV2"])
	def test_ContentV3GetMultiCreatorBlogPosts(self):
		print()
		print("V3 Get Multi Creator Blog Posts")
		params = dict()
		for (i, creatorId) in enumerate(self.subscribedCreatorIds):
			params["ids[" + str(i) + "]"] = creatorId
		response = self.getValidateAndAssert("/api/v3/content/creator/list", params=params)

		params["fetchAfter"] = json.dumps(response.json()["lastElements"])
		response = self.getValidateAndAssert("/api/v3/content/creator/list", params=params)

	"""
	Fourth level of tests.
	Dependencies: Content ids
	Retrieves: Content information, content comments
	"""
	videoAttachmentIds: set[(str, str, str)] = set()
	audioAttachmentIds: set[(str, str, str)] = set()
	pictureAttachmentIds: set[(str, str, str)] = set()

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetCreatorBlogPosts"])
	def test_ContentV3GetBlogPost(self):
		print()
		print("V3 Get Blog Post")
		for (creatorId, blogPostId) in self.blogPostIds:
			if creatorId not in self.subscribedCreatorIds:
				continue
			response = self.getValidateAndAssert("/api/v3/content/post", status=[requests.codes.ok, requests.code.forbidden], params={"id": blogPostId})
			self.videoAttachmentIds.update([(creatorId, blogPostId, x["id"]) for x in response.json()["videoAttachments"]])
			self.audioAttachmentIds.update([(creatorId, blogPostId, x["id"]) for x in response.json()["audioAttachments"]])
			self.pictureAttachmentIds.update([(creatorId, blogPostId, x["id"]) for x in response.json()["pictureAttachments"]])

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetCreatorBlogPosts"])
	def test_ContentV3GetRelatedBlogPosts(self):
		print()
		print("V3 Get Related Blog Posts")
		for (creatorId, blogPostId) in self.blogPostIds:
			if creatorId not in self.subscribedCreatorIds:
				continue
			response = self.getValidateAndAssert("/api/v3/content/related", status=[requests.codes.ok, requests.code.forbidden], params={"id": blogPostId})

	"""
	Fourth level of tests.
	Dependencies: Content ids
	Retrieves: Attachment information
	"""

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetBlogPost"])
	def test_ContentV3GetVideoContent(self):
		print()
		print("V3 Get Video Content")
		for (creatorId, blogPostId, videoAttachmentId) in self.videoAttachmentIds:
			if creatorId not in self.subscribedCreatorIds:
				continue
			response = self.getValidateAndAssert("/api/v3/content/video", status=[requests.codes.ok, requests.code.forbidden], params={"id": videoAttachmentId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetBlogPost"])
	def test_ContentV3GetPictureContent(self):
		print()
		print("V3 Get Picture Content")
		for (creatorId, blogPostId, pictureAttachmentId) in self.pictureAttachmentIds:
			if creatorId not in self.subscribedCreatorIds:
				continue
			response = self.getValidateAndAssert("/api/v3/content/picture", status=[requests.codes.ok, requests.code.forbidden], params={"id": pictureAttachmentId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetBlogPost"])
	def test_DeliveryV3GetDeliveryInfo(self):
		print()
		print("V3 Get Delivery Info - On Demand")
		limit = 1
		sleepDuration = 35 # 35 seconds inbetween each. Current rate limit is 2 req per min
		for (creatorId, blogPostId, videoAttachmentId) in random.sample(list(self.videoAttachmentIds), limit):
			if creatorId not in self.subscribedCreatorIds:
				continue
			response = self.getValidateAndAssert("/api/v3/delivery/info", status=[requests.codes.ok, requests.code.forbidden], params={"scenario": "onDemand", "entityId": videoAttachmentId})
			time.sleep(sleepDuration)

		print("V3 Get Delivery Info - Download")
		for (creatorId, blogPostId, videoAttachmentId) in random.sample(list(self.videoAttachmentIds), limit):
			if creatorId not in self.subscribedCreatorIds:
				continue
			response = self.getValidateAndAssert("/api/v3/delivery/info", status=[requests.codes.ok, requests.code.forbidden], params={"scenario": "download", "entityId": videoAttachmentId})
			time.sleep(sleepDuration)

		print("V3 Get Delivery Info - Livestream")
		for liveStreamId in self.subscribedLivestreamIds:
			response = self.getValidateAndAssert("/api/v3/delivery/info", status=[requests.codes.ok, requests.code.forbidden], params={"scenario": "live", "entityId": liveStreamId})
			time.sleep(sleepDuration)

	"""
	Sixth level of tests.
	Dependencies: User ids/names
	Retrieves: User information
	"""

	@pytest.mark.dependency(depends=[])
	def test_UserV2V3GetSelf(self):
		print()
		print("V3 Get Self")
		response = self.getValidateAndAssert("/api/v3/user/self")
		id = response.json()["id"]
		username = response.json()["username"]

		print()
		print("V3 Get User Notification List")
		response = self.getValidateAndAssert("/api/v3/user/notification/list")

		print()
		print("V3 Get External Links")
		response = self.getValidateAndAssert("/api/v3/user/links", params={"id": id})

		print()
		print("V3 Get External Activity")
		response = self.getValidateAndAssert("/api/v3/user/activity", params={"id": id})

		print()
		print("V2 Get User Info")
		response = self.getValidateAndAssert("/api/v2/user/info", params={"id": id})

		print()
		print("V2 Get User Info By Name")
		response = self.getValidateAndAssert("/api/v2/user/named", params={"username": username})

		print()
		print("V2 Get User Security")
		response = self.getValidateAndAssert("/api/v2/user/security")
