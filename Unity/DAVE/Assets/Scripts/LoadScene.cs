using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	private Button button;
	public string sceneName;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		Debug.Log ("Scene name: " + sceneName);
	}
	
	void OnClick(){
		if (ConnectionManager.auth == false) {
			SceneManager.LoadScene (sceneName);
		} else {
			SceneManager.LoadScene ("Start");
		}

	}
}
