using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	private Button button;
	public string sceneName;
    ConnectionManager coordinator = ConnectionManager.coordinator;

    // Use this for initialization
    void Start () {
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		Debug.Log ("Scene name: " + sceneName);
	}
	
	void OnClick(){

        // Unsubscribes from current topic if the scene that is left is eitehr Main or Student
        if (SceneManager.GetActiveScene().name == "Main" | SceneManager.GetActiveScene().name == "Student")
        {
            coordinator.Unsubscribe("root/" + coordinator.GetInstructor() + "/" +
                coordinator.GetRoom()
        );
        }

        // In case the current user is authenticated(instructor), goes back to the "Start" scene
        if (ConnectionManager.auth == false) {
			SceneManager.LoadScene (sceneName);
		} else {
			SceneManager.LoadScene ("Start");
		}
	}
}
