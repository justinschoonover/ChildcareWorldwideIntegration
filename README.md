# Childcare Worldwide Integration
Syncs data between the Denari Online API (DRAPI) and HubSpot

## Building & Deploying
![.NET Core](https://github.com/justinschoonover/ChildcareWorldwideIntegration/workflows/.NET%20Core/badge.svg)
```
gcloud builds submit
```

## Cloud Pub/Sub One Time Setup
```
# Enable Pub/Sub to create authentication tokens in your project
gcloud projects add-iam-policy-binding original-wonder-PROJECT_ID `
     --member=serviceAccount:service-PROJECT-NUMBER@gcp-sa-pubsub.iam.gserviceaccount.com `
     --role=roles/iam.serviceAccountTokenCreator

# Create a service account to represent the Pub/Sub subscription identity
gcloud iam service-accounts create cloud-run-pubsub-invoker `
     --display-name "Cloud Run Pub/Sub Invoker"
```

Permissions setup (required any time the ccw-integration-subscriber service is deleted)
```
# Give the invoker service permission to invoke the ccw-integration-subscriber service
gcloud run services add-iam-policy-binding ccw-integration-subscriber `
   --member=serviceAccount:cloud-run-pubsub-invoker@PROJECT_ID.iam.gserviceaccount.com `
   --role=roles/run.invoker
```

## Cloud Pub/Sub Topic & Subscription Setup
```
gcloud pubsub topics create TOPIC

gcloud pubsub subscriptions create TOPIC-subscription --topic [TOPIC] `
   --push-endpoint=SERVICE-URL/ `
   --push-auth-service-account=cloud-run-pubsub-invoker@PROJECT_ID.iam.gserviceaccount.com
```
