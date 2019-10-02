using AzureFunctionDriver.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace AzureFunctionDriver
{
    /// <summary>
    /// Azure Function Helper for triggering azure functions that do not trigger automatically
    /// </summary>
    public class AzureFunctionHelper
    {
        /// <summary>
        /// Publish settings username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Publish settings password
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Kudu Root for azure functions
        /// </summary>
        public string FunctionSiteRoot { get; private set; }

        /// <summary>
        /// Master function key
        /// </summary>
        public string MasterKey { get; private set; }

        public AzureFunctionHelper(String AzureAccountName, String AzurePassword, String AzureFunctionRoot, String AzureMasterKey)
        {
            this.Password = AzurePassword;
            this.Username = AzureAccountName;
            this.FunctionSiteRoot = AzureFunctionRoot;
            this.MasterKey = AzureMasterKey;
        }

        /// <summary>
        /// Web Client for accessing azure functions
        /// </summary>
        /// <returns>Web Client</returns>
        private WebClient webClient => new WebClient
        {
            Headers = { ["ContentType"] = "application/json" },
            Credentials = new NetworkCredential(Username, Password),
            BaseAddress = FunctionSiteRoot
        };

        private HttpClient httpClient => new HttpClient();

        /// <summary>
        /// Checks if a function exists
        /// </summary>
        /// <param name="functionName">Function's name</param>
        /// <returns>True if function exists</returns>
        public bool FunctionExists(String functionName)
        {
            try
            {
                Function functionJson =
                       JsonConvert.DeserializeObject<Function>(webClient.DownloadString(GetFunctionJsonUrl(functionName.ToString())));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Triggers an azure function
        /// </summary>
        /// <param name="functionName">Function's name</param>
        public void TriggerFunction(String functionName)
        {
            WebClient client = webClient;
            client.Headers.Add("x-functions-key", this.MasterKey);
            client.UploadString($"admin/functions/{functionName}", "POST", "");
        }

        /// <summary>
        /// Triggers an azure function
        /// </summary>
        /// <param name="functionName">Function's name</param>
        [Obsolete("Use trigger function instead")]
        public void TriggerFunctionRewrite(String functionName)
        {
            StopFunction(functionName);
            StartFunction(functionName);
        }

        /// <summary>
        /// Gets a function object
        /// </summary>
        /// <param name="functionName">The name of the function</param>
        /// <returns>Function object</returns>
        public Function GetFunctionAsObject(String functionName)
        {
            return JsonConvert.DeserializeObject<Function>(webClient.DownloadString(GetFunctionJsonUrl(functionName)));
        }

        /// <summary>
        /// Stops a function
        /// </summary>
        /// <param name="functionName">Function's name</param>
        private void StopFunction(String functionName)
        {
            SetFunctionState(functionName, true);
        }

        /// <summary>
        /// Starts a function
        /// </summary>
        /// <param name="functionName">Function's name</param>
        private void StartFunction(String functionName)
        {
            SetFunctionState(functionName, false);
        }

        /// <summary>
        /// Sets a functions state
        /// </summary>
        /// <param name="functionName">Function name</param>
        /// <param name="isDisabled">Sets function to result</param>
        private void SetFunctionState(String functionName, bool isDisabled)
        {
            try
            {
                string before = webClient.DownloadString(GetFunctionJsonUrl(functionName.ToString()));

                Function functionJson =
                    JsonConvert.DeserializeObject<Function>(before);
                functionJson.disabled = isDisabled;
                functionJson.bindings[0].runOnStartup = true;
                string afterwards = JsonConvert.SerializeObject(
                    functionJson,
                                Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                webClient.Headers["If-Match"] = "*";
                webClient.UploadString(GetFunctionJsonUrl(functionName.ToString()), "PUT", afterwards);
            }
            catch (Exception e)
            {
                // Its common for a 409 or a concurrent exception to return if we're executing this function multi-threaded.
                if (e.ToString().Contains("409") || e.ToString().Contains("WebClient does not support concurrent I/O operation"))
                {
                    // expecting error, but function wills till trigger
                }
                else
                {
                    throw new Exception($"Failed to set function state {functionName}.  Connection Info: " + Environment.NewLine + $"Uri: {webClient.BaseAddress}" + Environment.NewLine + $"Username: {Username} Password: {Password}" + Environment.NewLine + $"Failed with exception: {e}");
                }
            }
        }

        /// <summary>
        /// Gets the path for a function name
        /// </summary>
        /// <param name="functionName">Function's name</param>
        /// <returns>Function path</returns>
        private static string GetFunctionJsonUrl(string functionName)
        {
            return $"{functionName}/function.json";
        }
    }
}