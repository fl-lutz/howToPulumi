$env:AZURE_STORAGE_ACCOUNT=$storageAccountName
$env:AZURE_STORAGE_KEY=(az storage account keys list --account-name $storageAccountName | ConvertFrom-Json)[0].value
pulumi login azblob://$containerPath