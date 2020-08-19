# Childcare Worldwide Integration
Syncs data between the Denari Online API (DRAPI) and HubSpot

## Building & Deploying
![.NET Core](https://github.com/justinschoonover/ChildcareWorldwideIntegration/workflows/.NET%20Core/badge.svg)
```
gcloud builds submit
```

## One Time Setup Tasks

### Cloud Pub/Sub

```
#### Enable Pub/Sub to create authentication tokens in your project
gcloud projects add-iam-policy-binding PROJECT_ID `
     --member=serviceAccount:service-PROJECT-NUMBER@gcp-sa-pubsub.iam.gserviceaccount.com `
     --role=roles/iam.serviceAccountTokenCreator

#### Create a service account to represent the Pub/Sub subscription identity
gcloud iam service-accounts create cloud-run-pubsub-invoker `
     --display-name "Cloud Run Pub/Sub Invoker"
```

Permissions setup (required any time the ccw-integration-subscriber service is deleted)
```
#### Give the invoker service permission to invoke the ccw-integration-subscriber service
gcloud run services add-iam-policy-binding ccw-integration-subscriber `
   --member=serviceAccount:cloud-run-pubsub-invoker@PROJECT_ID.iam.gserviceaccount.com `
   --role=roles/run.invoker
```

### API Credentials (SSO)

#### OAuth client ID
Navigate to the [Credentials](https://console.cloud.google.com/apis/credentials) page (under APIs & Services) and create a new OAuth client ID. Select the `Web application` type.

Add these credentials to [Secret Manager](https://console.cloud.google.com/security/secret-manager).