using System.Threading.Tasks;
using Pulumi;

namespace Dependencies;

internal static class Program
{
    private static async Task<int> Main()
    {
        return await Deployment.RunAsync<MyStack>();
    }
}