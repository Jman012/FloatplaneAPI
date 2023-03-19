import requests
import schemathesis
from urllib.parse import urljoin
import os
import pytest

# https://stackoverflow.com/a/51026159/464870
class FPSession(requests.Session):
	def __init__(self, base_url=None):
		super().__init__()
		self.base_url = base_url
		self.headers.update({
			"User-Agent": "Floatplane API Docs Integration and Regression Tests v0.1.0, CFNetwork",
		})
		self.cookies.set("sails.sid", os.environ["sails.sid"])

	def request(self, method, url, *args, **kwargs):
		joined_url = urljoin(self.base_url, url)
		return super().request(method, joined_url, *args, **kwargs)

class TestFPAPIFlow():
	schema = schemathesis.from_path("../../floatplane-openapi-specification-trimmed.json", base_url="https://floatplane.com")
	session = FPSession("https://floatplane.com")

	creatorIds: set[str] = set()
	creatorUrlNames: set[str] = set()
	creatorOwnerIds: set[str] = set()

	def getValidateAndAssert(self, path: str, status: int, params: dict = None) -> requests.Response:
		response = self.session.get(path, params=params)
		print("Request GET " + path + " " + str(params) + ": " + str(response.status_code) +  " and " + str(len(response.content)) + " bytes")
		# print("Response text: " + response.text)
		self.schema[path]["GET"].validate_response(response)
		assert response.status_code == status, response.text
		return response

	@pytest.mark.dependency()
	def test_FlowStart(self):

		response = self.getValidateAndAssert("/api/v3/user/subscriptions", requests.codes.ok)
		self.creatorIds.update([sub["creator"] for sub in response.json()])

		response = self.getValidateAndAssert("/api/v3/creator/list", requests.codes.ok)
		self.creatorIds.update([creator["id"] for creator in response.json()])
		self.creatorUrlNames.update([creator["urlname"] for creator in response.json()])
		self.creatorOwnerIds.update([creator["owner"] for creator in response.json()])

	@pytest.mark.dependency(depends=["TestFPAPIFlow::test_FlowStart"])
	def test_CreatorSubscriptionPlans(self):
		for creatorId in self.creatorIds:
			response = self.getValidateAndAssert("/api/v2/plan/info", requests.codes.ok, params={"creatorId": creatorId})

