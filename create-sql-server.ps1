# ==========================
# Script: create-sql-server.ps1
# Objetivo: Criar um Azure SQL Server + Database (PaaS) para o MottuBracelet
# ==========================

# Variáveis (edite conforme necessário)
$resourceGroupName = "rg-mottubracelet"
$location = "brazilsouth"
$sqlServerName = "sqlserver-rm558186"   # Deve ser único no Azure
$sqlAdminUser = "admsql"
$sqlAdminPassword = "Fiap@2tdsvms"      # Substituir por senha forte
$sqlDatabaseName = "MottuBraceletDB"

Write-Host "==> Criando Resource Group (se não existir)..."
az group create `
  --name $resourceGroupName `
  --location $location

Write-Host "==> Criando Azure SQL Server..."
az sql server create `
  --name $sqlServerName `
  --resource-group $resourceGroupName `
  --location $location `
  --admin-user $sqlAdminUser `
  --admin-password $sqlAdminPassword

Write-Host "==> Criando Banco de Dados PaaS..."
az sql db create `
  --resource-group $resourceGroupName `
  --server $sqlServerName `
  --name $sqlDatabaseName `
  --service-objective S0

Write-Host "==> Configurando Firewall (acesso liberado para todos os IPs do Azure Services e IP atual)..."

# Libera acesso a todos os serviços do Azure
az sql server firewall-rule create `
  --resource-group $resourceGroupName `
  --server $sqlServerName `
  --name "AllowAzureServices" `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0

# Libera acesso do seu IP atual
$myIp = (Invoke-RestMethod -Uri "https://api.ipify.org?format=json").ip
az sql server firewall-rule create `
  --resource-group $resourceGroupName `
  --server $sqlServerName `
  --name "AllowMyIP" `
  --start-ip-address $myIp `
  --end-ip-address $myIp

# Montar Connection String
$connectionString = "Server=tcp:$sqlServerName.database.windows.net,1433;Initial Catalog=$sqlDatabaseName;Persist Security Info=False;User ID=$sqlAdminUser;Password=$sqlAdminPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

Write-Host "==> Banco de dados criado com sucesso!"
Write-Host "==> Connection String: $connectionString"

# Salvar em um arquivo .env para o deploy usar depois
".NET_CONNECTION_STRING=$connectionString" | Out-File -FilePath "./db-connection.env" -Encoding utf8

Write-Host "==> Connection String salva em db-connection.env"
