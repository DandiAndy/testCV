using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CognitiveXamarin.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CognitiveXamarin.Services
{
    public class CognitiveTrainingService
    {
        // **********************************************
        // *** Update or verify the following values. ***
        // **********************************************

        // Replace the subscriptionKey string value with your valid subscription key.
        private const string subscriptionKey = "35f8e82d4b0144f3aabb53c3e6a0f05a";

        // Replace or verify the region.
        //
        // You must use the same region in your REST API call as you used to obtain your subscription keys.
        // For example, if you obtained your subscription keys from the westus region, replace 
        // "westcentralus" in the URI below with "westus".
        //
        // NOTE: Free trial subscription keys are generated in the westcentralus region, so if you are using
        // a free trial subscription key, you should not need to change this region.
        private const string uriBase = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.2/Training/projects/768ccd4a-c898-4e0d-9864-0899df095b28/images/files";


        public static async Task<string> CognitiveTraining(string filepath, List<string> tags)
        {
            // Execute the REST API call.
            var request = CreateRequest(filepath, tags);
            return await MakeTrainingRequest(request);
        }

        private static string CreateRequest(string filepath, List<string> tags)
        {
            JObject jRequest = new JObject(
                new JProperty("Image",
                    new JArray(
                        new JObject(
                            new JProperty("Name", filepath),
                            new JProperty("Contents", GetImageAsByteArray(filepath)),
                            new JProperty("TagIds", "")
                        )
                    )
                ),
                new JProperty("TagIds",
                    new JArray(
                        from tag in tags
                        select new JValue(tag)   
                    )
                )
            );
            return jRequest.ToString();
        }


        /// <summary>
        ///     Gets the analysis of the specified image file by using the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        private static async Task<string> MakeTrainingRequest(string request)
        {
            var client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Training-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            //var requestParameters = "visualFeatures=Categories,Description,Color&language=en";

            // Assemble the URI for the REST API Call.
            var uri = uriBase;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            var byteData = Encoding.UTF8.GetBytes(request);


            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                return await response.Content.ReadAsStringAsync();
            }
        }


        /// <summary>
        ///     Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            var binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        /// <summary>
        ///     Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        private static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            var INDENT_STRING = "    ";
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < json.Length; i++)
            {
                var ch = json[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }

                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }

                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        var escaped = false;
                        var index = i;
                        while (index > 0 && json[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }

                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}