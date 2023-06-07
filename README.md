# BKB-EduSimplified

This project was generated with **_Microsoft Visual Studio version 17.4._**

This project provides the backend for **BKB-EduSimplified** project.

**Frontend** for this project has been developed by **Sakshi Agrawal** using Angular and can be found at below repository:
https://github.com/sakshiAgrawal99/eduSimplified

# About this project: 
The name of this project is **BKB-EduSimplified** and as the name suggests this application aims 
to simplify the education processes not only for students but also for teachers.

It provides a web platform for teachers to interact seamlessly with students and provide them with adequate study 
materials and syllabus based on courses easily. Also, the application provides teachers the ability 
to make important announcements directly through the web app. Teachers can upload/delete 
timetable for the various courses through the application. The web app also enables teachers to see 
all the students for a particular course and send them messages directly one-to-one.

From the student’s perspective, this web app can be very helpful as a one stop for all their academic 
requirements. Students can get all the study materials for their courses based on subject at one 
place, so they don’t have to search at the end moments for study, saving time and efforts. Also, 
they can directly view all the announcements made by Teachers/admin on the portal. Students can 
see the messages received from teachers and take action on them immediately.

# Database
We used **SQL Server Studio** as the database for this project and **Azure Cloud Blob Storage** for Object storage.
To run this project in local, create the database as per below schema in SQL Server Studio.

![image](https://github.com/Khilna1110/BKBCollegeManagement/assets/132914500/97209a45-c39d-4401-b6a6-a234dcc97f0b)

**Provide the reference for this database in appsettings.json file in C# as shown below**:

"**ConnectionStrings**": {

    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database={yourDBname};Trusted_Connection=True;MultipleActiveResultSets=true"
    
  }
  
# Azure Blob Storage
We used **Azure Blob Storage** for storage of Timetables and Study Materials for courses. We need to create Azure Container in Storage account using Azure Cloud account and update the below details in appsettings.json as well as API calls for download/upload timetables and study materials.

 "**BlobConnectionString**":
 "connection string for your Azure container",
 
 "**BlobContainerName**":
 "your azure cloud container",
 
 "**AccountName**":
 "your azure account name",
 
 "**AccessKey**": 
 "your azure access key"

