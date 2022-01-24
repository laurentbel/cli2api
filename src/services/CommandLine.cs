using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace cli2api {

    // Implement the process that is launch when starting a command line
    public class CommandLine {
        
        // Process info
        private ProcessStartInfo _processStartInfo;

        // Standard output
        private string _standardOutput;
        public string StandardOutput {
            get { return _standardOutput; }
        }
        
        // Standard error
        private string _standardError;
        public string StandardError {
            get { return _standardError; }
        }

        // Is the standard output json formatted
        private bool _isStandardOutputJson;
        public bool IsStandardOutputJson { 
            get { return _isStandardOutputJson; }
        }

        // Is the standard error json formatted
        private bool _isStandardErrorJson;
        public bool IsStandardErrorJson { 
            get { return _isStandardErrorJson; }
        }

        // Constructor
        public CommandLine(){
            // Create a generic ProcessStartInfo
            _processStartInfo = new ProcessStartInfo
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                StandardOutputEncoding = Encoding.UTF8,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        // Run command line async
        public async Task RunAsync(string command, string arguments)
        {
            // Setting the command and arguments
            _processStartInfo.FileName = command;
            _processStartInfo.Arguments = arguments;

            // Start the process and read output
            var process = Process.Start(_processStartInfo);

            // Read asynchronously
            var task1 = process.StandardOutput.ReadToEndAsync();
            var task2 = process.StandardError.ReadToEndAsync();

            // Wait for the process to complete
            bool hasExited = process.WaitForExit(30000);
            if (!hasExited) {
                // Kill process and children if not stopped
                process.Kill(true);
            }

            // Wait for the output and error to complete and get the result
            await Task.WhenAll(task1, task2);
            _standardOutput = task1.Result;
            _standardError = task2.Result;

            // Determine if the standard output is json
            try {
                JsonNode.Parse(_standardOutput);
                _isStandardOutputJson = true;
            }
            catch {
                _isStandardOutputJson = false;
            }

            // Determine if the standard error is json
            try {
                JsonNode.Parse(_standardError);
                _isStandardErrorJson = true;
            }
            catch {
                _isStandardErrorJson = false;
            }
        }
    }
}