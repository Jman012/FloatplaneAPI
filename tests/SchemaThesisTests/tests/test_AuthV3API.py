import schemathesis

schema = schemathesis.from_path("../../floatplane-openapi-specification-trimmed.json", base_url="https://floatplane.com")

@schema.parametrize(endpoint="/api/v3/auth/captcha/info")
def test_AuthAPIV3_CaptchaInfo(case):
    case.call_and_validate()
