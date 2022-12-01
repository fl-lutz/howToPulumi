using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using howToPulumi.helpers;

namespace howToPulumi.resources;

public static class AzKeyVault
{
    public enum KeyVaultReferenceIdentity
    {
        SystemAssigned
    }

    /// <summary>
    ///     Creates A Key Vault.
    /// </summary>
    /// <remarks>
    ///     To get the details of the resource definition for Microsoft.KeyVault/vaults please refer to
    ///     <see href="https://learn.microsoft.com/de-de/azure/templates/microsoft.keyvault/vaults?pivots=deployment-language-bicep" />.
    ///     For the Pulumi documentation please visit <see href="https://www.pulumi.com/registry/packages/azure-native/api-docs/keyvault/vault/" />.
    /// </remarks>
    /// <param name="resourceGroup">Name of the resource group to which the resource belongs.</param>
    /// <param name="location">The Azure location of the resource, which cannot be changed after the resource is created.</param>
    /// <param name="tagList">Sets the resource tags which can be used to add meta information that describe the resource.</param>
    /// <param name="protectResource">Defines the protection state of the resource. If true, teh resource cannot be deleted by removing it from the stack.</param>
    /// <param name="keyVaultName">Name of the Key Vault.</param>
    /// <param name="tenantid">The Azure Active Directory tenant ID that should be used for authenticating requests to the key vault.</param>
    /// <returns>The properties of a Key Vault Object</returns>
    public static Vault DefineKeyVault(
        string keyVaultName,
        ResourceGroup resourceGroup,
        string location,
        List<KeyValuePair<string, string>> tagList,
        bool protectResource,
        string tenantid)
    {
        // Variable Section
        var pulumiName = keyVaultName;
        var enableRbacAuthorization = false;
        var enableForDeployment = false;
        var enableForDiskEncryption = false;
        var enbaleForTemplateDeployment = false;
        var enablePurgeProtection = true;
        var enableSoftDelete = true;
        var softDeleteRetentionInDays = 7;
        var skuFamily = "A";
        var skuName = SkuName.Standard;

        // Resource creation
        return new Vault(pulumiName, new VaultArgs
            {
                ResourceGroupName = resourceGroup.Name,
                VaultName = keyVaultName,
                Location = location,
                Tags = Tags.CreateTags(tagList),
                Properties = new VaultPropertiesArgs
                {
                    EnableRbacAuthorization = enableRbacAuthorization,
                    EnabledForDeployment = enableForDeployment,
                    EnabledForDiskEncryption = enableForDiskEncryption,
                    EnabledForTemplateDeployment = enbaleForTemplateDeployment,
                    EnablePurgeProtection = enablePurgeProtection,
                    EnableSoftDelete = enableSoftDelete,
                    SoftDeleteRetentionInDays = softDeleteRetentionInDays,
                    Sku = new SkuArgs
                    {
                        Family = skuFamily,
                        Name = skuName
                    },
                    TenantId = tenantid
                }
            },
            new CustomResourceOptions
            {
                Protect = protectResource
            });
    }


    /// <summary>
    ///     Creates A Key Vault Secret.
    /// </summary>
    /// <remarks>
    ///     To get the details of the resource definition for Microsoft.KeyVault/vaults/secrets please refer to
    ///     <see href="https://learn.microsoft.com/de-de/azure/templates/microsoft.keyvault/vaults/secrets?pivots=deployment-language-bicep" />.
    ///     For the Pulumi documentation please visit <see href="https://www.pulumi.com/registry/packages/azure-native/api-docs/keyvault/secret/" />.
    /// </remarks>
    /// <param name="secretName">Name of the secret.</param>
    /// <param name="resourceGroupName">Name of the resource group to which the resource belongs.</param>
    /// <param name="keyVaultName">Name of the Key Vault.</param>
    /// <param name="tagList">Sets the resource tags which can be used to add meta information that describe the resource.</param>
    /// <param name="protectResource">Defines the protection state of the resource. If true, teh resource cannot be deleted by removing it from the stack.</param>
    /// <param name="description">The content type or description of the secret.</param>
    /// <param name="value">
    ///     The value of the secret. NOTE: 'value' will never be returned from the service, as APIs using this model are is intended for
    ///     internal use in ARM deployments. Users should use the data-plane REST service for interaction with vault secrets.
    /// </param>
    /// <param name="enabled">Determines whether the object is enabled.</param>
    /// <param name="expirationDate">Expiry date in seconds since 1970-01-01T00:00:00Z.</param>
    /// <param name="notBefore">Activates the secret not before date in seconds since 1970-01-01T00:00:00Z.</param>
    /// <returns>The properties of a Key Vault Secret Object</returns>
    public static Secret DefineKeyVaultSecret(
        string secretName,
        Output<string> keyVaultName,
        Output<string> resourceGroupName,
        List<KeyValuePair<string, string>> tagList,
        bool protectResource,
        string description,
        string value,
        bool enabled,
        int expirationDate,
        int notBefore)
    {
        // Variable Section
        var pulumiName = secretName;

        // Resource creation
        return new Secret(pulumiName, new SecretArgs
            {
                SecretName = secretName,
                ResourceGroupName = resourceGroupName,
                VaultName = keyVaultName,
                Tags = Tags.CreateTags(tagList),
                Properties = new SecretPropertiesArgs
                {
                    ContentType = description,
                    Value = value,
                    Attributes = new SecretAttributesArgs
                    {
                        Enabled = enabled,
                        Expires = expirationDate,
                        NotBefore = notBefore
                    }
                }
            },
            new CustomResourceOptions
            {
                Protect = protectResource
            });
    }
}
