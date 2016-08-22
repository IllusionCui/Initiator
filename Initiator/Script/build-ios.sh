#!/bin/sh

BASE=$(pwd)
PROJ_DIR=${BASE}/../Build/Initiator
RELEASE_DIR=${BASE}/../Build
PROJECT_NAME=Unity-iPhone
TARGET_SDK="iphoneos"
PROJECT_BUILDDIR="${PROJ_DIR}/build/Release-iphoneos"
PRODUCT_NAME="Initiator"
OUTPUT_NAME="Initiator"
BUNDLE_VERSION=`/usr/libexec/PlistBuddy -c "Print :CFBundleVersion" ${PROJ_DIR}/Info.plist`
PROVISIONING_PROFILE="${BASE}/ios-distribution.mobileprovision"
CERTIFICATE="iPhone Distribution: Ye Da (4596K4KGM6)"

echo Bundle Version $BUNDLE_VERSION
sleep 2

build_project () {
	macros=$1
	
	if [ "$macros" != "" ]; then
		macros=" ${macros}"
	fi
	
	# compile project
	echo Building Project
	cd "${PROJ_DIR}"
	xcodebuild -target "${PROJECT_NAME}" -sdk "${TARGET_SDK}" -configuration Release GCC_PREPROCESSOR_DEFINITIONS="\${inherited}${macros}" CODE_SIGN_IDENTITY="${CERTIFICATE}" CODE_SIGNING_REQUIRED=YES
	#Check if build succeeded
	if [ $? != 0 ]
	then
	  echo "Build failed!"
	  exit 1
	fi

	# Sign
	cd /tmp
	rm -rf Payload/
	mkdir -p Payload
	cp -r "${PROJECT_BUILDDIR}/${PRODUCT_NAME}.app" Payload/

	echo Signing, getting entitlements...
	codesign -d --entitlements - Payload/${PRODUCT_NAME}.app > entitlements.plist

	# replace the provision
	echo Copying new provisioning profile...
	cp "$PROVISIONING_PROFILE" Payload/${PRODUCT_NAME}.app/embedded.mobileprovision

	# sign with the new certificate (--resource-rules has been deprecated OS X Yosemite (10.10), it can safely be removed)
	echo Signing...
	/usr/bin/codesign -f -s "$CERTIFICATE" --entitlements entitlements.plist Payload/${PRODUCT_NAME}.app

	# zip it back up
	echo Packing...
	rm -f "${OUTPUT_NAME}.ipa"
	zip -qr "${OUTPUT_NAME}.ipa" Payload
	mv "${OUTPUT_NAME}.ipa" "${RELEASE_DIR}/"
	cd - &> /dev/null
	
	# not using symbols now
	#cd "${PROJECT_BUILDDIR}"
	#zip -r "${OUTPUT_NAME}.app.dSYM.zip" "${OUTPUT_NAME}.app.dSYM"
	#mv "${OUTPUT_NAME}.app.dSYM.zip" "${RELEASE_DIR}"
	#cd - &> /dev/null
}

# build
build_project


