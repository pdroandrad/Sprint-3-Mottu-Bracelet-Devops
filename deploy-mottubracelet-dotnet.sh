#!/bin/bash

# ==========================
# Variáveis
# ==========================
export RESOURCE_GROUP_NAME="rg-mottubracelet"
export WEBAPP_NAME="mottubracelet-rm558186"
export APP_SERVICE_PLAN="planMottuBracelet"
export LOCATION="brazilsouth"
export RUNTIME="DOTNETCORE|8.0"
export APP_INSIGHTS_NAME="ai-mottubracelet"
export GITHUB_REPO_NAME="pdroandrad/Sprint-3-Mottu-Bracelet-Devops"
export BRANCH="main"

# ==========================
# Criar Grupo de Recutsos
# ==========================
echo "Criando o Grupo de Recursos..."
az group create \
  --name $RESOURCE_GROUP_NAME \
  --location "$LOCATION"

# ==========================
# Criar Application Insights
# ==========================
echo "Criando o Application Insights..."
az monitor app-insights component create \
  --app "$APP_INSIGHTS_NAME" \
  --location "$LOCATION" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --application-type web

# ==========================
# Criar App Service Plan
# ==========================
echo "Criando o Plano de Serviço..."
az appservice plan create \
  --name "$APP_SERVICE_PLAN" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --location "$LOCATION" \
  --sku F1 \
  --is-linux

# ==========================
# Criar Web App
# ==========================
echo "Criando o Web App..."
az webapp create \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --plan "$APP_SERVICE_PLAN" \
  --runtime "$RUNTIME"

# ==========================
# Habilitar a autenticacao basica (SCM)
# ==========================
echo "Habilitando a autenticacao basica..."
az resource update \
  --resource-group $RESOURCE_GROUP_NAME \
  --namespace Microsoft.Web \
  --resource-type basicPublishingCredentialsPolicies \
  --name scm \
  --parent sites/$WEBAPP_NAME \
  --set properties.allow=true

# ==========================
# Recuperar a Connection String do Application Insights
# ==========================
echo "Recuperando a Connection String do Application Insights..."
CONNECTION_STRING=$(az monitor app-insights component show \
  --app "$APP_INSIGHTS_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --query connectionString \
  --output tsv)

# ==========================
# Configurar variáveis de ambiente
# ==========================
echo "Configurando variáveis de ambiente no Web App..."
az webapp config appsettings set \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --settings \
    APPLICATIONINSIGHTS_CONNECTION_STRING="$CONNECTION_STRING" \
    ApplicationInsightsAgent_EXTENSION_VERSION="~3" \
    XDT_MicrosoftApplicationInsights_Mode="Recommended" \
    XDT_MicrosoftApplicationInsights_PreemptSdk="1" \
    ConnectionStrings__DefaultConnection="Server=tcp:sqlserver-rm558186.database.windows.net,1433;Initial Catalog=MottuBraceletDB;User Id=admsql;Password=Fiap@2tdsvms;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# ==========================
# Reiniciar Web App
# ==========================
echo "Reiniciando o Web App..."
az webapp restart \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME"

# Criar a conexão do nosso Web App com o Application Insights
az monitor app-insights component connect-webapp \
    --app $APP_INSIGHTS_NAME \
    --web-app $WEBAPP_NAME \
    --resource-group $RESOURCE_GROUP_NAME

# ==========================
# Configurar GitHub Actions para Build e Deploy
# ==========================
echo "Configurando GitHub Actions para o deploy..."
az webapp deployment github-actions add \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --repo "$GITHUB_REPO_NAME" \
  --branch "$BRANCH" \
  --login-with-github

echo "✅ Deploy e workflow GitHub Actions configurados com sucesso!"
echo "Acesse seu Web App: https://$WEBAPP_NAME.azurewebsites.net"
