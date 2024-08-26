# Persistência Poliglota: PostgreSQL e MongoDB

Este projeto demonstra uma arquitetura de persistência poliglota, onde utilizamos dois bancos de dados diferentes para atender a requisitos específicos:

- **PostgreSQL** para gerenciar os dados relacionais dos pacientes.
- **MongoDB** para armazenar e manipular registros de prontuários de forma flexível e escalável.

## Arquitetura do Projeto

A aplicação é dividida em duas partes principais:

1. **Gerenciamento de Pacientes (PostgreSQL)**:
   - Armazena informações estruturadas e relacionais sobre pacientes, como nome, endereço e dados de contato.
   - Utiliza Entity Framework Core para interagir com o banco de dados PostgreSQL.

2. **Gerenciamento de Registros de Prontuário (MongoDB)**:
   - Lida com registros de prontuário, que são documentos semiestruturados.
   - Utiliza o driver oficial do MongoDB para C#.

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework web utilizado para desenvolver a API.
- **Entity Framework Core**: ORM utilizado para interação com o PostgreSQL.
- **MongoDB Driver for C#**: Driver utilizado para interagir com o MongoDB.
- **Docker**: Para orquestrar os serviços do PostgreSQL e MongoDB em containers.

## Pré-requisitos

- .NET SDK 6.0 ou superior
- Docker e Docker Compose

## Configuração do Ambiente

### Configuração do Docker

O projeto utiliza Docker Compose para configurar e subir os serviços do PostgreSQL e MongoDB. Certifique-se de que você tenha o Docker instalado e configurado em sua máquina.

Arquivo `docker-compose.yml`:

```yaml
version: '3.8'
services:
  postgres:
    image: postgres:13
    environment:
      POSTGRES_USER: user
      POSTGRES_PASSWORD: password
      POSTGRES_DB: PacientesDB
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  mongodb:
    image: mongo:latest
    container_name: mongodb
    environment:
      MONGO_INITDB_ROOT_USERNAME: mongouser
      MONGO_INITDB_ROOT_PASSWORD: mongopassword
      MONGO_INITDB_DATABASE: RegistrosDosProntuarios
    ports:
      - "27017:27017"
    volumes:
      - mongodb_data:/data/db

volumes:
  postgres_data:
  mongodb_data:
```

### Variáveis de Configuração

No arquivo `appsettings.json`, configure as strings de conexão para os bancos de dados:

```json
{
  "ConnectionStrings": {
    "PostgreSql": "Host=localhost;Database=PacientesDB;Username=user;Password=password",
  },
  "MongoDatabaseConfig": {
    "DatabaseName": "RegistrosDosProntuarios",
    "ConnectionString": "mongodb://mongouser:mongopassword@localhost:27017/RegistrosDosProntuarios?authSource=admin"
  }
}
```

### Subindo os Containers

Execute o comando abaixo para subir os containers do PostgreSQL e MongoDB:

```bash
docker compose up -d
```

## Estrutura do Projeto

- **Controllers/PacienteController.cs**: Controlador que gerencia as operações relacionadas aos pacientes.
- **Services/ProntuarioService.cs**: Serviço que gerencia a criação e manipulação de registros de prontuário no MongoDB.
- **Models/Paciente.cs**: Modelo que representa um paciente no PostgreSQL.
- **Models/Registro.cs**: Modelo que representa um registro de prontuário no MongoDB.

## Funcionalidades

1. **Criar Paciente**:
   - Adiciona um novo paciente ao banco de dados PostgreSQL.

2. **Criar Prontuário e Adicionar Registro**:
   - Cria um prontuário para o paciente recém-criado e adiciona um registro de criação no MongoDB.

3. **Obter Registros do Paciente**:
   - Retorna todos os registros associados a um prontuário de um paciente.

## Como Executar

1. **Suba os containers Docker**:
   ```bash
   docker compose up -d
   ```

2. **Restaure os pacotes NuGet**:
   ```bash
   dotnet restore
   ```

3. **Execute as migrações para o PostgreSQL**:
   ```bash
   dotnet ef database update
   ```

4. **Execute a aplicação**:
   ```bash
   dotnet run
   ```

5. **Acesse a API**:
   - A API estará disponível em `http://localhost:5000`.

## Considerações Finais

Este projeto demonstra como utilizar uma arquitetura de persistência poliglota para combinar as forças de diferentes bancos de dados em um único projeto. Ele é ideal para cenários onde dados relacionais e documentos semiestruturados precisam coexistir.
