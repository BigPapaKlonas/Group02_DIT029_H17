using UnityEngine;
using UnityEngine.UI;

public class Router : MonoBehaviour {

	/* 
	 * This class handles the routing of buttons on the 
	 * start screen of the application.
	 * When the instructor path is choosen diagram name and 
	 * instructor name is added to the database.
	*/
	public Button studentBtn;
	public Button instructorBtn;
	public Button diaNameBtn;
    public Button studentNameBtn;
	public Button uploadBtn;

	public InputField studentName;
	public InputField diagramName;

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
            loginBtn.gameObject.SetActive(false);
        }
    }

    void OnEnable()
	{

        studentBtn.onClick.AddListener(()    => buttonCallBack(studentBtn));
		instructorBtn.onClick.AddListener(() => buttonCallBack(instructorBtn));
		diaNameBtn.onClick.AddListener(() => buttonCallBack(diaNameBtn));
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
            GameObject login = Instantiate(loginPanel);
            login.transform.SetParent(canvas.transform, false);
            startPanel.SetActive(false);
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
		if (buttonPressed == diaNameBtn)
		{
			if (diagramName.text != "") {
				ConnectionManager.coordinator.SetDiagram (diagramName.text);
				buttonPressed.gameObject.SetActive (false);
				diagramName.gameObject.SetActive (false);
            
				// Publish to the broker.
				ConnectionManager.coordinator.Publish (
					"root/" + ConnectionManager.coordinator.GetInstructor () + "/" +
					ConnectionManager.coordinator.GetDiagram (), 
					"Init diagram", 
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
		diaNameBtn.onClick.RemoveAllListeners();
        studentNameBtn.onClick.RemoveAllListeners();
	}


}
