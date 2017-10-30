using UnityEngine;
using UnityEngine.UI;

public class RenderSystemBoxes : MonoBehaviour {

  public GameObject systemBoxPrefab;
  private float p = 0;

  private Button button;

  public void CreateSystemBoxes (JSONSequence json) {
    
    foreach(var process in json.Processes){
      // Placing the Boxes
      Vector3 position = new Vector3(0, 10, p);
      GameObject box = (GameObject) Instantiate(
        systemBoxPrefab,
        position,
        this.transform.rotation
      );
      box.name = process.Name;
      // Changes the Z position
      p = p + 3;

      box.GetComponentInChildren<TextMesh>().text =
        process.Name + " : " + process.Class;
        Debug.Log(process.Name + process.Class);
    }

  }
}
