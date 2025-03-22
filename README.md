
# Library System

This project to build an application about the manager the book in the library
## Techniques Overview
- Language: C#, HTML, CSS

- Database: SQL Server

- Framework: ASP.Net

- Design: Draw.io, Figma
## Features
- Design databases at logical and physical levels and build database schema.
- Features of application
    - JWT Authentication
    - Display the list of books
    - Features of Admin/Librarian:
        - Create a new user
        - Manage the books of system
        - Manage the users of system
        - Manage the borrowing
        - Receive notifications when users borrow or return books
        - Mangage the payment of borrowing
    - Features of Reader:
        - Register account
        - Display the list books
        - Borrow/Return books
        - Payment the borrowing books
        - Review the books after borrowing
## Configuration
- Step 1:
    - Install the packages of Nuget:
        - AutoMapper.Extensions.Microsoft.DependencyInjection
        - FirebaseStorage.net
        - FirebaseAdmin
        - Firebase.Auth
        - Microsoft.AspNetCore.Authentication.JwtBearer
        - Microsoft.EntityFrameworkCore
        - Microsoft.EntityFrameworkCore.SqlServer
        - Microsoft.EntityFrameworkCore.Tools
    - Connect to SqlServer
    - Add JwtSettings and Firebase configurations to appsettings.json
