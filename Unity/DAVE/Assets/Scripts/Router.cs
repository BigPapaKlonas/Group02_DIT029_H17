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
	public Button insNameBtn;
	public Button diaNameBtn;
    public Button studentNameBtn;
	public Button uploadBtn;

	public InputField studentName;
	public InputField instructorName;
	public InputField diagramName;

	public Text warning;

	void OnEnable()
	{
		studentBtn.onClick.AddListener(()    => buttonCallBack(studentBtn));
		instructorBtn.onClick.AddListener(() => buttonCallBack(instructorBtn));
		insNameBtn.onClick.AddListener(() => buttonCallBack(insNameBtn));
		diaNameBtn.onClick.AddListener(() => buttonCallBack(diaNameBtn));
        studentNameBtn.onClick.AddListener(() => buttonCallBack(studentNameBtn));

    }

	/* 
	 * Method for hiding and handling actions on UI elements.
	 */
	private void buttonCallBack(Button buttonPressed)
	{
		
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
			ConnectionManager.coordinator.SetInstructorBool(true);
			buttonPressed.gameObject.SetActive(false);
		}

		if (buttonPressed == insNameBtn)
		{	
			if (instructorName.text != "") {
				ConnectionManager.coordinator.SetInstructor (instructorName.text);
				buttonPressed.gameObject.SetActive (false);
				instructorName.gameObject.SetActive (false);
			} else {
				warning.text = "You have to enter your name";
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
		insNameBtn.onClick.RemoveAllListeners();
		diaNameBtn.onClick.RemoveAllListeners();
        studentNameBtn.onClick.RemoveAllListeners();
	}


}
