# CarseerWebAPP

CarseerWebAPP is a .NET Core project containerized by docker and hosted on azure.

## Description

CarseerWebAPP is a web application that gives users the ability to select car make, manufacturing year 
and filter the available vehicle types and models for the selected options.

it is a dockarized .NET Core 5.0 application that uses vpic.nhtsa.dot.gov APIs to consume the relivant data.

## Demo 

https://carseerhiringtask.azurewebsites.net/

## Getting Started

### Dependencies

* .NET Core SDK 5.0
*  ASP.NET Runtume 5.0
*  Any OS (Windows,linux,etc)
*  Docker Desktop Client
*  Visual Code / Visual Studio
*  bitbucket Terminal (bash terminal) / Windows Powershell / vs code terminal

### Installing

Clone the repository in your local machine using git bash terminal or any available CLI, 
make sure to do the following steps by step after you have opened the CLI terminal:-

```

cd desiredLocation
git init
git remote add origin https://github.com/husseinia92/CarseerHiringTask.git
git pull origin master

```

after completing the above steps, the project will be 
installed and the repo will be cloned into your local machine.

### Executing program

* Important Note : ( you must have docker desktop installed, in order to be able to execute the project )

* open visual code CLI terminal and cd to the installed project directory.
 - ex) cd D://Project/CarseerHiringTask
* type the following command to build the project 

- restore all project dependancies and nuget packages and make sure its ready for running.

```
dotnet restore
```

- build the project using the following command :

```
dotnet build
```

- create local docker image using the folling command :

```
docker build --tag carseerwebappimage .
```

- after the build finishes, type the following command that will list all images exists locally, 
  to insure that the image has been created successfully :-

```
docker image ls
```

 - create the container and run the image by typing the following command :-

```
docker run -d -p 8080:80 --name carseerapp carseerwebappimage 
```

- open your browser type localhost:8080 and hit enter, the web application will be opened in your browser!!

## Authors

Contributors names and contact info

Ahmad M. Husseini 
[@husseinia92](https://www.linkedin.com/in/husseinia92/)

## Version History

* 0.1
    * Initial Release

## License

This project is licensed under the MIT License - see the LICENSE.md file for details

