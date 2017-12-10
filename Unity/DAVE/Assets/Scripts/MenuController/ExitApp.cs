using UnityEngine;
using UnityEngine.UI;

public class ExitApp : MonoBehaviour {

	private Button button;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}

	void OnClick(){
		Application.Quit();
	}
}
