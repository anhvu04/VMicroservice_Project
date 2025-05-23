## VMicroservice_Project

**Phan Anh Vu**

### Using docker-compose

- docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans

### Using command

- dotnet ef migrations add InitialCreate --project {project-dir} --startup-project
  {project-dir} --output-dir ./Migrations
- dotnet ef database update --project {project-dir} --startup-project {project-dir}
- dotnet ef migrations remove --project {project-dir} --startup-project {project-dir}

### Application URLs - Development Environment

- Product API: http://localhost:5002/api/products

### Application URLs - Local Environment (Docker Container)

- Product API: http://localhost:6002/api/products

### Docker Application URLs - Local Environment (Docker Container)

- Portainer: http://localhost:6768 - username: admin ; password: admin
- Kibana: http://localhost:5602
- RabbitMq: http://localhost:15673 - username: guest ; password: guest