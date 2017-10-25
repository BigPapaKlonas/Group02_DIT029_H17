namespace QuickType
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using UnityEngine;

    public static class JsonHelper
    {
        public static JSON ParseToClass(string jsonString)
        {
            JSON data = JSON.FromJson(jsonString);
            return data;
        }
    }

    public partial class JSON
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("diagram")]
        public Diagram Diagram { get; set; }

        [JsonProperty("processes")]
        public List<Process> Processes { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("extensions")]
        public List<object> Extensions { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class Diagram
    {
        [JsonProperty("content")]
        public List<Content> Content { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("content")]
        public List<BaseContent> SubContent { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }
    }

    public partial class BaseContent
    {
        [JsonProperty("message")]
        public List<string> Message { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }

    public partial class Process
    {
        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class JSON
    {
        public static JSON FromJson(string json)
        {
            return JsonConvert.DeserializeObject<JSON>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this JSON self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
