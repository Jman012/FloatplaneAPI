const fs = require("fs");

function forObjectProperty(theObject, propertyName, doFunc, path) {
	Object.keys(theObject).forEach(el => {
		if (el == propertyName) {
			doFunc(theObject, path);
		}

		if (typeof theObject[el] === "object") {
			forObjectProperty(theObject[el], propertyName, doFunc, path + "." + el);
		}
	});
}

function difference(setA, setB) {
	const _difference = new Set(setA);
	for (const elem of setB) {
	  _difference.delete(elem);
	}
	return _difference;
  }

fs.readFile("../floatplane-openapi-specification.json", "utf8", (err, data) => {
	if (err) {
		console.log("Could not read ../floatplane-openapi-specification.json");
	} else {
		const spec = JSON.parse(data);

		forObjectProperty(spec.components.schemas, "properties", (schemaObject, path) => {
			let propertyKeys = Object.keys(schemaObject["properties"]);
			let requiredKeys = schemaObject["required"] || [];

			let leftoverKeys = difference(propertyKeys, requiredKeys);
			// console.log("======================");
			// console.log(propertyKeys);
			// console.log(requiredKeys);
			// console.log(leftoverKeys);
			if (leftoverKeys.size > 0) {
				console.log(path, Array.from(leftoverKeys));
			}
		}, "spec.components.schemas");
	}
});

