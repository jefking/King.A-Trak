# A-Trak - Synchronize files to the Cloud!

A-Trak is a tool to manage deploying content and resources to the cloud. It can also be used to backup content off of your primary service provider.
Choose to synchronize data in 9 different directions, between Azure, Amazon S3 and Windows Folders.

This is a utility for easily synchronize files with Blob Storage on Azure, S3 on Amazon or Folders.

## Current applications

* For Development teams to enable continuous deployment
* Make use of CDN's in place
* Release static websites
* Backup data on secondary account
* Backup data off of your primary service provider

We have built this to help you with your development and deployment process. Companies use this tool as part of an automated continuous deployment solution from Jenkins/TeamCity etc. to Amazon and Azure.

## Getting Started

* Clone the repository at <code>git clone git@github.com:AgileBusinessCloud/A-Trak.git</code>
* Run the A-Trak.csproj file in Visual Studio
* Configure the command line arguments in DEBUG tab of Project Properties
  * Example Folder to Azure Blob: <code>/From "C:\Project\MyMVCWebSite\FromFolder" /To "UseDevelopmentStorage=true" "tocontainer"</code>
  * Example Folder to Amazon S3: <code>/From "C:\Project\MyMVCWebSite\FromFolder" /To "SecretKey" "SecretAccessKey" "tobucket"</code>
  * Example Folder to Folder: <code>/From "C:\Project\MyMVCWebSite\FromFolder" /To "C:\Project\MyMVCWebSite\ToFolder"</code>
  * Example Azure Blob to Folder: <code>/From "UseDevelopmentStorage=true" "fromcontainer" /To "C:\Project\MyMVCWebSite\ToFolder"</code>
  * Example Azure Blob to Azure Blob: <code>/From "UseDevelopmentStorage=true" "fromcontainer" /To "UseDevelopmentStorage=true" "tocontainer"</code>
  * Example Azure Blob to Amazon S3: <code>/From "UseDevelopmentStorage=true" "fromcontainer" /To "SecretKey" "SecretAccessKey" "tobucket"</code>
  * Example Amazon S3 to Folder: <code>/From "SecretKey" "SecretAccessKey" "frombucket" /To "C:\Project\MyMVCWebSite\ToFolder"</code>
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

## Contributing

Contributions are always welcome. If you have find any issues with A-Trak, please report them to the [Github Issues Tracker](https://github.com/jefkingabc/A-Trak/issues?sort=created&direction=desc&state=open).

## Apache 2.0 Licence

Copyright 2014 Jef King

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

## About the Author

Jef King has worked in the software industry for fourteen years. Over this time he has experienced a range of responsibilities in various industries. His passion for technology and motivating teams has kept his drive and focus strong. Early on in his career he showed an entrepreneurial spirit, starting multiple small companies. He departed from this to learn more about the software industry by working with larger companies, such as Microsoft. These diverse experiences have given a very unique perspective on teams and software engineering. Since moving back to Vancouver he has built several highly productive software development teams, and inspired others to try similar techniques.