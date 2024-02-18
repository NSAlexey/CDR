# CDR
The Brief 
Our product team would like you to develop a new call detail record (CDR) business intelligence platform. The idea behind it is that the call records can be queried to find information such as average call cost, longest calls,  average number of calls in a given time period. 
For this technical test we would like you to create a CDR API. This means that a front-end user interface is not required, neither is any consideration of any services which you might expect to be shared such as authentication. A CDR will be provided as a comma separated value file; these files are delivered as often as daily and can be very large (gigabytes), so being able to simply upload new files to database is a must. Between 5 to 8 endpoints demonstrating a variety of insights (i.e. not all averages, max, mins), in addition to the CDR upload will be sufficient.
We’d like you to consider what API methods would be useful to other teams writing calling code, how can you make the API easy to use, is each method doing what someone else would expect it to do? Your design should have testing and maintainability at its core, and you should consider what testing is appropriate.
The expectation from the product team is that we produce a fully working system as soon as possible, then continue to add features. We hope that you will think about this expectation as you work. 

1. This solution is based on REST API technology. Project is Web API .Net Core. CsvHelper is used to parse csv files. DB in memory is used to except installing SQL Server.
2. Current solution uses DB in memory, as a result not all LINQ operations are available. For example, GroupBy and OrderBy
3. - More unit test is needed to cover different cases with data in files and to cover another methods. 
   - Optimisation of finding previous imported data: add index to table on MS SQL server or separate journal with imported files
   - additional statistics endpoints