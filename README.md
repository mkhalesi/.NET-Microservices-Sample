# .NET Microservices Sample Project

There is a couple of microservices which implemented e-commerce modules over <b>Payment</b>, <b>Product</b>, <b>Basket</b>, <b>Discount</b>, <b>Ordering</b> and <b>Identity</b> microservices with <b>NoSQL (MongoDB, Redis)</b> and <b>Relational databases (Sql Server)</b> with
communicating over <b>RabbitMQ Event Driven Communication</b> and using <b>Ocelot API Gateway</b>.

<h3>
Deploying .Net Microservices into <h>Kubernetes</b> and moving deployments to the cloud <b>Azure Kubernetes Services (AKS)</b> with using <b>Azure Container Registry (ACR)</b>
</h3>

<hr>

# Whats Including In This Repository

<h4>Identity microservice which includes:</h4>
<ul>
  <li>
    Implement Identity Server - Duenede Server
  </li>
  <li>
    Implement ASP.Net Identity
  </li>
  <li>
    SqlServer database
  </li>
</ul>

<h4>API Gateway Web & Admin Microservice which includes:</h4>
<ul>
  <li>
     Implement API Gateways with Ocelot
  </li>
  <li>
    Implement Authentication with JWT Bearer
  </li>
  <li>
    Sample microservices/containers to reroute through the API Gateways
  </li>
</ul>

<h4>Product Microservice which includes:</h4>
<ul>
  <li>
    REST API principles, CRUD operations
  </li>
  <li>
    Implement Authentication with JWT Bearer
  </li>
  <li>
    Consuming RabbitMQ Messages
  </li>
  <li>
    Swagger Open API implementation
  </li>
  <li>
    SqlServer database
  </li>
</ul>

<h4>Ordering Microservice which includes:</h4>
<ul>
  <li>
    <b>MongoDB database</b> connection and containerization
  </li>
  <li>
    REST API principles, CRUD operations
  </li>
  <li>
    Implement Authentication with JWT Bearer
  </li>
  <li>
    Consuming RabbitMQ Messages
  </li>
  <li>
    Swagger Open API implementation
  </li>
</ul>

<h4>Discount Microservice which includes:</h4>
<ul>
  <li>
    ASP.NET Grpc Server applicatios
  </li>
  <li>
    Exposing Grpc Services with creating Protobuf messages
  </li>
  <li>
    SqlServer database
  </li>
</ul>

<h4>Basket Microservice which includes:</h4>
<ul>
  <li>
    <b>Redis database</b> connection and containerization
  </li>
  <li>
    REST API principles, CRUD operations
  </li>
  <li>
    Implement Authentication with JWT Bearer
  </li>
  <li>
    Consuming RabbitMQ Messages
  </li>
  <li>
    Swagger Open API implementation
  </li>
</ul>

<h4>Payment Microservice which includes:</h4>
<ul>
  <li>
    REST API principles
  </li>
  <li>
    Implement Clean Architecture
  </li>
  <li>
    Implement Authentication with JWT Bearer
  </li>
  <li>
    Consuming RabbitMQ Messages
  </li>
  <li>
    Swagger Open API implementation
  </li>
  <li>
    SqlServer database
  </li>
</ul>

<h4>Microservices Communication :</h4>
<ul>
  <Li>
    Sync inter-service gRPC Communication
  </Li>
  <li>
    Async Microservices Communication with RabbitMQ Message-Broker Service
  </li>
  <li>
    Using RabbitMQ Publish/Subscribe Topic Exchange Model
  </li>
</ul>

<h4>Web & Admin Frontend Microservice :</h4>
<ul>
  <li>
    Implement Authentication with OpenIdConnect
  </li>
  <li>
    Call Ocelot APIs with HttpClientFactory and RestClient
  </li>
</ul>

<h4>Docker Compose establishment with all microservices on docker :</h4>
<ul>
  <li>
   Containerization of microservices
  </li>
  <li>
    Containerization of databases
  </li>
  <li>
    Override Environment variables
  </li>
</ul>

<hr>

# Installing

<pre>docker-compose up --build</pre>

<p>
  You can launch microservices as below urls:
  <blockquote>
    you can replace Your <b>IP Address</b> in .env file with host.docker.internal
  </blockquote>
</p>

<ul>
  <li>
    <p>Identity Service -> http://host.docker.internal:7018</p>
  </li>
  <li>
    <p>API Gateway Admin -> http://host.docker.internal:11023</p>
  </li>
  <li>
    <p>API Gateway Web -> http://host.docker.internal:10023</p>
  </li>
  <li>
    <p>Product API -> http://host.docker.internal:11002/swagger/index.html</p>
  </li>
    <li>
    <p>Ordering API -> http://host.docker.internal:7002/swagger/index.html</p>
  </li>
    <li>
    <p>Discount API -> http://host.docker.internal:8003/swagger/index.html</p>
  </li>
  <li>
    <p>Basket API -> http://host.docker.internal:6002/swagger/index.html</p>
  </li>
  <li>
    <p>Payment API -> http://host.docker.internal:10002/swagger/index.html</p>
  </li>
  <li>
    <p>Rabbit Management Dashboard -> http://host.docker.internal:15672 -- guest/guest</p>
  </li>
  <li>
    <p>Web Frontend -> http://host.docker.internal:44328</p>
  </li>
  <li>
    <p>Admin Frontend -> http://host.docker.internal:7298</p>
  </li>
</ul>

<h2>Apply Kubernetes clusters with command: </h2>
<pre>kubectl apply /k8s</pre>
<p>
  <blockquote>
    you can find Port or NodePort of services in k8s yaml files
  </blockquote>
</p>
<h4>Currently working on Deploy to Azure Kubernetes Services through CI/CD Azure Pipelines</h4>

<hr>

# Authors
<ul>
  <li>
    Mohammad Khalesi - <a href="https://github.com/mkhalesi">Mkhalesi</a>
  </li>
</ul>
