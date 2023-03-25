import requests
import schemathesis
from urllib.parse import urljoin
import os
import pytest
import time

# https://stackoverflow.com/a/51026159/464870
class FPSession(requests.Session):
	def __init__(self, base_url=None):
		super().__init__()
		self.base_url = base_url
		self.headers.update({
			"User-Agent": "Floatplane API Docs Integration and Regression Automated Tests v0.1.0, CFNetwork",
		})
		self.cookies.set("sails.sid", os.environ["sails.sid"])

	def request(self, method, url, *args, **kwargs):
		joined_url = urljoin(self.base_url, url)
		return super().request(method, joined_url, *args, **kwargs)

class TestFPAPIFlow():
	schema = schemathesis.from_path("../../floatplane-openapi-specification-trimmed.json", base_url="https://floatplane.com")
	session = FPSession("https://floatplane.com")

	def getValidateAndAssert(self, path: str, status: int = requests.codes.ok, params: dict = None) -> requests.Response:
		response = self.session.get(path, params=params)
		print("Request GET " + path + " " + str(params) + ": " + str(response.status_code) +  " and " + str(len(response.content)) + " bytes")
		# print("Response text: " + response.text)
		self.schema[path]["GET"].validate_response(response)
		assert response.status_code == status, response.text
		return response

	"""
	First level of tests.
	Dependencies: None
	Retrieves: Subscriptions, top-level lists (list of creators), user information, etc.
	"""
	creatorIds: set[str] = set() # Populated by test_LoadCreators
	creatorUrlNames: set[str] = set() # Populated by test_LoadCreators
	creatorOwnerIds: set[str] = set() # Populated by test_LoadCreators

	@pytest.mark.dependency()
	def test_CreatorSubscriptionPlanV2(self):
		print()
		print("SubscriptionsV3 List User Subscriptions")
		response = self.getValidateAndAssert("/api/v3/user/subscriptions")
		self.creatorIds.update([sub["creator"] for sub in response.json()])

	@pytest.mark.dependency()
	def test_LoadCreators(self):
		print()
		print("CreatorV3 Get Creators")
		response = self.getValidateAndAssert("/api/v3/creator/list")
		self.creatorIds.update([creator["id"] for creator in response.json()])
		self.creatorUrlNames.update([creator["urlname"] for creator in response.json()])
		self.creatorOwnerIds.update([creator["owner"] for creator in response.json()])

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
		limit = 5 # Get 5 per page
		for creatorId in self.creatorIds:
			# Get latest and earliest
			for sort in ["DESC", "ASC"]:
				# Get various post type combinations
				for type in [{}, {"hasVideo": True}, {"hasAudio": True}, {"hasPicture": True}, {"hasText": True}, {"hasVideo": True, "hasAudio": True}, {"hasVideo": True, "hasPicture": True}, {"hasAudio": True, "hasPicture": True}, {"hasVideo": True, "hasAudio": True, "hasPicture": True}]:
					# Get posts
					print(dict({"id": creatorId, "limit": limit, "sort": sort}, **type))
					params = dict({"id": creatorId, "limit": limit, "sort": sort}, **type)
					response = self.getValidateAndAssert("/api/v3/content/creator", params=params)
					self.blogPostIds.update([(creatorId, blogPost["id"]) for blogPost in response.json()])

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
			response = self.getValidateAndAssert("/api/v3/content/post", params={"id": blogPostId})
			self.videoAttachmentIds.update([(creatorId, blogPostId, x["id"]) for x in response.json()["videoAttachments"]])
			self.audioAttachmentIds.update([(creatorId, blogPostId, x["id"]) for x in response.json()["audioAttachments"]])
			self.pictureAttachmentIds.update([(creatorId, blogPostId, x["id"]) for x in response.json()["pictureAttachments"]])

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetCreatorBlogPosts"])
	def test_ContentV3GetRelatedBlogPosts(self):
		print()
		print("V3 Get Related Blog Posts")
		for (creatorId, blogPostId) in self.blogPostIds:
			response = self.getValidateAndAssert("/api/v3/content/post", params={"id": blogPostId})

	"""
	Fourth level of tests.
	Dependencies: Content ids
	Retrieves: Attachment information
	"""

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetBlogPost"])
	def test_ContentV3GetVideoContent(self):
		print()
		print("V3 Get Video Content")
		for (creator, blogPostId, videoAttachmentId) in self.videoAttachmentIds:
			response = self.getValidateAndAssert("/api/v3/content/video", params={"id": videoAttachmentId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetBlogPost"])
	def test_ContentV3GetPictureContent(self):
		print()
		print("V3 Get Picture Content")
		for (creator, blogPostId, pictureAttachmentId) in self.pictureAttachmentIds:
			response = self.getValidateAndAssert("/api/v3/content/picture", params={"id": pictureAttachmentId})

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_ContentV3GetBlogPost"])
	def test_DeliveryV3GetDeliveryInfo(self):
		print()
		print("V3 Get Delivery Info - On Demand")
		for (creator, blogPostId, videoAttachmentId) in self.videoAttachmentIds:
			response = self.getValidateAndAssert("/api/v3/delivery/info", params={"scenario": "onDemand", "entityId": videoAttachmentId})
			time.sleep(1)

		print("V3 Get Delivery Info - Download")
		for (creator, blogPostId, videoAttachmentId) in self.videoAttachmentIds:
			response = self.getValidateAndAssert("/api/v3/delivery/info", params={"scenario": "download", "entityId": videoAttachmentId})
			time.sleep(1)

	"""
	Sixth level of tests.
	Dependencies: User ids/names
	Retrieves: User information
	"""