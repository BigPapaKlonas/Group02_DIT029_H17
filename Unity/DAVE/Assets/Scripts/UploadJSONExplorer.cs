using System.Collections;
using System.Runtime.InteropServices;
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
    private Button button;
    
    // offset used to position house "districts"
    float positionOffset = -50;



#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string id);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name);
    }

    // Called from browser
    public void OnFileUploaded(string url) {
        StartCoroutine(OutputRoutine(url));
}

#else
    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnPointerDown(PointerEventData eventData) { }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(Title, Directory, Extension, Multiselect);
        foreach(var file in paths)
        {
            if (file.Length > 0)
            {
                // Starts a new routine with the path to the selected file as argument
                StartCoroutine(OutputRoutine(new System.Uri(file).AbsoluteUri));
            }
        }
    }

#endif
    private IEnumerator OutputRoutine(string url)
    {
        // Debug: file's path
        Debug.Log("Path: " + url);

        var loader = new WWW(url);
        yield return loader;

        // Read the json from the file into a string
        string output = loader.text;

        // Debug: Json text
        Debug.Log("Raw JSON: " + output);

        // Creates a broker for the parsing and rendering.
        new JsonBroker(output, positionOffset);
        positionOffset += 40;
    }
}