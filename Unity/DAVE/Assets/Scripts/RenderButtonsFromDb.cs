using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RethinkDb.Driver.Net;
using UnityEngine.SceneManagement;

public class RenderButtonsFromDb : MonoBehaviour {

	public Button buttonPrefab;
	public Canvas canvas;

	// Add string from unity
	public string table;
	public string row;

	private Vector3 position;

	float y = 90f;
	float x = -160f;

	private string name;

	void Start () {

		RenderButtons (table, row);
		ObserveChange (table, row);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// Use this to set up an observer for the selected table, the return is the changes made to the selected row. 
	void ObserveChange (string table, string selectedRow)
	{
		Cursor<string> cursor = Coordinator.R.Db ("root")
			.Table(table).Changes()
			.Filter(row => 
				row.G("new_val").G(selectedRow)
				.Gt(row.G("old_val").G(selectedRow))
			).G("new_val").RunCursor<string>(Coordinator.conn);
		
		foreach (var i in cursor) {
			Debug.Log("Changes: " + i);

			position = new Vector3 (x, y, 0f);
			var btn = Instantiate (buttonPrefab, position, Quaternion.identity) as Button;
			var rectTransform = btn.GetComponent<RectTransform>();
			rectTransform.SetParent (canvas.transform, false);
			btn.GetComponentInChildren<Text>().text = i;

			// Todo: This is a naive solution that needs some work.  
			y -= 30;
			PositionCheck ();
		}
	}

	//This method renders buttons according to our database, use on Start ().
	void RenderButtons (string table, string selectedRow)
	{
		Cursor<string> result = Coordinator.R
			.Db("root").Table(table).GetField(selectedRow)
			.RunCursor<string>(Coordinator.conn);

		foreach(var i in result){
			Debug.Log("Result: " + i);
			position = new Vector3 (x, y, 0f);
			var btn = Instantiate (buttonPrefab, position, Quaternion.identity) as Button;
			var rectTransform = btn.GetComponent<RectTransform>();
			rectTransform.SetParent (canvas.transform, false);
			btn.GetComponentInChildren<Text>().text = i;
			this.name = i;
			btn.onClick.AddListener (OnClick);
			// Todo: This is a naive solution that needs some work.  
			y -= 30;
			PositionCheck ();

		}
	}

	void OnClick ()
	{
		switch (this.table) 
		{
		case "instructors":
			Debug.Log ("table: " + table + " name: " + name);
			Coordinator.coordinator.setInstructor (name);
			SceneManager.LoadScene ("DiagramChoice");
			break;
		case "diagrams":
			Debug.Log ("table: " + table + " name: " + name);
			Coordinator.coordinator.setDiagram (name);
			//SceneManager.LoadScene ("InstructorChoice");
			break;
		default:
			Debug.Log ("Error in table name OnClick");
			break;
		}
	}

	void PositionCheck()
	{
		if (y < -90f) {
			y = 90;
			x += 160;
		} 
	}
}
