# Answers to technical questions

1. How long did you spend on the coding assignment?  
What would you add to your solution if you had more time?  
If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.  

   I spent about **25 hours** on the coding assignment.  
   If I had more time, I would add:   
    - Authentication and Authorization mechanism, but it depends on the usecase:
      * If consumers were out of organizations, I would add dynamic Api Key with a different plan per consumer. 
      * If more complicated poclicy was needed, I would use JWT instead of Api key.
    - A distributed cache like Redis instead of a memory cache
***
2. What was the most useful feature that was added to the latest version of your language of choice?  
Please include a snippet of code that shows how you've used it?  
In my opinion, _**Required member**_ is one of the useful features introduced in C#11. Before this feature, when developers added a new Property or Field to old classes, they must have found every place in the codebase in which the class had been initiated in ordr to set this property. This manual process would cause bugs in case they miss to change all codes. But with this feature, they see compiler errors, and it prevents run-time errors.  

***
3. How would you track down a performance issue in production? Have you ever had to do this?  
Definitly yes. I always take the following steps:
    1. First, I ask about the issue, to know where and when it happened. (I ask these questions, because sometimes after a new publish we face a new performance issue.)
    2. I check Trace and logs(execution time and number of calls)
    3. I trace the codebase, line by line and check the execution time of each line. (I need to know and even sometimes guess which lines of code causes this performance issue.)
    4. If some parts of the codebase are outer Web API or Web service call, I would check them.
    5. Sometimes, in production, the Databases are the point of performance issues, so  I check query execution time, indexes, structure, ORM etc.
    6. Sometimes, we have a kind of deadlock (the general concept of deadlock), and they are related to some mistakes in development and misusage of the locking mechanism.

***
4. What was the latest technical book you have read or tech conference you have been to? What did you learn?  
Recently, I started to read _**Kafka: The Definitive Guide** (Realtime Data and Stream Processing at Scale)_ 2nd edition published by O'REILLY. I have not completed it yet but I highly recommend this book to software engineers who develop applications that use Kafka’s APIs and for DevOps engineers who install, configure, tune, and monitor Kafka in production.  
I study this book because in my current position, our team decided to use Kafka as a stream processor instead of the legacy stream processor which uses Microsoft StreamInsight. Reading this book helped us to learn it properly from scratch and to use it easily.

***
5. What do you think about this technical assessment?  
I really enjoyed doing it. However I didn't have much time, I was passionate about developing it. Besides, it made me to write codes with more precision and to think about each line of code.
I assume every word in this assessment document is part of the problem that I must deal with to solve it with the best practices. Because I believe all developers need to learn to work in situations in which all the criteria are not obvious, and during the coding they will be obvious to them. 
***
6. Please, describe yourself using JSON.  

```json
{ 
  "FirstName": "Samaneh",
  "LastName": "Bahrami", 
  "location": "Tehran-Iran",  
  "JobTitle": ".Net BackEnd Developer",  
  "Educations": [  
    {  
      "Field": "Software Engineering",  
      "Degree": "Master"  
    },  
    {  
      "Field": "Software Engineering",  
      "Degree": "Bachelor"  
    }  
  ],  
  "WorkExperiences": [ 
    {  
      "JobTitle": "Software Analyst",  
      "CompanyName": "Goldiran Logistic",  
      "StartDate": "2016-05-15",  
      "EndDate": "2017-03-10",  
      "Working": false  
    },  
    {  
      "JobTitle": ".Net BackEnd Developer",  
      "CompanyName": "Goldiran Co",  
      "StartDate": "2017-03-10",  
      "EndDate": "2023-05-26",  
      "Working": false  
    },  
    {  
      "JobTitle": "Software Analyst",  
      "CompanyName": "Goldiran Logistic",  
      "StartDate": "2023-05-27",  
      "EndDate": null,  
      "Working": true  
    }  
  ],  
  "Skills": {  
    "ExperiencdIn": [  
      ".Net",  
      "C#",  
      "EF-EF Core",  
      "MS SQl Server",  
      "TSQL",  
      "OOP",  
      "Solid Principles",  
      "Clean Arch",  
      "Web Apis",  
      "Async Programming",  
      "GIT",  
      "SCRUM",  
      "REDIS",  
      "RabbitMQ"  
    ],   
    "DevelopingIn": [  
      "SignalR",  
      "Kafka (Message Queuing- Stream Processing)",  
      "Docker"  
    ]  
  }  
}
 ```  
***
Thank you for providing me with this opportunity.

_Samaneh Bahrami_