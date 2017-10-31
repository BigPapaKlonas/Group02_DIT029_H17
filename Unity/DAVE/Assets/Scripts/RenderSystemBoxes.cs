using UnityEngine;
using UnityEngine.UI;

public class RenderSystemBoxes : MonoBehaviour {

  public GameObject systemBoxPrefab;
  private float p = 0;
    float size;

  private Button button;

  public void CreateSystemBoxes (JSONSequence json) {
      foreach (var content in json.Diagram.Content) {
            foreach (var names in content.SubContent)
                size++;
        }
      foreach (var process in json.Processes){
      // Placing the Boxes
      
      Vector3 position = new Vector3(0, (size * 2) + 1, p);
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
    }

  }
}
