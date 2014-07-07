A-Trak is a tool to manage deploying content and resources to the cloud. It can also be used to backup content off of your primary service provider.
Choose to synchronize data in 9 different directions, between Azure, Amazon and Windows.

This is a utility to synchronize files between Azure, Amazon and Windows.

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.ATrak)
```
PM> Install-Package King.ATrak
```

## Current applications
* For Development teams to enable continuous deployment
* Make use of CDN's in place
* Release static websites
* Backup data on secondary account
* Backup data off of your primary service provider

We have built this to help you with your development and deployment process. Companies use this tool as part of an automated continuous deployment solution from Jenkins/TeamCity etc. to Amazon and Azure.

## Getting Started
* Clone the repository
* Run the A-Trak.csproj file in Visual Studio
* Configure the command line arguments in DEBUG tab of Project Properties
  * Example Folder to Azure Blob: <code>/From "C:\\FromFolder" /To "UseDevelopmentStorage=true" "tocontainer"</code>
  * Example Folder to Amazon S3: <code>/From "C:\\FromFolder" /To "SecretKey" "SecretAccessKey" "tobucket"</code>
  * Example Folder to Folder: <code>/From "C:\FromFolder" /To "C:\ToFolder"</code>
  * Example Azure Blob to Folder: <code>/From "UseDevelopmentStorage=true" "fromcontainer" /To "C:\ToFolder"</code>
  * Example Azure Blob to Azure Blob: <code>/From "UseDevelopmentStorage=true" "fromcontainer" /To "UseDevelopmentStorage=true" "tocontainer"</code>
  * Example Azure Blob to Amazon S3: <code>/From "UseDevelopmentStorage=true" "fromcontainer" /To "SecretKey" "SecretAccessKey" "tobucket"</code>
  * Example Amazon S3 to Folder: <code>/From "SecretKey" "SecretAccessKey" "frombucket" /To "C:\ToFolder"</code>
  * Example Amazon S3 to Azure Blob: <code>/From "SecretKey" "SecretAccessKey" "frombucket" /To  "UseDevelopmentStorage=true" "tocontainer"</code>
  * Example Amazon S3 to Amazon S3: <code>/From "SecretKey" "SecretAccessKey" "frombucket" /To "SecretKey" "SecretAccessKey" "tobucket"</code>
* Run that puppy (Hit F5)!

## Configuration
* From
  * Specify From in configuration file, if you are not dynamically passing in the From argument
* To
  * Specify To in configuration file, if you are not dynamically passing in the To argument
* CreateSnapShot
  * Specify CreateSnapShot if you want to turn off Azure Blob SnapShot ability (values: true, false)
* Synchronize
  * Will delete files out of destination if they do not exist in source; acts like a purge (values: true, false)
* CacheControl
  * Specify the cache-control header in the HTTP Response; great for integration with Azure CDN.