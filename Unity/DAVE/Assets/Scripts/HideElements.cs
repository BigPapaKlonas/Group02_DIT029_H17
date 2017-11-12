using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideElements : MonoBehaviour {

	public Button student;
	public Button instructor;
	public Button insNameBtn;
	public Button diaNameBtn;

	public InputField studentName;
	public InputField instructorName;
	public InputField diagramName;

	void OnEnable()
	{
		student.onClick.AddListener(()    => buttonCallBack(student));
		instructor.onClick.AddListener(() => buttonCallBack(instructor));
		insNameBtn.onClick.AddListener(() => buttonCallBack(insNameBtn));
		diaNameBtn.onClick.AddListener(() => buttonCallBack(diaNameBtn));
	}

	private void buttonCallBack(Button buttonPressed)
	{
		if (buttonPressed == student)
		{
			Debug.Log("Clicked: " + student.name);
			buttonPressed.gameObject.SetActive(false);
			instructor.enabled = false;
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
			buttonPressed.gameObject.SetActive(false);
			instructorName.gameObject.SetActive(false);
		}
		if (buttonPressed == diaNameBtn)
		{
			Debug.Log("Clicked: " + diaNameBtn.name);
			buttonPressed.gameObject.SetActive(false);
			diagramName.gameObject.SetActive(false);
		}

	}

	void OnDisable()
	{
		student.onClick.RemoveAllListeners();
		instructor.onClick.RemoveAllListeners();
		insNameBtn.onClick.RemoveAllListeners();
		diaNameBtn.onClick.RemoveAllListeners();
	}
}
