using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;


namespace AzureFunctionDriver.Models
{
    /// <summary>
    /// Bindings for function
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// Gets or sets Type binding
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Gets or sets  Schedule binding
        /// </summary>
        public string schedule { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether User monitor binding
        /// </summary>
        public bool useMonitor { get; set; }

        /// <summary>
        /// Gets or sets  Direction binding
        /// </summary>
        public string direction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Run on startup binding
        /// </summary>
        public bool runOnStartup { get; set; }

        /// <summary>
        /// Gets or sets  Queue name binding
        /// </summary>
        public string queueName { get; set; }

        /// <summary>
        /// Gets or sets  Binding name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets  Connection binding
        /// </summary>
        public string connection { get; set; }

        /// <summary>
        /// Gets or sets API Key Binding
        /// </summary>
        public string apiKey { get; set; }
    }
}