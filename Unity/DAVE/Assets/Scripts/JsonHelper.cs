using System.Collections.Generic;
using Newtonsoft.Json;

public class JsonHelper
{
    //To serialize (to string) JsonConvert.SerializeObject(JSONSequence, Converter.Settings);

    private string JSONString;
    private string type;

    public JsonHelper(string nJson)
    {
            JSONString = nJson;
            type = JsonConvert.DeserializeObject<JSON>(JSONString, Converter.Settings).Type;
    }

    public string GetDiagramType()
    {
        return type;
    }

    public JSONSequence ParseSequence()
    {
        return JsonConvert.DeserializeObject<JSONSequence>(JSONString, Converter.Settings);
    }

    public JSONClass ParseClass()
    {
        return JsonConvert.DeserializeObject<JSONClass>(JSONString, Converter.Settings);
    }

    public JSONDeployment ParseDeployment()
    {
        return JsonConvert.DeserializeObject<JSONDeployment>(JSONString, Converter.Settings);

    }

}


public partial class JSON
{
    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public partial class JSONSequence : JSON     //Extends the JSON class
{
    [JsonProperty("diagram")]
    public Diagram Diagram { get; set; }

    //Processes is a list composed of process classes
    [JsonProperty("processes")]
    public List<Process> Processes { get; set; }
}

public partial class JSONClass : JSON

{
    [JsonProperty("classes")]
    public Class[] Classes { get; set; }

    [JsonProperty("relationships")]
    public Relationship[] Relationships { get; set; }
}

public partial class JSONDeployment : JSON
{
    [JsonProperty("mapping")]
    public Mapping[] Mapping { get; set; }
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

public partial class Class
{
    [JsonProperty("fields")]
    public Field[] Fields { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public partial class Field
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public partial class Relationship
{
    [JsonProperty("superclass")]
    public string Superclass { get; set; }

    [JsonProperty("subclass")]
    public string Subclass { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public partial class Mapping
{
    [JsonProperty("device")]
    public string Device { get; set; }

    [JsonProperty("process")]
    public string Process { get; set; }
}


public class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
    };
}