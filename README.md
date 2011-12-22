# A-Track -- Blob Uploader Tool

This is a utility for easily uploading files (CSS/JS) to Blob Storage on Windows Azure. 

It is meant to allow developers to easily leverage Windows Azure's CDN services.

## Getting Started

* Clone the repository at <code>git@github.com:AgileBusinessCloud/A-Track.git</code>
* Run the csproj file in Visual Studio
* Configure the command line arguments in DEBUG tab of Project Properties
** Example: <code>"C:\Project\MyMVCWebSite\Content" "content" "UseDevelopmentStorage=true"</code>
** first argument: location of files you wish to push to Azure on your local environment
** second argument: name of the container you want to push to on Azure
** third argument: connection string
* Run that puppy (Hit F5)!

### Screenshot of configuration arguments

![A-Track Configuration](https://github.com/AgileBusinessCloud/A-Track/raw/master/Config.PNG)

## About the Author

Jef King has worked in the software industry for twelve years. Over this time he has experienced a range of responsibilities in various industries. His passion for technology and motivating teams has kept his drive and focus strong. Early on in his career he showed an entrepreneurial spirit, starting multiple small companies. He departed from this to learn more about the software industry by working with larger companies, such as Microsoft. These diverse experiences have given a very unique perspective on teams and software engineering. Since moving back to Vancouver he has built several highly productive software development teams, and inspired others to try similar techniques.

## Apache 2.0 Licence

   Copyright 2011 Agile Business Cloud

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.