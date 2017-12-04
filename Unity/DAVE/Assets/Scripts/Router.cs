using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Router : MonoBehaviour {

	/* 
	 * This class handles the routing of buttons on the 
	 * start screen of the application.
	 * When the instructor path is choosen diagram name and 
	 * instructor name is added to the database.
	*/
	public Button studentBtn;
	public Button instructorBtn;
	public Button roomNameBtn;
    public Button studentNameBtn;
	public Button uploadBtn;

	public InputField studentName;
	public InputField roomName;

	public Text warning;

    /*
    * Authentication variables.
    */
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject signUpPanel;

    private GameObject canvas;

    public Button loginBtn;

    private void Start()
    {
        canvas = GameObject.Find("StartCanvas");

        if (ConnectionManager.auth == true)
        {
            loginBtn.GetComponentInChildren<Text>().text = "Log out";
            studentBtn.gameObject.SetActive(false);
            studentName.gameObject.SetActive(false);
            studentNameBtn.gameObject.SetActive(false);
            instructorBtn.GetComponent<RectTransform>().localPosition = new Vector3 (0f, -25f, 0f);
            roomName.GetComponent<RectTransform>().localPosition = new Vector3(-15f, -25f, 0f);
            uploadBtn.GetComponent<RectTransform>().localPosition = new Vector3(0f, -25f, 0f);
        }
    }

    void OnEnable()
	{

        studentBtn.onClick.AddListener(()    => buttonCallBack(studentBtn));
		instructorBtn.onClick.AddListener(() => buttonCallBack(instructorBtn));
		roomNameBtn.onClick.AddListener(() => buttonCallBack(roomNameBtn));
        studentNameBtn.onClick.AddListener(() => buttonCallBack(studentNameBtn));
        loginBtn.onClick.AddListener(() => buttonCallBack(loginBtn));

    }

	/* 
	 * Method for hiding and handling actions on UI elements.
	 */
	private void buttonCallBack(Button buttonPressed)
	{
        if (buttonPressed == loginBtn)
        {
            if (ConnectionManager.auth != true)
            {
                GameObject login = Instantiate(loginPanel);
                login.transform.SetParent(canvas.transform, false);
                startPanel.SetActive(false);
            }
            else
            {
                ConnectionManager.auth = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }
		
		if (buttonPressed == studentBtn)
		{
			ConnectionManager.coordinator.SetInstructorBool(false);
			buttonPressed.gameObject.SetActive(false);
		}

        if (buttonPressed == studentNameBtn)
        {
			ConnectionManager.coordinator.SetStudent(studentName.text);
        }

		if (buttonPressed == instructorBtn)
		{
            if (ConnectionManager.auth == false)
            {  
                GameObject login = Instantiate(loginPanel);
                login.transform.SetParent(canvas.transform, false);
                startPanel.SetActive(false);
            }
            else
            {
                ConnectionManager.coordinator.SetInstructorBool(true);
                buttonPressed.gameObject.SetActive(false);
            }
        }
		if (buttonPressed == roomNameBtn)
		{
			if (roomName.text != "") {
				ConnectionManager.coordinator.SetRoom (roomName.text);
				buttonPressed.gameObject.SetActive (false);
                roomName.gameObject.SetActive (false);
            
				// Publish to the broker.
				ConnectionManager.coordinator.Publish (
					"root/" + ConnectionManager.coordinator.GetInstructor () + "/" +
					ConnectionManager.coordinator.GetRoom (), 
					"Init room", 
					true    
				);
			} else {
				warning.text = "You have to enter a diagram name";
			}
        }

	}

	void OnDisable()
	{
		studentBtn.onClick.RemoveAllListeners();
		instructorBtn.onClick.RemoveAllListeners();
		roomNameBtn.onClick.RemoveAllListeners();
        studentNameBtn.onClick.RemoveAllListeners();
	}


}
