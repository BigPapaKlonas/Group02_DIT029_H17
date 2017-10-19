using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSystemBoxes : MonoBehaviour {

  public string[] response = {"u1", "g", "u2"};
  public GameObject systemBoxPrefab;

  private float p = 0;

  void Start () {
    CreateSystemBoxes();
  }
  void Update () {

  }


  void CreateSystemBoxes () {
    foreach(string boxName in response){
      // Placing the Boxes
      Vector3 position = new Vector3(0, 5, p);
      GameObject box = (GameObject) Instantiate(
        systemBoxPrefab,
        position,
        this.transform.rotation
      );
      // Changes the Z position
      p = p + 3;
      box.name = boxName;
      //Check which kind of system box, might need to add more clauses.
      if(boxName[0] == 'g'){
        box.GetComponentInChildren<TextMesh>().text = boxName + ": Gateway";
      } else if (boxName[0] == 'u') {
        box.GetComponentInChildren<TextMesh>().text = boxName + ": User";
      }

      int i = 0;
      while(i < 20){
        i++;
      }

    }
  }
}
