using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RethinkDb.Driver.Net;

public class Router : MonoBehaviour {

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

	private void buttonCallBack(Button buttonPressed)
	{
		if (buttonPressed == student)
		{
			Debug.Log("Clicked: " + student.name);
			buttonPressed.gameObject.SetActive(false);
			instructor.enabled = false;
		}

        if (buttonPressed == studentNameBtn)
        {
            Debug.Log("Clicked: " + studentNameBtn.name);
            Coordinator.coordinator.SetStudent(studentName.text);
            //Coordinator.coordinator.Publish(
              //  "root/students",
              //  Coordinator.coordinator.GetStudent(),
              //  true
            //);
        }

		if (buttonPressed == instructor)
		{
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
            SceneManager.LoadScene("Diagram");
            //Coordinator.coordinator.Publish(
              //  "root/" + Coordinator.coordinator.GetInstructor() + "/" + 
              //  Coordinator.coordinator.GetDiagram() + "/", 
              //  "Init diagram", 
              //  true    
            //);
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

    void Insert()
    {

        string instructor = Coordinator.coordinator.GetInstructor();

        Coordinator.R
        .Db("root").Table("diagrams").Insert(Coordinator.R.Array(
            Coordinator.R.HashMap("name", Coordinator.coordinator.GetDiagram())
            .With("type", "sequence_diagram")
            .With("instructor", instructor)
            ))
        .Run(Coordinator.conn);

        Coordinator.R
        .Db("root").Table("instructors").Insert(Coordinator.R.Array(
            Coordinator.R.HashMap("name", instructor)
            .With("diagrams", Coordinator.R.Array())
              )
            )
        .Run(Coordinator.conn);
    }
}
