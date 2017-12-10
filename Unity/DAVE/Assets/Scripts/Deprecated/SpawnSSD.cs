using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSSD : MonoBehaviour {

    public GameObject SSD;

    private Button button;
    // Use this for initialization
    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {

        GameObject SSDGO = (GameObject)Instantiate(
          SSD,
          this.transform.position,
          this.transform.rotation
        );
        SSDController SSDController = SSD.GetComponent<SSDController>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
