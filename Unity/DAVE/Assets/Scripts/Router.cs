using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

public class Router : MonoBehaviour {

	/* @author Joacim Eberlen
	 * This class handles the routing of buttons on the start screen of the application.
	 * When the instructor path is choosen diagram name and instructor name is added to the database.
	*/
	public Button student;
	public Button instructor;
	public Button insNameBtn;
	public Button diaNameBtn;
    public Button studentNameBtn;

	public InputField studentName;
	public InputField instructorName;
	public InputField diagramName;

	void OnEnable()
	{
		student.onClick.AddListener(()    => buttonCallBack(student));
		instructor.onClick.AddListener(() => buttonCallBack(instructor));
		insNameBtn.onClick.AddListener(() => buttonCallBack(insNameBtn));
		diaNameBtn.onClick.AddListener(() => buttonCallBack(diaNameBtn));
        studentNameBtn.onClick.AddListener(() => buttonCallBack(studentNameBtn));

    }

	/* 
	 * Method for hiding and handling actions on UI elements.
	 */
	private void buttonCallBack(Button buttonPressed)
	{
		
		if (buttonPressed == student)
		{
			Coordinator.coordinator.SetInstructorBool(false);
			Debug.Log ("ins bool: " + Coordinator.coordinator.GetInstructorBool());
			Debug.Log("Clicked: " + student.name);
			buttonPressed.gameObject.SetActive(false);
			instructor.enabled = false;
		}

        if (buttonPressed == studentNameBtn)
        {
            Debug.Log("Clicked: " + studentNameBtn.name);
            Coordinator.coordinator.SetStudent(studentName.text);
        }

		if (buttonPressed == instructor)
		{
			Coordinator.coordinator.SetInstructorBool(true);
			Debug.Log ("ins bool: " + Coordinator.coordinator.GetInstructorBool());
			Debug.Log("Clicked: " + instructor.name);
			buttonPressed.gameObject.SetActive(false);
			student.enabled = false;
		}

		if (buttonPressed == insNameBtn)
		{
			Debug.Log("Clicked: " + insNameBtn.name);
            Coordinator.coordinator.SetInstructor(instructorName.text);
            buttonPressed.gameObject.SetActive(false);
			instructorName.gameObject.SetActive(false);
		}
		if (buttonPressed == diaNameBtn)
		{
			Debug.Log("Clicked: " + diaNameBtn.name);
            Coordinator.coordinator.SetDiagram(diagramName.text);
			buttonPressed.gameObject.SetActive(false);
			diagramName.gameObject.SetActive(false);
            
			// Publish to the broker.
            Coordinator.coordinator.Publish(
				"root/" + Coordinator.coordinator.GetInstructor() + "/" + 
				Coordinator.coordinator.GetDiagram(), 
                "Init diagram", 
                true    
            );
        }

	}

	void OnDisable()
	{
		student.onClick.RemoveAllListeners();
		instructor.onClick.RemoveAllListeners();
		insNameBtn.onClick.RemoveAllListeners();
		diaNameBtn.onClick.RemoveAllListeners();
        studentNameBtn.onClick.RemoveAllListeners();
	}


}
