using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Resources;
using chap4.helpers;

namespace chap4.resources;

public static class AzResourceGroup
{
    /// <summary>
    ///     Creates an empty Resource Group
    /// </summary>
    /// <remarks>
    ///     To get the details of the resource definition for Microsoft.Resources/resourceGroups please refer to
    ///     <see href="https://learn.microsoft.com/de-de/azure/templates/microsoft.resources/resourcegroups?pivots=deployment-language-bicep" />.
    ///     For the Pulumi documentation please visit <see href="https://www.pulumi.com/registry/packages/azure-native/api-docs/resources/resourcegroup/" />.
    /// </remarks>
    /// <param name="location">
    ///     The Azure region where the resource will be created. It cannot be changed after the resource is created. Please use the
    ///     default regions defined by Zeiss.
    /// </param>
    /// <param name="protectResource">
    ///     Defines the protection state of the resource. If true, the resource cannot be accidentially deleted by removing it from
    ///     the stack.
    /// </param>
    /// <param name="resourceGroupName">
    ///     The unique name of the Resource Group. Please use the ConventionOverConfiguration helper to ensure the naming
    ///     convention.
    /// </param>
    /// <param name="tagList">
    ///     Defines the resource tags which can be used to add meta information that describe the resource. Just use them when you want to
    ///     define specific tags. Some tags are already defined on the Subscription scope.
    /// </param>
    /// <returns>The properties of a Resource Group Object</returns>
    public static ResourceGroup Define(
        string resourceGroupName,
        string location,
        List<KeyValuePair<string, string>> tagList,
        bool protectResource)
    {
        // Variable Section
        var pulumiName = resourceGroupName;

        // Resource creation
        return new ResourceGroup(pulumiName, new ResourceGroupArgs
            {
                ResourceGroupName = resourceGroupName,
                Location = location,
                Tags = Tags.CreateTags(tagList)
            },
            new CustomResourceOptions
            {
                Protect = protectResource
            });
    }
}
