#!/bin/bash

if [ -z "$1" ] || [ -z "$2" ]
then
	echo "Usage: ./fp-frontend-diff.sh <version1> <version2>"
	exit 1
fi

before=${1//./\\.}
after=${2//./\\.}
sed -i.bak -e "s/$before/theversion/g" Frontend/$1/main.js
sed -i.bak -e "s/$after/theversion/g" Frontend/$2/main.js

paths=("/runtime.js" "/polyfills.js" "/scripts.js" "/main.js")
for path in "${paths[@]}"; do
    diff -q Frontend/$1$path Frontend/$2$path
done
