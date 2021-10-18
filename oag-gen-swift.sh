rm -rf Swift/*
openapi-generator generate\
	-i floatplane-openapi-specification.json\
	-o Swift\
	-g swift5\
	--library vapor\
	--global-property models,supportingFiles,apis=Activation:Auth:CDN:Comment:ConnectedAccounts:Content:Creator:CreatorSubscriptionPlan:Edges:FAQ:Iframe:Livestream:LoyaltyRewards:Playlist:Socket:Subscriptions:Sync:User:WebNotification
