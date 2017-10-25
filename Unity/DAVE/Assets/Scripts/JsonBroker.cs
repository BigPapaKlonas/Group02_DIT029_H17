using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JsonBroker : MonoBehaviour 
{

    private QuickType.JSON json;
    private Button uplButton;
    private Text invalidJSON;
    public float invalidJSONTextDuration = 3f;

    public JsonBroker(string nJson)
    {

        if (IsValidJson(nJson))
        {
            Debug.Log("Entered true" + " bool är : " + IsValidJson(nJson));

            QuickType.JSON parsed = QuickType.JsonHelper.ParseToClass(nJson);
            this.json = parsed;
            uplButton = GameObject.Find("UploadBtn").GetComponent<Button>();
            Render();
        }
        else
        {
            //invalidJSON = GameObject.Find("InvalidJSON").GetComponent<Text>();
            //StartCoroutine(TemporarilyActivate());
        }

    }

    public void Render()
    {
        RenderSystemBoxes();
        RenderMessages();
    }

    private void RenderSystemBoxes()
    {
        uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(this.json);
    }

    private void RenderMessages()
    {
        uplButton.GetComponent<StartMessages>().NewMessage(this.json);
    }
    //Validates the Json string (https://stackoverflow.com/questions/14977848/how-to-make-sure-that-string-is-valid-json-using-json-net)
    private static bool IsValidJson(string strInput)
    {
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(strInput);
                return true;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
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

    private IEnumerator TemporarilyActivate()
    {
        invalidJSON.enabled = true;
        yield return new WaitForSeconds(invalidJSONTextDuration);
        invalidJSON.enabled = false;
    }
}
