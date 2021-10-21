#!/bin/bash

if [ -z "$1" ] || [ -z "$2" ]
then
	echo "Usage: ./fp-frontend-unminify-diff.sh <version1> <version2>"
	exit 1
fi

sed -i.bak -e "s/$1/theversion/g" Frontend/$1/app.js
sed -i.bak -e "s/$2/theversion/g" Frontend/$2/app.js

paths=("/app.js" "/vendor.js" "/assets/js/dependencies.js" "/assets/js/video/shaka.mint.js" "/assets/js/video/video.min.js" "/assets/js/video/plugin/chromecast.min.js" "/assets/js/video/plugin/contrib-hls.mint.js" "/assets/js/video/plugin/contrib-shaka.es6.js" "/assets/js/video/plugin/hotkeys.min.js" "/assets/js/video/plugin/resolution-switcher.min.js" "/assets/js/video/plugin/thumbnails.js")
for path in "${paths[@]}"; do
    diff -q Frontend/$1$path Frontend/$2$path
done
