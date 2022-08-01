#!/bin/sh

$(npm bin)/openapi-generator-cli generate \
 -i ../floatplane-openapi-specification-trimmed.json \
 -o FloatplaneAPIClientCSharp \
 -g csharp-netcore \
 --library restsharp \
 --global-property=apiDocs=false,modelDocs=false,apiTests=false,modelTests=false \
 --additional-properties=packageName=FloatplaneAPIClientCSharp,caseInsensitiveResponseHeaders=true,disallowAdditionalPropertiesIfNotPresent=true,nullableReferenceTypes=true,optionalEmitDefaultValues=true,useDateTimeOffset=true,targetFramework=net6.0,packageGuid={693B7B08-9886-48B1-9769-20C819FECBFE}

cp ExtendedApiClient.cs FloatplaneAPIClientCSharp/src/FloatplaneAPIClientCSharp/Client/ExtendedApiClient.cs
