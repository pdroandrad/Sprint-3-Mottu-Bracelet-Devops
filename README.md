# Mottu Bracelet

## 📌 Descrição do Projeto

O **Mottu Bracelet** é um projeto desenvolvido para a empresa Mottu, visando o gerenciamento eficiente de motos nos pátios de manutenção. Cada moto recebe um bracelete que se comunica com o aplicativo, permitindo:

- Localização rápida da moto no pátio.
- Emissão de sinais sonoros e infravermelhos acionados pelo dispositivo.
- Integração de informações entre moto, pátio e dispositivo.

Esta versão do projeto implementa uma **API RESTful** utilizando **ASP.NET Core Web API**, com foco em boas práticas:

- Endpoints CRUD para as entidades **Moto**, **Dispositivo**, **Patio** e **HistoricoPatio**.
- Paginação em listagens.
- Suporte a **HATEOAS** (links para navegação entre recursos).
- Status codes HTTP adequados.
- Documentação automática via **Swagger/OpenAPI**.
---

## 👨‍💻 Integrantes

- Pedro Abrantes Andrade | RM558186
- Ricardo Tavares de Oliveira Filho | RM556092
- Victor Alves Carmona | RM555726

---

## 📐 Arquitetura da Solução:

<img width="532" height="289" alt="sprint-3-devops" src="https://github.com/user-attachments/assets/0335b851-39ed-44b9-afa7-6611de1a1834" />

## 📂 Execução da aplicação com Banco de Dados SQL em nuvem via Azure CLI
Roteiro em .txt disponível em [https://github.com/pdroandrad/Sprint-3-Mottu-Bracelet-Devops/blob/main/Rotero_execucao_webapp.txt]

1. Registrar Serviços:

   ```
   az provider register --namespace Microsoft.Web
   az provider register --namespace Microsoft.Insights
   az provider register --namespace Microsoft.OperationalInsights
   az provider register --namespace Microsoft.ServiceLinker
   az extension add --name application-insights
   ```

2. Realizar Fork e clonar repositório GitHub:
   Link do repositório: [https://github.com/pdroandrad/Sprint-3-Mottu-Bracelet-Devops.git]
   ```
   git clone https://github.com/pdroandrad/Sprint-3-Mottu-Bracelet-Devops.git
   ```

3. Alterar terminal para Powershall e criar Azure SQL Server:

  ```
   cd Sprint-3-Mottu-Bracelet-Devops.git
   .\create-sql-server.ps1
   ```

4. Criar tabelas do Banco de Dados:

   ```
   sqlcmd -S tcp:sqlserver-rm558186.database.windows.net,1433 -U admsql -P 'Fiap@2tdsvms' -d mottubraceletdb -i script_bd.sql
   ```

5. Alterar terminal para Bash e verificar extensão do application-insights:

   ```
   az extension list -o table
   ```

6. Conceder privilégio de execução e rodar o script da aplicação:

   ```
   cd Sprint-3-Mottu-Bracelet-Devops.git
   chmod +x deploy-mottubracelet-dotnet.sh
   ./deploy-mottubracelet-dotnet.sh --login-with-GitHub
   ```

7. No GitHub, adicionar Secrets and Variables:
   settings > secrets and variables > actions > new repository secrets

   - Name: ```ConnectionStrings__DefaultConnection```
   - Value: ```Server=tcp:sqlserver-rm558186.database.windows.net,1433;Initial Catalog=MottuBraceletDB;User Id=admsql;Password=Fiap@2tdsvms;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;```
   
9. Editar arquivo YAML editado dentro da pasta workflows criada na raiz do projeto. Copiar o código abaixo de "run: dotnet publish":

   ```
   env: 
      ConnectionStrings__DefaultConnection: ${{ secrets.ConnectionStrings__DefaultConnection }}
   ```

10. No Portal da Azure acessar o banco de dados SQL criado e fazer login no Editor de Consultas com login e senha.
   Caso o IP não esteja liberado, acessar SQL Server > Segurança > Rede > Adicionar o endereço IPv4 do cliente > Salvar

11. Acessar no browser o endereço ```https://mottubracelet-rm558186.azurewebsites.net/swagger/index.html``` para realização de testes.

12. Realizar testes no Swagger:
    Exemplos de Patios para inserir (POST):

   ```
   {
     "nome": "Pátio Central",
     "capacidadeMaxima": 50,
     "administradorResponsavel": "João da Silva",
     "endereco": {
       "logradouro": "Av. Paulista",
       "numero": 1000,
       "cep": "01310-000",
       "complemento": "Próximo ao metrô Trianon",
       "cidade": "São Paulo",
       "pais": "Brasil"
     }
   }
   ```
   ```
   {
     "nome": "Pátio Zona Norte",
     "capacidadeMaxima": 120,
     "administradorResponsavel": "Maria Oliveira",
     "endereco": {
       "logradouro": "Rua das Flores",
       "numero": 45,
       "cep": "02012-030",
       "complemento": "Ao lado do shopping Norte Center",
       "cidade": "São Paulo",
       "pais": "Brasil"
     }
   }
   ```

12. Verificar no editor de consultas (Banco de dados SQL, no portal da Azure) as operações CRUD:

   ```
   Select * from Patio;
   ```
