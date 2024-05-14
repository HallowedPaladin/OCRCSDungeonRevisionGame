Insructions

Build with .Net 6.0 (Visual Studio and Visual Studio Code do not support .Net 8.0 on MacOS)
NuGet packages are chosen to match with .Net 6.0 as a consequence.

The project is based on the Visual Studio API projct template from and provides RESTful APIs for the Insignia game client.
In development a Swagger UI is generated to assist with testing.


NuGet Packages 
==============
Core project dependencies for API server
- Swashbuckle.AspNetCore v6.2.3
Microsoft Database Entity Framework support
- Microsoft.EntityFrameworkCore v6.0.29
- Microsoft.EntityFrameworkCore.Design v6.0.29
- Microsoft.EntityFrameworkCore.Tools v6.0.29
MySQL Database Support
- MySql.EntityFrameworkCore v6.0.29
- Pomelo.EntityFrameworkCore.MySql v6.0.23
Hashcode generation support:
- BCrypt.Net-Next v4.0.3
Json generation of the Swagger documentation
- Newtonsoft.Json v13.0.3
- Nswag.ApiDescription.Client v14.0.7
JWT (Json Web Tokens) used for securing API calls
- Microsoft.AspNetCore.Authentication.JwtBearer v6.0.21
- System.IdentityModel.Tokens.Jwt v6.32.2

*** Note ***
The versions of the NuGet packages are chosen to work with .Net framework version 6.x.
This is because .Net Framework version 8.0 is not fully supported on MacOS, particulalry by Visual Studio.
In future Visual Studio Code will provide v8.x support but as of writing it's not yet ready.

Open a Terminal (command line)
==============================
Right click on the root of the project in the Solution Explorer pane.
Choose 'Open In Terminal'
This will open a terminal in the root of the project (same location as the .sln file)

Installing DotNet Tools
=======================
*MacOS only
To generate the classes to support DB access use the "dotnet ef" tool.
See: https://learn.microsoft.com/en-us/ef/core/cli/dotnet

From the command line:
> dotnet tool install --global dotnet-ef

2. Additional config steps for Mac users.  Add the following to .bash_profile and/or .zprofile:
        # Add .NET Core SDK tools
        export PATH="$PATH:/Users/tim/.dotnet/tools"

3. Reload the .profile scripts from the /etc and user home directory (or you can just reopen the shell window)
> zsh -l
or
> bash -l

Verify dotnet tools installation:
> dotnet ef
                             _/\__
                       ---==/    \\
                 ___  ___   |.    \|\
                | __|| __|  |  )   \\\
                | _| | _|   \_/ |  //|\\
                |___||_|       /   \\\/\\

        Entity Framework Core .NET Command-line Tools 8.0.3


Generating Database Classes
===========================
To build the EntityFramework classes from the DB...

Generate (aka scaffold) the database classes from the MySQL DB (replace the parameters in chevrons<> with specific values).
> dotnet ef dbcontext Scaffold "server=<localhost>;port=<port>;user=<user>;password=<password>;database=<schemaname>" MySql.EntityFrameworkCore --output-dir Entities -f  --project <projectname> --context InsigniaDBContext --context-dir Contexts

*** Important #1 ***
This needs to be run from the correct folder (the same folder where the Visual Studio .sln file is).

Anotate the generated code to help manage concurrency conflicts (optimistic locking) using the [Timestamp] annotation.  See below
update - this has now ben automated.

*** Important #2 ***
Manual changes such as annotations will be deleted when the classes are regenerated.

*** Important #3 ***
The generated InsigniaDBContext class needs to be edited (insructions are in the generated code)   
    https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations
 - For each class representing a table with optimistic locking
    - Add the ''using'' clause: using System.ComponentModel.DataAnnotations;
    - Add the annotation [Timestamp] to a suitable field (only one field per class, created specifically for this purpose)
 - The field MUST be a TIMESTAMP and MUST have 'ON UPDATE CURRENT_TIMESTAMP' defined on the field in the table

 *** UPDATE ***
 The T4 script AddTimestampAnnotationTemplate.tt can be run (right click -> Tools -> Process T4 Template) and it will add the using clause and the annotation.
 *** Important ***
 This script needs to be run whenever the DB classes are regenerated.


Config the project to use the MySQL DB
======================================
#1 Edit the appsetting.json file and the match Development version.  Add the following before the closing '}' (swap in appropriate values as before).
    "ConnectionStrings": {
        "DefaultConnection": "server=<server>;port=<port>;user=<user>;password=<password>;database=<schemaname>;"
    }

#2 Enable the DBContext in Program.cs
    using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
    // Database
    builder.Services.AddDbContext<InsigniaDBContext>(options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                   new MySqlServerVersion(new Version(8, 0, 36))); // Specify your MySQL server version here
    });

#3 Tidy upthe OnConfiguring method in the InsigniaDBContext class
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{}


Create Web API Controllers
==========================

Right-click on the Controllers folder -> Add -> Controller -> Select "API Controller with actions, using Entity Framework" -> Choose your model class and data context class.

*** MacOS Visual Studio ***
Right-click on the Controllers folder -> Add -> New Scaffolding... -> API Controller with Actions Using Entity Framework

This will generate a controller with basic CRUD operations for your entity.


Open API Tool
=============
Insall the tool for generating the Swagger json file:
> dotnet tool install --global Swashbuckle.AspNetCore.Cli
This should install the same version (6.5.0) as for the Swashbuckle NuGet package used in this poject

Run the tool
> swagger tofile --output Swagger/GameServerSwagger.json> GameServer/bin/Debug/GameServer.dll v1
*** Update ***
This has been automated to be regenerated whenever the project is built.  The following config was added to the end of the
Visual Studio .csproj file.  Note that this places teh file in the Swagger folder so it will be under source control:
  <Target Name="Generate OpenAPI Specification Document" AfterTargets="Build">
    <Exec Command="swagger tofile --output Swagger/$(AssemblyName)Swagger.json $(OutputPath)$(AssemblyName).dll v1" ContinueOnError="true"></Exec>
  </Target>

A 'ConnectedService' has been defined using this Swagger json file.  This can be seen in the solution exlorer.
Whenever the json is changed the Connected Service will rebuild a .cs client file in the 'obj' folder.  This folder is excluded from source control.
The code can also be found by opening the 'OpenAPI - GameServerSwagger' in the Connected Services folder in solution explorer.  Expand the
configuration section and scroll down to the 'View Code' button'.  Click to open the file.

Program.cs has been updated to allow Swagger to take JWT tokens.  On trhe Swagger page there is 'Authorize' with a padlock icon.  This can be used to
input a JWT token to use when a request is sent.  This means it may no longer be necessary to use the Postman testing utility (see next section).

Testing Utilities
=================
Download (https://www.postman.com/downloads/) and install Postman and create a free account.
Postman is used for testing APIs (much like the Swagger interface generated by Visual Studio).  Postman allows you to chnage the HTTP header
so you can see and insert JWT tokens (amongst other things

Postman Team Name: InsigniaWarriors
Join the team here: https://app.getpostman.com/join-team?invite_code=dde04175431a4e9e79037a199002dcc4
Team URL: https://insigniawarriors.postman.co/

TODO
====
#1 The database connection properties are in appsettings.json so the root DB password is exposed.  This needs to be handled in a secret store.