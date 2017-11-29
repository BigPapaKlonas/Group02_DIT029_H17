using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class JsonBroker
{
    private Button uplButton;

    public JsonBroker(string nJson, float houseOffset)
    {
        // offset used to position house "districts"

        if (IsValidJson(nJson))
        {
            // Creates a JsonHelper for the parsing.
            JsonHelper JsonHelper = new JsonHelper(nJson);
            uplButton = GameObject.Find("UploadBtn").GetComponent<Button>();
            
            switch (JsonHelper.GetDiagramType())
            {
                case "sequence_diagram":
                    Debug.Log("Sequence");
                    RenderSequence(JsonHelper.ParseSequence());
                    break;
                case "class_diagram":
                    Debug.Log("Class");
                    RenderClassDiagram(JsonHelper.ParseClass(), houseOffset);
                    break;
                case "deployment_diagram":
                    Debug.Log("deployment");
                    RenderDeployment(JsonHelper.ParseDeployment());
                    break;
                default:
                    Debug.Log("Invalid diagram type");
                    break;
            }
        }
        else
        {
            Debug.Log("Invalid JSON");
            //throw new ArgumentException("The JSON is Invalid");
        }
    }

    public void RenderSequence(JSONSequence JSONSequence)
    {
        RenderSystemBoxes(JSONSequence);
        RenderMessages(JSONSequence);
    }

    public void RenderClassDiagram(JSONClass JSONClass, float houseOffset)
    {
        string id = Guid.NewGuid().ToString("N");
        RenderClasses(JSONClass, id, houseOffset);
        RenderRelationships(JSONClass, id);
    }

    public void RenderDeployment(JSONDeployment JSONDeployment)
    {
        //Placeholder
    }

    private void RenderSystemBoxes(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
    }

    private void RenderMessages(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<StartMessages>().NewMessage(JSONSequence);
    }

    public void RenderClasses(JSONClass JSONClass, string id, float offset)
    {
        uplButton.GetComponent<RenderClasses>().AddHouse(JSONClass, id, offset);
    }

    public void RenderRelationships(JSONClass JSONClass, string id)
    {
        uplButton.GetComponent<RenderClassRelationship>().AddRelationship(JSONClass, id);
    }

    private bool IsValidJson(string strInput)
    //https://stackoverflow.com/questions/14977848/how-to-make-sure-that-string-is-valid-json-using-json-net
    {
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(strInput);
                obj.Equals(obj); //Could not suppress warning 'value is assigned but never use' so this prevents the error message
                return true;
            }
            catch (JsonReaderException jex) //Exception in parsing json
            {
                Debug.Log(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Debug.Log(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
