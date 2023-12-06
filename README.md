# .NET Microservices Sample Project

There is a couple of microservices which implemented e-commerce modules over <b>Payment</b>, <b>Product</b>, <b>Basket</b>, <b>Discount</b>, <b>Ordering</b> and <b>Identity</b> microservices with <b>Relational databases (Sql Server)</b> with
communicating over <b>RabbitMQ Event Driven Communication</b> and using <b>Ocelot API Gateway</b>.

# Whats Including In This Repository
soon

# Installing

<code>docker-compose up --build</code>

<h3>You can launch microservices as below urls:</h3>
you can replace Your IP Address in .env file with host.docker.internal

<ul>
  <li>
    Product API -> http://host.docker.internal:8000/swagger/index.html
  </li>
  <li>
    Basket API -> http://host.docker.internal:8001/swagger/index.html
  </li>
  <li>
    Discount API -> http://host.docker.internal:8002/swagger/index.html
  </li>
  <li>
    Ordering API -> http://host.docker.internal:8004/swagger/index.html
  </li>
  <li>
    Shopping.Aggregator -> http://host.docker.internal:8005/swagger/index.html
  </li>
  <li>
    API Gateway -> http://host.docker.internal:8010/Catalog
  </li>
  <li>
    Rabbit Management Dashboard -> http://host.docker.internal:15672 -- guest/guest
  </li>
  <li>
    Web UI -> http://host.docker.internal:8006
  </li>
</ul>

