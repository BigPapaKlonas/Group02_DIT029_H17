using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
		SceneManager.LoadScene (sceneName);
	}
}
