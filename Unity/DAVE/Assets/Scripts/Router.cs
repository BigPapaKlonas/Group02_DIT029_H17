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
            Insert();
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

	// Insert to the Database.
    void Insert()
    {

        string instructor = Coordinator.coordinator.GetInstructor();

		/* 
		 * If database contains the instructor name: 
		 * Update instructors.diagrams
		 * and add the diagram to diagrams table.
		 * Else:
		 * Add both to instructors and diagrams tables.
		*/
		if (Coordinator.R.Db ("root").Table ("instructors").GetField ("name")
			.Contains (instructor).Run (Coordinator.conn)) 
		{
			Coordinator.R.Db("root")
				.Table("diagrams").Insert(Coordinator.R.Array(
					Coordinator.R.HashMap("name", Coordinator.coordinator.GetDiagram())
					// TODO: Handle different types of diagrams.
					.With("type", "sequence_diagram")
					.With("instructor", instructor)
				))
				.Run(Coordinator.conn);
			
			Coordinator.R.Db ("root")
				.Table ("instructors")
				.Filter (row => row.G ("name").Eq (instructor))
				.Update (Coordinator.R.HashMap("diagrams", Coordinator.R.Array(Coordinator.coordinator.GetDiagram())))
				.Run(Coordinator.conn);
			
		} else {
			
	       Coordinator.R.Db("root")
				.Table("diagrams").Insert(Coordinator.R.Array(
	            Coordinator.R.HashMap("name", Coordinator.coordinator.GetDiagram())
				 // TODO: Handle different types of diagrams.
	            .With("type", "sequence_diagram")
	            .With("instructor", instructor)
	            ))
	        .Run(Coordinator.conn);

	        Coordinator.R.Db("root")
				.Table("instructors").Insert(Coordinator.R.Array(
	            Coordinator.R.HashMap("name", instructor)
	            .With("diagrams", Coordinator.R.Array())
	              )
	            )
	        .Run(Coordinator.conn);
		}
    }
}
