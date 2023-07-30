# this tells Make to run 'make help' if the user runs 'make'
# without this, Make would use the first target as the default
.DEFAULT_GOAL := help

# here we have a simple way of outputting documentation
# the @-sign tells Make to not output the command before running it
help:
	@echo 'Available commands:'
	@echo "clean validate test testdocs-all"
	@echo "docs-trimmed docs-full docs-async"

# Section: Helpers and structural

clean:
	rm -rf ./Docs
	rm -f src/floatplane-openapi-specification-trimmed.json
trim:
	node tools/trim.js src/floatplane-openapi-specification.json src/floatplane-openapi-specification-trimmed.json
validate:
	npx openapi-generator-cli validate -i src/floatplane-openapi-specification.json
validate-trimmed: trim
	npx openapi-generator-cli validate -i src/floatplane-openapi-specification-trimmed.json
docs-skeleton:
	mkdir -p Docs
	cp ./static/index.html ./Docs/index.html
yaml: validate validate-trimmed docs-skeleton
	npx json2yaml src/floatplane-openapi-specification.json > Docs/floatplane-openapi-specification.yaml
	npx json2yaml src/floatplane-openapi-specification-trimmed.json > Docs/floatplane-openapi-specification-trimmed.yaml
	npx openapi-generator-cli validate -i Docs/floatplane-openapi-specification.yaml
	npx openapi-generator-cli validate -i Docs/floatplane-openapi-specification-trimmed.yaml
test: validate trim
	cd tests/SchemaThesisTests; poetry run pytest -v -s

# Section: Trimmed docs

docs-oag-html2: docs-skeleton validate-trimmed
	npx openapi-generator-cli generate -i src/floatplane-openapi-specification-trimmed.json -o Docs/OAG-html2/ -g html2
docs-oag-dynamic-html: docs-skeleton validate-trimmed
	npx openapi-generator-cli generate -i src/floatplane-openapi-specification-trimmed.json -o Docs/OAG-dynamic-html/ -g dynamic-html
docs-redoc: docs-skeleton validate-trimmed
	mkdir -p Docs/Redoc
	npx redoc-cli bundle -o Docs/Redoc/redoc-static.html src/floatplane-openapi-specification-trimmed.json
docs-rapidoc: docs-skeleton validate-trimmed
	mkdir -p Docs/Rapidoc
	cp ./static/rapidoc.html ./Docs/Rapidoc/rapidoc.html
docs-reslate: docs-skeleton validate-trimmed
	mkdir -p Docs/ReSlateDocs
	cd Docs/ReSlateDocs; npx reslate init Docs/ReSlateDocs
	npx widdershins src/floatplane-openapi-specification-trimmed.json -o Docs/ReSlateDocs/index.md
	cd Docs/ReSlateDocs; npx reslate build
	rm -rf Docs/ReSlateDocs/site
	mv Docs/ReSlateDocs/_site Docs/ReSlateDocs/site
docs-postman: docs-skeleton validate-trimmed
	mkdir -p Docs/Postman-v2.1
	npx openapi2postmanv2 --spec src/floatplane-openapi-specification-trimmed.json --output Docs/Postman-v2.1/floatplaneapi-postman2.1-collection.json --pretty -O folderStrategy=Tags,includeAuthInfoInExample=false
docs-swaggerui: docs-skeleton validate-trimmed
	mkdir -p Docs/SwaggerUI
	cp static/SwaggerUI/* Docs/SwaggerUI
	sed -i.bak -e 's/floatplane-openapi-specification.json/floatplane-openapi-specification-trimmed.json/g' Docs/SwaggerUI/index.html
docs-trimmed: docs-oag-html2 docs-oag-dynamic-html docs-redoc docs-rapidoc docs-reslate docs-postman docs-swaggerui
	@echo "docs-trimmed complete!"

# Section: Full docs

docs-oag-html2-full: docs-skeleton
	npx openapi-generator-cli generate -i src/floatplane-openapi-specification.json -o Docs/OAG-html2-full/ -g html2
docs-oag-dynamic-html-full:
	npx openapi-generator-cli generate -i src/floatplane-openapi-specification.json -o Docs/OAG-dynamic-html-full/ -g dynamic-html
docs-redoc-full: docs-skeleton
	mkdir -p Docs/Redoc-full
	npx redoc-cli bundle -o Docs/Redoc-full/redoc-static.html src/floatplane-openapi-specification.json
docs-rapidoc-full: docs-skeleton
	mkdir -p Docs/Rapidoc-full
	cp ./static/rapidoc-full.html ./Docs/Rapidoc-full/rapidoc-full.html
docs-reslate-full: docs-skeleton
	mkdir -p Docs/ReSlateDocs-full
	cd Docs/ReSlateDocs-full; npx reslate init Docs/ReSlateDocs-full
	npx widdershins src/floatplane-openapi-specification.json -o Docs/ReSlateDocs-full/index.md
	cd Docs/ReSlateDocs-full; npx reslate build
	rm -rf Docs/ReSlateDocs-full/site
	mv Docs/ReSlateDocs-full/_site Docs/ReSlateDocs-full/site
docs-postman-full: docs-skeleton
	mkdir -p Docs/Postman-v2.1-full
	npx openapi2postmanv2 --spec src/floatplane-openapi-specification.json --output Docs/Postman-v2.1-full/floatplaneapi-postman2.1-collection.json --pretty -O folderStrategy=Tags,includeAuthInfoInExample=false
docs-swaggerui-full: docs-skeleton
	mkdir -p Docs/SwaggerUI-full
	cp static/SwaggerUI/* Docs/SwaggerUI-full
docs-full: validate docs-oag-html2-full docs-oag-dynamic-html-full docs-redoc-full docs-rapidoc-full docs-reslate-full docs-postman-full docs-swaggerui-full
	@echo "docs-full complete!"

# Section: Async docs

docs-async-frontend: docs-skeleton
	npx ag -o Docs/AsyncAPIFrontend src/floatplane-asyncapi-frontend-specification.json @asyncapi/html-template
docs-async-chat:
	npx ag -o Docs/AsyncAPIChat src/floatplane-asyncapi-chat-specification.json @asyncapi/html-template
docs-async: docs-async-frontend docs-async-chat
	@echo "docs-async complete!"

# Section: Final docs

docs-static: trim docs-skeleton
	cp ./src/floatplane-openapi-specification.json ./src/floatplane-openapi-specification-trimmed.json Docs/
	cp ./src/floatplane-asyncapi-frontend-specification.json Docs/
	cp ./src/floatplane-asyncapi-chat-specification.json Docs/
docs-all: clean validate docs-skeleton docs-trimmed docs-full docs-async docs-static yaml
	@echo "docs-all complete!"
