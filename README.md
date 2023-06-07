# BKB-EduSimplified

This project was generated with **_Microsoft Visual Studio version 17.4._**

This project provides the backend for **BKB-EduSimplified** project.

**Frontend** for this project has been developed by **Sakshi Agrawal** and can be found at below repository:
https://github.com/sakshiAgrawal99/eduSimplified

# Database
We used **SQL Server Studio** as the database for this project and **Azure Cloud Blob Storage** for Object storage.
To run this project in local, create the database as per below schema in SQL Server Studio.

![image](https://github.com/Khilna1110/BKBCollegeManagement/assets/132914500/9cc8d06e-61a4-4f61-b9e0-6b0b02ece34d)

**Provide the reference for this database in appsettings.json file in C# as shown below**:

"ConnectionStrings": {

    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database={yourDBname};Trusted_Connection=True;MultipleActiveResultSets=true"
    
  }
  
# Azure Blob Storage
We used **Azure Blob Storage** 
