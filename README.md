# Coding-Challenge

This repo contain couple of projects that provide the solution for the coding challenge.
In order to review and test the solution, please follow the instructions below.

The solution was written on dotnet core version 2.1.701, IDE used Visual Studio Code version 1.37 and workstation used.

      ProductName:	Mac OS X
      ProductVersion:	10.14.6
      BuildVersion:	18G87 

Before you start please make sure the workstation has git client and at least dotnet core version 2.1.701 installed.

Also please perform the following task using command prompt.

1. Create a folder on your local workstation.
2. Go to the newly created folder and clone the repo (git clone https://github.com/syedtakbar/Coding-Challenge.git)
3. Make sure there are two folders under Coding-Challeng folder, maze-api and maze-client.
4. Go to maze-api using cd command and execute dotnet run
5. Please make sure the maze-api server is running... it should start listening on: http://localhost:8080
6. Now open up a new command prompt
7. Go to the maze-client folder using cd command and execute dotnet run
8. Please make sure the command screen showing the solved solution correctly.
9. All challenge files are located on maze-client folder, please feel free to change those files and re-run maze-client by executing dotnet run

You may capture stdout from maze-client by executing command dotnet run | tee output.file


