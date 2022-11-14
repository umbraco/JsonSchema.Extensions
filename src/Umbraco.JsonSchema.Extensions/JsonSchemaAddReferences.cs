using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Umbraco.JsonSchema.Extensions
{
    /// <summary>
    /// Adds references to a JSON schema file.
    /// </summary>
    /// <seealso cref="Microsoft.Build.Utilities.Task" />
    public class JsonSchemaAddReferences : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Gets or sets the JSON schema file.
        /// </summary>
        /// <value>
        /// The JSON schema file.
        /// </value>
        [Required]
        public string JsonSchemaFile { get; set; }

        /// <summary>
        /// Gets or sets the references to add.
        /// </summary>
        /// <value>
        /// The references to add.
        /// </value>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (References.Length == 0)
            {
                // No references to add
                return true;
            }

            using (FileStream fs = File.Open(JsonSchemaFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                JObject schema;
                if (fs.Length == 0)
                {
                    // Create new schema
                    schema = new JObject()
                    {
                        { "$schema", "http://json-schema.org/draft-04/schema#" }
                    };
                }
                else
                {
                    // Read existing schema file
                    using (var sr = new StreamReader(fs, Encoding.UTF8, true, 1024, leaveOpen: true))
                    using (var reader = new JsonTextReader(sr))
                    {
                        schema = (JObject)JToken.ReadFrom(reader);
                    }

                    // Truncate file
                    fs.SetLength(0);
                }

                // Merge schema with references
                schema.Merge(CreateReferences(References), new JsonMergeSettings()
                {
                    MergeArrayHandling = MergeArrayHandling.Union
                });

                // Write schema file
                using (var sw = new StreamWriter(fs))
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;

                    schema.WriteTo(writer);
                }
            }

            return true;
        }

        private static JObject CreateReferences(ITaskItem[] references)
            => new JObject()
            {
                {
                    "allOf",
                    new JArray(references.Select(x => new JObject()
                    {
                        { "$ref", x.ItemSpec }
                    }))
                }
            };
    }
}
