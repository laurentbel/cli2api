using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;


namespace cli2api
{
    public static class RunCommand
    {

        [FunctionName("RunCommand")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{command}/{*arguments}")] HttpRequest req,
            string command,
            string arguments,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get list of allowed commands from config
            string allowedCommands = Environment.GetEnvironmentVariable("cli2api:commands") ?? "";
            if (!allowedCommands.Split(',').Contains(command)) {
                return new BadRequestObjectResult($"Command {command} is not allowed. Ensure env variable cli2api:commands is correctly configured.");
            }

            // Parsing arguments
            if (!string.IsNullOrEmpty(arguments)) {
                string[] argumentsArray = arguments.Split('/');
                argumentsArray.ToList().ForEach(x => x = WebUtility.UrlDecode(x));
                arguments = String.Join(' ', argumentsArray);
            }

            // Append any suffix argument
            string suffixArgument = Environment.GetEnvironmentVariable("cli2api:suffix_argument") ?? "";
            arguments = string.Concat(arguments, " ", suffixArgument).TrimEnd();
            
            // Run the command
            CommandLine commandLine = new CommandLine();
            string output = await commandLine.RunAsync(command, arguments);

            // Return the result
            return new OkObjectResult(output);
        }
    }
}
