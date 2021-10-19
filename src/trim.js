const fs = require("fs");

fs.readFile("../floatplane-openapi-specification.json", "utf8", (err, data) => {
	if (err) {
		console.log("Could not read ../floatplane-openapi-specification.json");
	} else {
		const spec = JSON.parse(data);

		for (var path in spec.paths) {
			console.log(path + " " + spec.paths[path]);
			for (var method in spec.paths[path]) {
				console.log(path + " " + method);
				if (spec.paths[path][method].description == "TODO") {
					console.log(path + " " + method + " <- Removed");
					delete spec.paths[path][method];
				}
			}
		}

		for (var path in spec.paths) {
			if (Object.keys(spec.paths[path]).length == 0) {
				delete spec.paths[path];
			}
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
