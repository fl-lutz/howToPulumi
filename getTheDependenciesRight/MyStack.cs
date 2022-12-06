using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.KeyVault;

namespace Dependencies;
public class MyStack : Stack
{
    public MyStack(){
        
        var tenantId = GetClientConfig.InvokeAsync().Result.TenantId;


        // Creating Resource Groups:
        // the Deployment of the Resource Groups is parallel, since they have no dependency.
        var resourceGroupService = new ResourceGroup("howToPulumi-service-rg", new ResourceGroupArgs
            {
                ResourceGroupName = "howToPulumi-service-rg",
                Location = "westeurope"
            });
        var resourceGroupIdeas = new ResourceGroup("howToPulumi-ideas-rg", new ResourceGroupArgs
            {
                ResourceGroupName = "howToPulumi-ideas-rg",
                Location = "westeurope"
            });

        // The KeyVault will be created after the Service Resource Group Creation is done.
        var keyVault = new Vault("howToPulumi-kv", new VaultArgs{
            VaultName = "howToPulumi-kv",
            ResourceGroupName = resourceGroupService.Name,
            Location = resourceGroupService.Location,
            Properties = new Pulumi.AzureNative.KeyVault.Inputs.VaultPropertiesArgs{
                TenantId = tenantId,
                Sku = new Pulumi.AzureNative.KeyVault.Inputs.SkuArgs{
                    Name = SkuName.Standard,
                    Family = SkuFamily.A
                }
            }
        });

        // The Secret is dependant on the Keyvault, which is dependant
        // on the Service Resource Group.
        var firstSecret = new Secret("howToPulumi-Sct-1", new SecretArgs{
            VaultName = keyVault.Name,
            ResourceGroupName = resourceGroupService.Name,
            SecretName = "testSecret",
            Properties = new Pulumi.AzureNative.KeyVault.Inputs.SecretPropertiesArgs{
                Value = "topSecretValue"
            }
        });

        // This Secret is additionally to the KeyVault also refering an Ouput
        // of firstSecret, therefore it will be created after fistSecret
        var secondSecret = new Secret("howToPulumi-Sct-2", new SecretArgs{
            VaultName = keyVault.Name,
            ResourceGroupName = resourceGroupService.Name,
            SecretName = Output.Format($"{firstSecret.Name}1"),
            Properties = new Pulumi.AzureNative.KeyVault.Inputs.SecretPropertiesArgs{
                Value = "topSecretValue"
            }
        });

        // This Secret is created in parallel to firstSecret, since it has the same
        // dependencies as firstSecret with the Resource Group and the KeyVault.
        var thirdSecret = new Secret("howToPulumi-Sct-4", new SecretArgs{
            VaultName = keyVault.Name,
            ResourceGroupName = resourceGroupService.Name,
            SecretName = "secret",
            Properties = new Pulumi.AzureNative.KeyVault.Inputs.SecretPropertiesArgs{
                Value = "topSecretValue"
            }
        });

        // This Secret has no Dependencies, since all its Inputs are hardcoded Strings.
        // To avoid any errors in the Deployment, its Dependencies need to be clarified
        // manually, by the DependsOn Section.
        var fourthSecret = new Secret("howToPulumi-Sct-3", new SecretArgs{
            VaultName = "howToPulumi-kv",
            ResourceGroupName = "howToPulumi-service-rg",
            SecretName = "badPracticeSecret",
            Properties = new Pulumi.AzureNative.KeyVault.Inputs.SecretPropertiesArgs{
                Value = "topSecretValue"
            }
        },
        new CustomResourceOptions{
            DependsOn = {resourceGroupService, keyVault} 
        });
    }
}