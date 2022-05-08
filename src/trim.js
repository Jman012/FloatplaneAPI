const fs = require("fs");

// Nothing to trim for either Frontend nor Chat AsyncAPI files.

fs.readFile("../floatplane-openapi-specification.json", "utf8", (err, data) => {
	if (err) {
		console.log("Could not read ../floatplane-openapi-specification.json");
	} else {
		const spec = JSON.parse(data);

		for (var path in spec.paths) {
			for (var method in spec.paths[path]) {
				if (spec.paths[path][method].description.indexOf("TODO") == 0) {
					console.log("Removed: " + path + " " + method);
					delete spec.paths[path][method];
				}
			}
		}

		for (var path in spec.paths) {
			if (Object.keys(spec.paths[path]).length == 0) {
				delete spec.paths[path];
			}
		}

		var tagsToRemove = [];
		for (var tag in spec.tags) {
			if (!Object.keys(spec.paths).flatMap(path => Object.keys(spec.paths[path]).map(operation => spec.paths[path][operation])).some(op => op.tags.some(t => t == spec.tags[tag].name))) {
				console.log("Removing tag " + spec.tags[tag].name);
				tagsToRemove.push(spec.tags[tag].name);
			}
		}
		for (var tagToRemove of tagsToRemove) {
			spec.tags = spec.tags.filter(tag => tag.name != tagToRemove);
		}

		fs.writeFile("../floatplane-openapi-specification-trimmed.json", JSON.stringify(spec, null, 4), "utf8", (err) => {
			if (err) {
				console.log("Cound not write to ../floatplane-openapi-specification-trimmed.json");
			} else {
				console.log("Done");
			}
		});
	}
});
