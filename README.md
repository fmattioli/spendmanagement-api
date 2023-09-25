# spendmanagement-api
Using concepts and technologies such as Microservices, CQRS, Event Sourcing, Repository Pattern, Clean Arch, MongoDB, SQLServer, and Kafka. The main objective related to this project is to provide an API where you can add spending and manage it in a microservice context.

# Related projects
> https://github.com/fmattioli/spendmanagement-apigateway <br/>
> https://github.com/fmattioli/spendmanagement-domain <br/>
> https://github.com/fmattioli/spendmanagement-eventhandler <br/>
> https://github.com/fmattioli/spendmanagement-readmodel <br/>
> https://github.com/fmattioli/spendmanagement-identity <br/>


# Related packages
> https://github.com/fmattioli/spendmanagement-contracts <br/>
> https://github.com/fmattioli/spendmanagement-topics

# Architecture diagram
![Alt text](src/SpendManagementDiagramFlow.jpg?raw=true "Title")

# How to make it work on your machine?
The entire project has docker support.
It's necessary to run the docker-composes in a defined order.
To start is necessary to run the docker-compose that will create the dependencies that are used by projects inside the SpendManagament ecosystem.
> https://github.com/fmattioli/spendmanagement-common-containers/blob/main/docker-compose.yml

After that, is necessary to run the docker-composes from the SpendManagament ecosystem.
The first one is SpendManagament.EventHandler. This project has the purpose of bringing events from a Kafka topic and saving it on a database NoSQL.
> https://github.com/fmattioli/spendmanagement-eventhandler/blob/main/docker-compose.yml

The second project that is necessary to run the docker-compose is SpendManagament.Domain. This project has the purpose of receiving commands on a Kafka topic and converting them into domain events. 
This project also stored these commands and events in a database, respecting the event sourcing pattern.
> https://github.com/fmattioli/spendmanagement-domain/blob/main/docker-compose.yml

The third project that is necessary to run the docker-compose is the SpendManagament.Identity. This project has the purpose to generate JWT tokens and manage the users who access and use the SpendManagament project.
>https://github.com/fmattioli/spendmanagement-identity/blob/main/docker-compose.yml

The third project that is necessary to run the docker-compose is the SpendManagament.Identity. This project has the purpose to generate JWT tokens and manage the users who access and use the SpendManagament project.
> https://github.com/fmattioli/spendmanagement-identity/blob/main/docker-compose.yml

The fourth project that is necessary to run the docker-compose is the SpendManagament.ReadModel. This project has the purpose of being a ReadModel related to the SpendManagament context.
This project read data from a NoSQL database. 
> https://github.com/fmattioli/spendmanagement-readmodel/blob/main/docker-compose.yml

The fifth project that is necessary to run the docker-compose is the SpendManagament.Api. This project has the purpose of producing commands related to the SpendManagement context.
> https://github.com/fmattioli/spendmanagement-api/blob/main/docker-compose.yml

To finish, the SpendManagament project has an apigateway. This apigateway has references to all APIs inside the SpendManagamnet ecosystem. 
> https://github.com/fmattioli/spendmanagement-apigateway/blob/main/docker-compose.yml

