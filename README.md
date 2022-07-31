# Floatplane API Specification

Visit the pre-generated documentation, trimmed OpenAPI spec, and Postman collection at https://jman012.github.io/FloatplaneAPIDocs.

Visit Floatplane at https://www.floatplane.com.

---

This repository is an API specification of the video streaming service [Floatplane](https://www.floatplane.com) using in the [OpenAPI 3.0.3](https://swagger.io/specification/) specification for REST and [AsyncAPI 2.4.0](https://www.asyncapi.com/) specification for asynchronous events. The main files for this repository are `floatplane-openapi-specification.json`, `floatplane-asyncapi-chat-specification.json`, and `floatplane-asyncapi-frontend-specification.json`. Contained in them are definitions for all of the paths and channels in the Floatplane API, definitions of common models between the API endpoints, descriptions of authentication and authorization mechanisms, and more.

This repository serves as an open and central source specification for the Floatplane API by the community, for purposes of tinkering and creating custom clients for Floatplane. At the time of writing, Floatplane has its main website, along with both Android and iOS applications. The main use case envisioned in the creation of this repository is to make it easier to create TV-first applications for, e.g., tvOS, Roku, Google TV, etc.

# Automatic Generation

The main purpose of this repository is to enable automatic generation of documentation and client code libraries.

## OpenAPI/AsyncAPI & Code Generation

The Floatplane API specification can be used to automatically generate client code of the Floatplane API in most major programming languages. It is best advised to use the [trimmed version](https://jman012.github.io/FloatplaneAPIDocs/floatplane-openapi-specification-trimmed.json) of the REST API to only generate the endpoints that have been thoroughly documented. Various generators exist for different use cases. 

A notable open-source generator is [OpenAPI Generator](https://openapi-generator.tech/docs/generators) which supports 38 different languages, along with variations for different networking libraries in some languages. It additionally includes many configurations when generating clients. For AsyncAPI, the [AsyncAPI Generator](https://github.com/asyncapi/generator) is recommended.

It would be best to keep your version of the specification, and a script to auto-generate the library with all of the correct configurations, in source control. Then, run the script to generate the code library or files, and keep those in source control as well in your project, or execute the script in your build scripts.

### Example

```sh
openapi-generator openapi-generator generate -i floatplane-openapi-specification-trimmed.json -o Swift -g swift5 --library vapor
```

```sh
ag -o FloatplaneChatAPI floatplane-asyncapi-chat-specification.json @asyncapi/nodejs-template
```

## OpenAPI/AsyncAPI & Documentation Generation

The API specifications can also be used to generate documentation. Pre-generated renders of the documentation for this repository are available at https://jman012.github.io/FloatplaneAPIDocs. There are a variety of renders available, including:
- Swagger UI - https://github.com/swagger-api/swagger-ui
- Redoc - https://redoc.ly/redoc
- ReSlate & Widdershins - https://github.com/Mermade/reslate - https://github.com/Mermade/widdershins
- RapiDoc - https://mrin9.github.io/RapiDoc/
- OpenAPI Generator - https://openapi-generator.tech/
- OpenAPI to Postman v2.1 Converter - https://github.com/postmanlabs/openapi-to-postman
- AsyncAPI Generator - https://github.com/asyncapi/generator

### Example

```sh
redoc-cli bundle -o Docs/Redoc/redoc-static.html floatplane-openapi-specification.json
```

```sh
ag -o Docs/AsyncAPIChat floatplane-asyncapi-chat-specification.json @asyncapi/html-template
```

---

## Working on This Repository

### Documentation Generation

In order to generate all of the documentation available at https://jman012.github.io/FloatplaneAPIDocs automatically when testing changes to the OpenAPI file,
1. Clone this repository
2. Run `npm install`
	1. This will install all of the necessary tooling
	2. The OpenAPI Generator tooling requires Java to be installed, but will fail silently if it is not.
	3. Note that for AsyncAPI, it depends on installing `puppeteer`. If working on an M1 Apple device, you may run into issues with this dependency. Following [this article](https://linguinecode.com/post/how-to-fix-m1-mac-puppeteer-chromium-arm64-bug) may help.
3. Make changes as necessary to `floatplane-openapi-specification.json` or the AsyncAPI specification files
4. Run `npm run docs-all`. This will:
	1. Trim the spec into `floatplane-openapi-specification-trimmed.json`
	2. Generate documentation for the trimmed spec into the `/Docs` folder
	3. Generate documentation for the full spec into the `/Docs` folder
	4. Copy over the spec and some other files from `/static` into the `/Docs` folder

Then, open `/Docs/index.html` to view the changes. A Dockerfile is also available:

```sh
docker build --tag fpapidocs:latest .
```

### Analyze difference between Floatplane frontends

The list of APIs was generated from the Floatplane frontend files, available at https://frontend.floatplane.com/{version}/*.js

When a new version of the Floatplane frontend is released (which is done silently), we can analyze the differences between the files to find new endpoints being used. To do so more easily, some tools are included:

1. Clone this repository
2. Change directory into the `/src` folder: `cd src`
3. Fetch the frontend files for the **previous** version: `./fp-frontend-fetch.sh <previous version number>`
	1. E.g. `./fp-frontend-fetch.sh 3.5.1`
	2. This assumes that `wget` is installed on the system
4. Fetch the frontend files for the **current** version: `./fp-frontend-fetch.sh <previous version number>`
	1. E.g. `./fp-frontend-fetch.sh 3.5.1-a`
5. Un-minify the files: `prettier --write Frontend`
	1. This assumes that [Prettier](https://prettier.io/) is installed on the system
6. Perform a quick diff to clean up the files and see which files differ: `./fp-frontend-diff.sh <previous version> <current version>`
	1. E.g. `./fp-frontend-diff.sh 3.5.1 3.5.1-a`
	2. The cleanup replaces references of the version numbers in, specifically, `app.js` with a common piece of text in order to avoid many false-positives in the resulting diffs.
7. Then, manually inspect the diff of the files listed to see what has changed.

The file `fp-frontend-version.txt` is a collection of recent version changes that Floatplane has made, starting with `3.5.1`. This may be updated irregularly.

### Integration Testing

After making changes to the `floatplane-openapi-specification.json`, run `npm run test` in order to run integration tests with the Floatplane API. This ensures that the specification and its models are aligned with the API correctly.

The environment variable "sailssid" needs to be set to the value of the `sails.sid` HTTP Cookie for authentication in order for integration tests to run.

Integration tests will test for:
- Expected HTTP 200 responses for valid requests
- Expected HTTP 400/401/403/404 responses for invalid requests
- A strict match between the response data from FP and the response schema/model in the specification:
	- Case-sensitive property matching
	- Case-sensitive enumeration matching
	- No missing properties in the schema (if new properties begin appearing in FP response objects we want to begin tracking them)
	- No extraneous properties in schema (if a property isn't in the FP response then it shouldn't be in the schema)
	- Exact type matching (a string with a number in it should not be equivalent to a number)

Write integration/unit tests in the `tests/FloatplaneAPIClientCSharpTester` C# project.

#### Structure

All integration test code is in `tests/`. The trimmed specification is generated and validated. A C# (csharp) client is generated with OpenAPI Generator with strict settings (see `tests/generate-csharp.sh`) into the git-ignored `tests/FloatplaneAPIClientCSharp` folder with a static project GUID. This contains all of the Models and APIs in normal code generation. The unit test project resides in VCS in `tests/FloatplaneAPIClientCSharpTester` and already links to the client project in the other folder.

The FloatplaneAPIClientCSharpTester was generated using `tests/regenerate-csharp-test-library.sh` file. This also uses the OpenAPI Generator to generate the skeleton with API unit tests (no Model unit tests). It then removes the client library and re-links project references to the main client in `tests/FloatplaneAPIClientCSharp`. After this, it can be built tests can be run. 

Note that re-generating this unit test project will wipe out existing unit tests. This will be useful to add new endpoint API tests when new APIs are introduced by Floatplane, but you should use VCS change tracking to re-do the existing unit tests.

---

# Contributions

Anyone is free to help
- Document undocumented API endpoints
- Add more information to already-documented endpoints
- Fix issues with documentation
- Restructure models
- Add more pre-generated docs
- Etc.

# License

MIT License. See the LICENSE file.
