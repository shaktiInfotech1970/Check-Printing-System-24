using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var collectionUri = new Uri("https://dev.azure.com/shivshakti1972/");
            var projectName = "CHECK PRINTING SYSTEM 24";

            try
            {
                // Interactive login popup
                var creds = new VssClientCredentials();

                using (var tpc = new TfsTeamProjectCollection(collectionUri, creds))
                {
                    tpc.EnsureAuthenticated();

                    var vcs = tpc.GetService<VersionControlServer>();

                    var project = vcs.GetTeamProject(projectName);

                    project.SetCheckinClientPolicies(null);

                    Console.WriteLine("Policies removed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}