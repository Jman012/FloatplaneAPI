#!/bin/bash
if [ -z "$1" ]
then
	echo "Usage: ./fp-frontend-fetch.sh <version>"
	exit 1
fi

root="https://frontend.floatplane.com/"
echo Downloading Floatplane frontend version $root$1 ...
mkdir -p Frontend/$1/assets/js/video/plugin
paths=("/app.js" "/vendor.js" "/assets/js/dependencies.js" "/assets/js/video/shaka.mint.js" "/assets/js/video/video.min.js" "/assets/js/video/plugin/chromecast.min.js" "/assets/js/video/plugin/contrib-hls.mint.js" "/assets/js/video/plugin/contrib-shaka.es6.js" "/assets/js/video/plugin/hotkeys.min.js" "/assets/js/video/plugin/resolution-switcher.min.js" "/assets/js/video/plugin/thumbnails.js")
for path in "${paths[@]}"; do
	echo "$root$1$path"
	wget "$root$1$path" -O "Frontend/$1$path" -q
done

echo "Done fetching frontend files for $1. Run 'prettier --write Frontend' to un-minify the files so they can be diffed, and then './fp-frontend-diff.sh $1 <version2>' to get a quick diff summary."
