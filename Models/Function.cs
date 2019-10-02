using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionDriver.Models
{
    /// <summary>
    /// Model for Azure function
    /// </summary> 
    public class Function
    {
        /// <summary>
        /// Gets or sets Bindings
        /// </summary>
        public List<Binding> bindings { get; set; }

        /// <summary>
        /// Gets or sets Script file relative location
        /// </summary>
        public string scriptFile { get; set; }

        /// <summary>
        /// Gets or sets Entry point
        /// </summary>
        public string entryPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Function metadata is set to disabled
        /// </summary>
        public bool disabled { get; set; }
    }
}
