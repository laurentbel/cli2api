using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Newtonsoft.Json;

namespace cli2api
{
    public static class RunCommand
    {
        private static string _apiKeyName = "x-api-key";

        [FunctionName("RunCommand")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{command}/{*arguments}")] HttpRequest req,
            string command,
            string arguments,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Check if an api key is expected
            string apiKey = Environment.GetEnvironmentVariable("cli2api:api_key") ?? "";
            if (!string.IsNullOrEmpty(apiKey)) {
                if (!req.Headers.ContainsKey(_apiKeyName) ||
                    req.Headers[_apiKeyName] != apiKey) {
                        return new UnauthorizedResult();
                    }
            }

            // Get list of allowed commands from config
            string allowedCommands = Environment.GetEnvironmentVariable("cli2api:commands") ?? "";
            if (!allowedCommands.Split(',').Contains(command)) {
                return new BadRequestObjectResult($"Command {command} is not allowed. Ensure env variable cli2api:commands is correctly configured.");
            }

            // Parsing arguments
            if (!string.IsNullOrEmpty(arguments)) {
                string[] argumentsArray = arguments.Split('/');
                argumentsArray = argumentsArray.Select(x => x = Uri.UnescapeDataString(x)).ToArray();
                arguments = String.Join(' ', argumentsArray);
            }

            // Append any suffix argument
            string suffixArgument = Environment.GetEnvironmentVariable("cli2api:suffix_argument") ?? "";
            arguments = string.Concat(arguments, " ", suffixArgument).TrimEnd();
            
            // Run the command
            CommandLine cli = new CommandLine();
            await cli.RunAsync(command, arguments);

            // Return the result
            if (!string.IsNullOrEmpty(cli.StandardOutput)) {
                if (cli.IsStandardOutputJson) {
                    return new ContentResult { Content = cli.StandardOutput, ContentType = "application/json" };
                }
                else {
                    return new OkObjectResult(cli.StandardOutput);
                }
            }
            else if (!string.IsNullOrEmpty(cli.StandardError)) {
                if (cli.IsStandardErrorJson) {
                    return new ContentResult { Content = cli.StandardError, ContentType = "application/json" };
                }
                else {
                    return new OkObjectResult(cli.StandardError);
                }
            }
            else {
                return new OkResult();
            }
        }
    }
}
