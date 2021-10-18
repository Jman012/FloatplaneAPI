# FloatplaneAPI

An API specification of the content streaming service https://www.floatplane.com represented in the [OpenAPI 3.0.3](https://swagger.io/specification/) specification. The main file for this repository is `floatplane-openapi-specification.json`. Contained in it are definitions for all of the paths in the Floatplane API, definitions of common models between the API endpoints, descriptions of authentication and authorization mechanisms, and more.

This repository serves as an open and central source documentation for the Floatplane API, for purposes of tinkering and creating custom clients for Floatplane. At the time of writing, Floatplane has its main website, along with both Android and iOS applications. The main use case envisioned in the creation of this repository is to make it easier to create TV-first applications for, e.g., tvOS, Roku, Google TV, etc.

## OpenAPI & Generation

Given that the majority of this repository is in the OpenAPI specification, the `floatplane-openapi-specification.json` file can be used to automatically generate documentation, client code, and even server code of the Floatplane API. Various generators exist for different use cases. A notable open-source generator is [OpenAPI Generator](https://openapi-generator.tech/).

### Documentation

You can visit a live render of the documentation for this repository at {TODO}. This is generated using [Redoc](https://redoc.ly/redoc/) from Redocly.

### Code Generation

