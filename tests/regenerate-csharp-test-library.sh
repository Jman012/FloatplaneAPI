#!/bin/sh

$(npm bin)/openapi-generator-cli generate \
 -i ../floatplane-openapi-specification-trimmed.json \
 -o FloatplaneAPIClientCSharpTester \
 -g csharp-netcore \
 --library restsharp \
 --global-property=apiDocs=false,modelDocs=false,apiTests=true,modelTests=false \
 --additional-properties=packageName=FloatplaneAPIClientCSharp,caseInsensitiveResponseHeaders=true,disallowAdditionalPropertiesIfNotPresent=true,nullableReferenceTypes=true,optionalEmitDefaultValues=true,useDateTimeOffset=true,targetFramework=net6.0,packageGuid={693B7B08-9886-48B1-9769-20C819FECBFE}

pushd FloatplaneAPIClientCSharpTester
dotnet sln remove src/FloatplaneAPIClientCSharp/FloatplaneAPIClientCSharp.csproj
dotnet sln add ../FloatplaneAPIClientCSharp/src/FloatplaneAPIClientCSharp/FloatplaneAPIClientCSharp.csproj
rm -rf src/FloatplaneAPIClientCSharp
pushd src/FloatplaneAPIClientCSharp.Test
dotnet add reference ../../../FloatplaneAPIClientCSharp/src/FloatplaneAPIClientCSharp/FloatplaneAPIClientCSharp.csproj
popd
popd
