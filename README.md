# NumberGuesser

A simple number guessing game for becoming familiar with C# and MongoDB in a .NET environment.

Steps to use locally:
- Pull the repo and open the project in Visual Studio (I am using Community 2022 v17.5.2). 
- Ensure your Visual Studio has at minimum the 'ASP.NET and web development' Workload installed.
- You should also install MongoDB:
	- https://www.mongodb.com/try/download/community
	- https://www.mongodb.com/try/download/compass (optional)
- In addition, you'll need the Mongo extension for Visual Studio. In the Solution Explorer view:
	- Right click on 'Dependencies' -> 'Manage NuGet Packages...'
	- Search MongoDB.Driver and install
- To visualize your stats, download MongoCompass and connect to your local database at 127.0.0.1:27017
- Run the Program (Ctrl + F5)
