# FarmCentral_ST10116983
(PROG7311-TASK2-ST10116983): Published : FarmCentral (ASP .NET CORE MVC) ,  Web API's , Azure SQL Server Database
--------------------------------------------------------------------------------------------------------
PLEASE TAKE NOTE
-----------------
You will require an internet connection,to run the application, which will consume an api to read/write to the database

------
LINKS
Website :
https://st10116983farmcentral.azurewebsites.net/

API : 
https://farmercentralapi.azurewebsites.net/api/
https://farmercentralapi.azurewebsites.net/api/Users
https://farmercentralapi.azurewebsites.net/api/usersproducts
https://farmercentralapi.azurewebsites.net/api/products
https://farmercentralapi.azurewebsites.net/api/producttypes
--------
PURPOSE
--------
The purpose of this application is to help local farmers track their stocks of their products in the most efficient way as possible.
The should also be able to view thier products that they have added. Farmers can only be added to the database if a logged-in employee
adds them and creates their account. The employee can also update and delete farmers products, filter them by product date and time.

INTRODUCTION
------------
This is a Farm Central ASP .NET CORE Model–View–Controller Pattern (MVC) application. 
Its dependency is a C# Web API that performs read/write actions to a published azure sql server database. 
This application fulfils the following requirements/features:

FEATURES -
-----------------

VERSION 1.0.0 - TASK2 : FUNCTIONAL REQUIREMENTS
---------------------------------
 The prototype website shall have a database of farmers with their associated products.
 The prototype website shall make provision for two different user roles: farmer and employee.
 The prototype website shall require farmers and employees to log into the website to access user‐specific information.
 The prototype website shall allow a logged‐in employee to add a new farmer to the database.
 The prototype website shall allow a logged‐in farmer to add a new product to their profile in the database.
 The prototype website shall allow a logged‐in employee to view the list of all the products ever supplied by a specific farmer.
 The prototype website shall allow a logged‐in employee to filter the displayed list of products supplied by a specific farmer according to the date range or type of product.

NON-FUNTIONAL REQUIREMENTS
---------------------------
Use of Language Integrated Query (LINQ) to manipulate the data.
A custom API that is consumed by the MVC web application.
Azure SQL Server database that the API performs read/write/update/delete actions to

INSTALLATION
------------
VISUAL STUDIO :  https://www.guru99.com/download-install-visual-studio.html for the installation guide on how to install visual studio
.NET 5 : https://www.c-sharpcorner.com/article/getting-started-with-net-5-0/ for the installation on Getting Started With .NET 5.0

1. Download the file (FarmCentral_ST10116983.zip) or Clone the repository from GitHub
2. Locate the downloaded file under your downloads in File Explorer. Right click on the file and select Extract All and set the location path to your desktop.
3.  Locate the database script found in the folder (PROG6212-Task2-20103016.zip) and open the file with SQL Server Management Studio or Visual Studio ((PROG6212-Task2-20103016-Script.sql)).
4.  You will then need to run the scripts to create the tables and their columns with dependencies. simply press ctrl+a , and then fn+f5 key to run the scripts.
5.  You will then need to open the solution file (FarmCentral_ST10116983.sln), by double clicking on it.
// IF RUNNING LOCALLY - FOLLOW BELOW
6.  Then you would need to open the solution explorer
6.1 Web API project
6.2 Select and open (Startup.cs)
6.3 Change the connection string in optionsBuilder.UseSqlServer("Server=your-server-name;Database=planner;Trusted_Connection=True;");
6.4 Select and open (Startup.cs)
5.5  services.AddDbContext<FarmCentralContext>(options => options.UseSqlServer("Server=your-server-name;Database=planner;Trusted_Connection=True;"));
7.  You will then need to right click on the solution file and select "Build project"
8.  To run this software open the file and select the FarmCentral_ST10116983.exe (Shortcut).
9.  To debug and compile the software open the FarmCentral_ST10116983 file with Visual Studio and the press press cltr+b to build and then (alt+f5) / (alt+fn+f5) to run the application.

NOTE/IMPLEMENTED FEATURES
---------------------------
Please note you are required to have an internet connection for this application
1. This software allows the user to select a custom start date and end date to filter the products type
2. This software is consuming an API that connects to an Azure SQL server database
3. The Employee can perform CRUD functions to the farmers details/products
5. The user can now register and create their own persoanl account with their private details and their data will only be viewed by themselves
6. The app also implements hashing of passwords that is stored in the database in case of a breack
7. The app also has a feature that allows the farmer to reset their passwords when they login 
FAQ
---
Q. What does this app do?
A. This software helps the farmers keep track of thier stock intake and products supplied.
***
Q. Is this software easy to use?
A. Yes, This application has tool tips that guides the users on what each component does. 
    It also has helpful Message boxes to alert the user when needed.
    It also features validation so all data and logic is correctly stored and displayed.
***
Q. Can i share this software?
A. No, This app is still in Beta developments and cannot be distibuted.
*** 
Q. How does this software benefit my academic studies?
A. This software will extend my knowledge on deployment of MVC applications as well as creating , using and deploying API's as well as Databases
***
BONUS FEATURES
--------------
1. I have implemented the use of Models , Views and View Models (MVC).
2. I have implemented the use of Data Annotations in my Model layers.
3. I have implemented the use of Lambda expression LINQ through out my entire application.
4. I have implemented a password reset feature
5. I have implemented a feature to test where the user has an internet connection or not
6. I have additionally used asyncronours programming with multi-threading 
7. I have implemented the use of multiple class libraries/ nuget packages
8. I have added asthetics to the design of this application with multiple forms of error handling
9. I have implemented the use of J-Query, and CSS (Initiative to self study topics and implemented)
10. I have implemented CRUD fucntions with API's
11. I have published my Web App (MVC) ,  API , and SQL Server Database

CONTACT
---------
Version No. 1.0.0
Release Date 17/06/2022
Author : Maharaj Kiaan
Contact Detail : st10116983@vcconnect.edu.za(email) for any enquires 
