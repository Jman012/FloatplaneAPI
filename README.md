# Floatplane API Specification

Visit the pre-generated documentation renders at https://jman012.github.io/FloatplaneAPIDocs

---

An API specification of the content streaming service https://www.floatplane.com represented in the [OpenAPI 3.0.3](https://swagger.io/specification/) specification. The main file for this repository is `floatplane-openapi-specification.json`. Contained in it are definitions for all of the paths in the Floatplane API, definitions of common models between the API endpoints, descriptions of authentication and authorization mechanisms, and more.

This repository serves as an open and central source specification for the Floatplane API, for purposes of tinkering and creating custom clients for Floatplane. At the time of writing, Floatplane has its main website, along with both Android and iOS applications. The main use case envisioned in the creation of this repository is to make it easier to create TV-first applications for, e.g., tvOS, Roku, Google TV, etc.

## OpenAPI & Code Generation

Given that the majority of this repository is in the OpenAPI specification, the `floatplane-openapi-specification.json` file can be used to automatically generate documentation, client code, and even server code of the Floatplane API. Various generators exist for different use cases. A notable open-source generator is [OpenAPI Generator](https://openapi-generator.tech/).

### Example

```sh
openapi-generator openapi-generator generate -i floatplaneopenapi_v10.json -o Swift -g swift5 --library vapor
```

## OpenAPI & Documentation Generation

You can visit a live render of the documentation for this repository at https://jman012.github.io/FloatplaneAPIDocs. There are a variety of renders available, including:
- OpenAPI Generator - https://openapi-generator.tech/
- Redoc - https://redoc.ly/redoc
- RapiDoc - https://mrin9.github.io/RapiDoc/
- ReSlate & Widdershins - https://github.com/Mermade/reslate - https://github.com/Mermade/widdershins
- OpenAPI to Postman v2.1 Converter - https://github.com/postmanlabs/openapi-to-postman

### Example

```sh
redoc-cli bundle -o Docs/Redoc/redoc-static.html floatplane-openapi-specification.json
```

## Using This Repository

### Documentation Generation

In order to generate all of the documentation available at https://jman012.github.io/FloatplaneAPIDocs automatically when testing changes to the OpenAPI file,
1. Clone this repository
2. Run `npm install`
	1. This will install all of the necessary tooling
	2. The OpenAPI Generator tooling requires Java to be installed, but will fail silently if it is not.
3. Make changes as necessary to `floatplane-openapi-specification.json`
4. Run `npm run docs-all`. This will:
	1. Trim the spec into `floatplane-openapi-specification-trimmed.json`
	2. Generate documentation for the trimmed spec into the `/Docs` folder
	3. Generate documentation for the full spec into the `/Docs` folder
	4. Copy over the spec and some other files from `/static` into the `/Docs` folder

Then, open `/Docs/index.html` to view the changes.

### Analyze difference between Floatplane frontends

The list of APIs was generated from the Floatplane frontend files, available at https://frontend.floatplane.com/{version}/*.js

When a new version of the Floatplane frontend is released (which is done silently), we can analyze the differences between the files to find new endpoints being used. To do so more easily, some tools are included:

1. Clone this repository
2. Change directory into the `/src` folder: `cd /src`
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