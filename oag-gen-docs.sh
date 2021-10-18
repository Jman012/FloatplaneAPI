rm -rf Docs/*
openapi-generator generate\
	-i floatplane-openapi-specification.json\
	-o Docs\
	-g html2\
	--global-property models,supportingFiles,apis=Activation:Auth:CDN:Comment:ConnectedAccounts:Content:Creator:CreatorSubscriptionPlan:Edges:FAQ:Iframe:Livestream:LoyaltyRewards:Playlist:Socket:Subscriptions:Sync:User:WebNotification
