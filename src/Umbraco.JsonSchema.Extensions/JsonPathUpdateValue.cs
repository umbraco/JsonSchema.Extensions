using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Umbraco.JsonSchema.Extensions
{
    /// <summary>
    /// Updates the value of a property in a JSON file using a JSON path expression.
    /// </summary>
    /// <seealso cref="Microsoft.Build.Utilities.Task" />
    public class JsonPathUpdateValue : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Gets or sets the JSON file.
        /// </summary>
        /// <value>
        /// The JSON file.
        /// </value>
        [Required]
        public string JsonFile { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JSON path expression that specifies the property to update.
        /// </summary>
        /// <value>
        /// The JSON path expression that specifies the property to update.
        /// </value>
        [Required]
        public string Path { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JSON value to set.
        /// </summary>
        /// <value>
        /// The JSON value to set.
        /// </value>
        [Required]
        public string Value { get; set; } = null!;

        /// <inheritdoc />
        public override bool Execute()
        {
            using (FileStream fs = File.Open(JsonFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                // Read JSON file
                JToken json;
                using (var sr = new StreamReader(fs, Encoding.UTF8, true, 1024, leaveOpen: true))
                using (var reader = new JsonTextReader(sr))
                {
                    json = JToken.ReadFrom(reader);
                }

                // Select token using JSON path expression
                JToken? jsonToken = json.SelectToken(Path);
                if (jsonToken is not null)
                {
                    // Update JSON value
                    jsonToken.Replace(JToken.Parse(Value));

                    // Truncate file
                    fs.SetLength(0);

                    // Write JSON file
                    using (var sw = new StreamWriter(fs))
                    using (var writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = Formatting.Indented;

                        json.WriteTo(writer);
                    }
                }
            }

            return true;
        }
    }
}
