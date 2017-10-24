// written by Bekwnn, 2015
// contributed by Guney Ozsan, 2016

using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class JsonHelper
{

    public static string GetJsonObject(string jsonString, string handle)
    {
        string pattern = "\"" + handle + "\"\\s*:\\s*\\{";

        Regex regx = new Regex(pattern);

        Match match = regx.Match(jsonString);

        if (match.Success)
        {
            int bracketCount = 1;
            int i;
            int startOfObj = match.Index + match.Length;
            for (i = startOfObj; bracketCount > 0; i++)
            {
                if (jsonString[i] == '{') bracketCount++;
                else if (jsonString[i] == '}') bracketCount--;
            }
            return "{" + jsonString.Substring(startOfObj, i - startOfObj);
        }

        //no match, return null
        return null;
    }

    public static string[] GetJsonObjects(string jsonString, string handle)
    {
        string pattern = "\"" + handle + "\"\\s*:\\s*\\{";

        Regex regx = new Regex(pattern);

        //check if there's a match at all, return null if not
        if (!regx.IsMatch(jsonString)) return null;

        List<string> jsonObjList = new List<string>();

        //find each regex match
        foreach (Match match in regx.Matches(jsonString))
        {
            int bracketCount = 1;
            int i;
            int startOfObj = match.Index + match.Length;
            for (i = startOfObj; bracketCount > 0; i++)
            {
                if (jsonString[i] == '{') bracketCount++;
                else if (jsonString[i] == '}') bracketCount--;
            }
            jsonObjList.Add("{" + jsonString.Substring(startOfObj, i - startOfObj));
        }

        return jsonObjList.ToArray();
    }

    public static string[] GetJsonObjectArray(string jsonString, string handle)
    {
        string pattern = "\"" + handle + "\"\\s*:\\s*\\[\\s*{";

        Regex regx = new Regex(pattern);

        List<string> jsonObjList = new List<string>();

        Match match = regx.Match(jsonString);

        if (match.Success)
        {
            int squareBracketCount = 1;
            int curlyBracketCount = 1;
            int startOfObjArray = match.Index + match.Length;
            int i = startOfObjArray;
            while (true)
            {
                if (jsonString[i] == '[') squareBracketCount++;
                else if (jsonString[i] == ']') squareBracketCount--;

                int startOfObj = i;
                for (i = startOfObj; curlyBracketCount > 0; i++)
                {
                    if (jsonString[i] == '{') curlyBracketCount++;
                    else if (jsonString[i] == '}') curlyBracketCount--;
                }
                jsonObjList.Add("{" + jsonString.Substring(startOfObj, i - startOfObj));

                // continue with the next array element or return object array if there is no more left
                while (jsonString[i] != '{')
                {
                    if (jsonString[i] == ']' && squareBracketCount == 1)
                    {
                        return jsonObjList.ToArray();
                    }
                    i++;
                }
                curlyBracketCount = 1;
                i++;
            }
        }

        //no match, return null
        return null;
    }

    public static bool IsNull<T>(this T[] array)
    {
        return array == null;
    }

    public static bool IsEmpty<T>(this T[] array)
    {
        return array.Length == 0;
    }

    public class Meta
    {
        public string format;
        public string version;
        public string extension;

        public override string ToString()
        {
            return "\r\n" + "format: " + format + "\r\n" + "version: " + version + "\r\n" + "extension: " + extension;
        }
    }

    public class Type
    {
        public string type;

        public override string ToString()
        {
            return "\r\n" + "type: " + type;
        }
    }

    public class Processes
    {
        public string @class;
        public string name;

        public override string ToString()
        {
            return "\r\n" + "class: " + @class + "\r\n" + "name: " + name;
        }
    }

    public class ProcessesList
    {

        public List<Processes> Processes;
    }

    public static void parseJSON(string jsonString)
    {

        string metaString = JsonHelper.GetJsonObject(jsonString, "meta");
        Debug.Log("metaString: " + metaString);

        Meta meta = JsonUtility.FromJson<Meta>(metaString);
        Debug.Log("Diagram_meta: " + meta.ToString());



        Type type = JsonUtility.FromJson<Type>(jsonString);
        Debug.Log("Diagram_type: " + type.ToString());



        Processes[] objects = JsonHelper2.getJsonArray<Processes>(jsonString);

        ProcessesList myProcessesList = new ProcessesList();
        JsonUtility.FromJsonOverwrite(jsonString, myProcessesList);

        Debug.Log("processes is null?: " + objects.IsNull());

        //string processesString = JsonHelper.GetJsonObject(jsonString, "diagram");
        //Debug.Log("processesString: " + processesString);


    }

    public class JsonHelper2
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"processes\": " + json + "}";
            Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}