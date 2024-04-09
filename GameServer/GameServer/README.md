Insructions

Build with .Net 7.0 (Visual Studio and Visual Studio Code do not support .Net 8.0 on MacOS)
NuGet packages are chosen to match with .Net 7.0 as a consequence.

The project is based on the Visual Studio API projct template from and provides RESTful APIs for the Insignia game client.
In development a Swagger UI is generated to assist with testing.


NuGet Packages
==============
Core project dependencies for API server
- Microsoft.AspNetCore.OpenApi v7.0.17
- Swashbuckle.AspNetCore
Microsoft Database Entity Framework support
- Microsoft.EntityFrameworkCore v7.0.17
- Microsoft.EntityFrameworkCore.Design v7.0.17
- Microsoft.EntityFrameworkCore.Tools v7.0.17
MySQL Database Support
- MySql.EntityFrameworkCore v7.0.14
Hashcode generation support:
- BCrypt.Net.Next v4.0.3


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
*** Important #1 ***
This needs to be run from the correct folder (the same folder where the Visual Studio .sln file is).
> dotnet ef dbcontext Scaffold "server=<localhost>;port=<port>;user=<user>;password=<password>;database=<schemaname>" MySql.EntityFrameworkCore --output-dir Entities -f  --project <projectname> --context InsigniaDBContext --context-dir Contexts
 
Anotate the generate code to help manage concurrency conflicts (optimistic locking) using the [Timestamp] annotation.
*** Important #2 ***
Manual changes such as annotations will be deleted when the classes are regenerated.
*** Important #3 ***
The generated InsigniaDBContext class needs to be edited (insructions are in the generated code)   
    https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations
 - For each class representing a table with optimistic locking
    - Add the ''using'' clause: using System.ComponentModel.DataAnnotations;
    - Add the annotation [Timestamp] to a suitable field (only one field per class, created specifically for this purpose)
 - The field MUST be a TIMESTAMP and MUST have 'ON UPDATE CURRENT_TIMESTAMP' defined on the field in the table

 UPDATE:
 The T4 script AddTimestampAnnotationTemplate.tt can be run (right click -> Tools -> Process T4 Template) and it will add the using clause and the annotation.
 This script shuld be run whenever the DB classes are regenerated.


Config the project to use the DB
================================
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
                   new MySqlServerVersion(new Version(8, 3, 0))); // Specify your MySQL server version here
    });

#3 Tidy upthe OnConfiguring method in the DBConext class
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{}


API Enabling the DB Classes
===========================

Create Web API Controllers:

Right-click on the Controllers folder -> Add -> Controller -> Select "API Controller with actions, using Entity Framework" -> Choose your model class and data context class.

*** MacOS Visual Studio ***
Right-click on the Controllers folder -> Add -> New Scaffolding... -> API Controller with Actions Using Entity Framework

This will generate a controller with basic CRUD operations for your entity.


Errors

Running the project
"Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found or is out of date.
To generate a developer certificate run 'dotnet dev-certs https'.
To trust the certificate (Windows and macOS only) run
'dotnet dev-certs https --trust'.\nFor more information on configuring HTTPS see https://go.microsoft.com/fwlink/?linkid=848054."


Generating Controllers for the DB tables
No database provider has been configured for this DbContext.
A provider can be configured by overriding the 'DbContext.OnConfiguring' method or by using 'AddDbContext' on the application service provider.
If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object in its constructor and passes it to the base constructor for DbContext.


