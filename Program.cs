using System.Collections.Generic;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using howToPulumi.resources;
using Pulumi;

return await Pulumi.Deployment.RunAsync(() =>
{
        var config = new Pulumi.Config();
        var tenantId = GetClientConfig.InvokeAsync().Result.TenantId;
        var subscriptionId = GetClientConfig.InvokeAsync().Result.SubscriptionId;
        //set manually for exception - just leave it to use default in config azurenative:location
        var location = config.Require("location");
        var tags = new List<KeyValuePair<string, string>>(){new KeyValuePair<string, string>("Provisioner", "Pulumi")};

        //Get Deployment Secrets from Keyvault
        Output<GetSecretResult> example = GetSecret.Invoke(new GetSecretInvokeArgs(){
            ResourceGroupName="zofllutz",
            SecretName="howToPulumiSecret",
            VaultName="tstkv08151"});

        #region Resource Group

        //Pulumi.Log.Info(example.Apply(value => value.Properties.Value));

        var resourceGroup = AzResourceGroup.Define(
            "howToPulumi-RG",
            location,
            new List<KeyValuePair<string, string>>(),
            true
        );

        #endregion

        #region Key Vault
        
        var keyvault = AzKeyVault.DefineKeyVault(
            "howToPulumi-KV",
            resourceGroup,
            location,
            tags,
            true,
            tenantId
        );

        #endregion
});