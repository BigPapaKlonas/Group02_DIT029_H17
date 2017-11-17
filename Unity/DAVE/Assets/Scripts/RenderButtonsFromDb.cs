using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using RethinkDb.Driver.Model;
using UnityEngine.SceneManagement;

public class RenderButtonsFromDb : MonoBehaviour {

	public Button buttonPrefab;
	public ScrollRect scroll;
	public RectTransform parentPanel;

	// Add string from unity
	public string table;
	public string row;

	Coordinator coordinator = Coordinator.coordinator;
	RethinkDB R = Coordinator.R;
	Connection conn = Coordinator.conn;

	void Start () {

		RenderButtons (table, row);

	}

	//This method renders buttons according to our database, use on Start ().
	void RenderButtons (string table, string selectedRow)
	{
        Cursor<string> result;
        switch (table)
        { 
            case "instructors":
                Debug.Log("Case instructors");
                result = R.Db("root")
				.Table(table).GetField(selectedRow)
                .RunCursor<string>(conn);
                break;
            case "diagrams":
                Debug.Log("Case diagrams");
                result = R.Db("root")
				.Table(table).Filter(row => row.G("instructor").Eq(coordinator.GetInstructor())).GetField(selectedRow)
               .RunCursor<string>(conn);
                break;
            default:
                result = R.Db("root")
				.Table(table).GetField(selectedRow)
                .RunCursor<string>(conn);
                break;
        }
			

		foreach(var i in result){
			Debug.Log("Result: " + i);
			var btn = Instantiate (buttonPrefab) as Button;

			btn.transform.SetParent (parentPanel, false);
			btn.transform.localScale = new Vector3 (1, 1, 1);
			btn.GetComponentInChildren<Text>().text = i;
			btn.onClick.AddListener(() => ButtonCallBack(btn));
		}
	}

	private void ButtonCallBack (Button buttonPressed)
	{

        string name = buttonPressed.GetComponentInChildren<Text>().text;

        switch (this.table) 
		{
		case "instructors":
			Debug.Log ("table: " + table + " name: " + name);
			Coordinator.coordinator.SetInstructor (name);
			SceneManager.LoadScene ("DiagramChoice");
			break;
		case "diagrams":
			Debug.Log ("table: " + table + " name: " + name);
			Coordinator.coordinator.SetDiagram (name);
			Coordinator.coordinator.Publish (
				"root/" + Coordinator.coordinator.GetInstructor() + "/" + 
				Coordinator.coordinator.GetDiagram() + "/students/", 
				Coordinator.coordinator.GetStudent(),
				true
			);
			Coordinator.coordinator.Subscribe (
				"root/" + Coordinator.coordinator.GetInstructor() + "/" + 
				Coordinator.coordinator.GetDiagram()
			);

			SceneManager.LoadScene (Coordinator.coordinator.GetDiagramType ());

			break;
		default:
			Debug.Log ("Error in table name OnClick");
			break;
		}
	}
		
}
