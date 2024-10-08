using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using FileModel.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YourProjectName.AzureFunctions
{
    public class CosmosDbChangeTrigger
    {
        private readonly IConfiguration _configuration;

        public CosmosDbChangeTrigger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName("CosmosDbChangeTrigger")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "fileupload",
            collectionName: "test",
            ConnectionStringSetting = "cosmosdbconnection1",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<FileModel> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);

                var apiKey = _configuration["Mailtrap:ApiKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_configuration["Mailtrap:FromEmail"], "Example User");
                var subject = "Cosmos DB Change Detected";
                var to = new EmailAddress(_configuration["Mailtrap:ToEmail"], "Example User");
                var plainTextContent = "A change was detected in Cosmos DB.";
                var htmlContent = "<strong>A change was detected in Cosmos DB.</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
