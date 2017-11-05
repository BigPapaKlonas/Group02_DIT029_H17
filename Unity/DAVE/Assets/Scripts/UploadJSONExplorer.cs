using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

[RequireComponent(typeof(Button))]
public class UploadJSONExplorer : MonoBehaviour, IPointerDownHandler
{
    // The following variables dictates the file explorer's behaviour and looks
    public string Title = "Select the your diagram file..";
    public string FileName = "";
    public string Directory = "";
    public string Extension = "json";
    public bool Multiselect = false;

    public string output;

    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(Title, Directory, Extension, Multiselect);
        if (paths.Length > 0)
        {
            // Starts a new routine with the path to the selected file as argument
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

    private IEnumerator OutputRoutine(string url)
    {
        // Debug: file's path
        Debug.Log("Path: " + url);

        var loader = new WWW(url);
        yield return loader;

        // Read the json from the file into a string
        output = loader.text;

        // Debug: Json text
        Debug.Log("JSON: " + output);

        //For testing the EventLog script, remove when done
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("logmsg" + "From: gateway \r\nTo: u1 \r\nMessage: [fwe, dwed d, sd]");
            Debug.Log("logmsg" + "From: u1 \r\nTo: u2 \r\nMessage: [yo, ui2, ere]");
            Debug.Log("logmsg" + "From: u5 \r\nTo: u9 \r\nMessage: [re, rer , re]");
        }
      

        /*
        // Pass the json to JsonUtility, and tell it to create a GameData object from it
               GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
        public class PlayerInfo
    {
        public string name;
        public int lives;
        public float health;

        public static PlayerInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayerInfo>(jsonString);
        }

        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }**/

    }
}