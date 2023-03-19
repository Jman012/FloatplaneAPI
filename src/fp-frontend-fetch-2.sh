#!/bin/bash
if [ -z "$1" ]
then
	echo "Usage: ./fp-frontend-fetch.sh <version>"
	exit 1
fi

root="https://frontend.floatplane.com/"
echo Downloading Floatplane frontend version $root$1 ...
paths=("/runtime.js" "/polyfills.js" "/scripts.js" "/main.js")
for path in "${paths[@]}"; do
	echo "$root$1$path"
	wget "$root$1$path" -O "Frontend/$1$path" -q
done

echo "Done fetching frontend files for $1. Run 'prettier --write Frontend' to un-minify the files so they can be diffed, and then './fp-frontend-diff.sh $1 <version2>' to get a quick diff summary."
