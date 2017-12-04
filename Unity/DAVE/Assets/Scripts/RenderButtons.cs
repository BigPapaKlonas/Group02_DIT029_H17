using UnityEngine;
using UnityEngine.UI;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using UnityEngine.SceneManagement;

public class RenderButtons : MonoBehaviour {

	public Button buttonPrefab;
	public ScrollRect scroll;
	public RectTransform parentPanel;

	// Add string from unity
	public string table;
	public string row;

	ConnectionManager coordinator = ConnectionManager.coordinator;
	RethinkDB R = ConnectionManager.R;
	Connection conn = ConnectionManager.conn;

	void Start () {

		Render (table, row);

	}

	/*
	 * This method renders buttons according to our database.
	 *  It checks which table should be checked (set from unity UI based on scene), 
	 *  Then a cursor(a list of sorts) is created based
	 *   on the filters in the switch statement.
	 * 
	 * Every var in the Cursor is handled in the foreach loop: 
	 * 		1. Instantiate a button with the text based on cursor.
	 * 		2. Create a button callback listener.
	 */
	void Render (string table, string selectedRow)
	{
        Cursor<string> result;
        switch (table)
        { 
            case "instructors":
                result = R.Db("root")
				.Table(table).GetField(selectedRow)
                .RunCursor<string>(conn);
                break;
            case "diagrams":
                result = R.Db("root")
				.Table(table).Filter(row => row.G("instructor")
					.Eq(coordinator.GetInstructor())).GetField(selectedRow)
               .RunCursor<string>(conn);
                break;
            default:
                result = R.Db("root")
				.Table(table).GetField(selectedRow)
                .RunCursor<string>(conn);
                break;
        }
			

		foreach(var i in result){
			var btn = Instantiate (buttonPrefab) as Button;

			btn.transform.SetParent (parentPanel, false);
			btn.transform.localScale = new Vector3 (1, 1, 1);
			btn.GetComponentInChildren<Text>().text = i;
			btn.onClick.AddListener(() => ButtonCallBack(btn));
		}
	}

	/*
	 * The callback that handles the setting of which instructor and diagram the student chose.
	 * In the instructor case it loads the next scene.
	 * In the diagram case it publishes the student name to the chosen room /student and 
	 * subscribes to the "class room". 
	 * Depending on type of diagram a scene is loaded.
	 */
	private void ButtonCallBack (Button buttonPressed)
	{

        string name = buttonPressed.GetComponentInChildren<Text>().text;

        switch (this.table) 
		{
		case "instructors":
			coordinator.SetInstructor (name);
			SceneManager.LoadScene ("DiagramChoice");
			break;
		case "diagrams":
			coordinator.SetRoom (name);
			coordinator.Publish (
				"root/" + coordinator.GetInstructor () + "/" +
				coordinator.GetRoom () + "/students/", 
				coordinator.GetStudent (),
				true
			);
			coordinator.Subscribe (
				"root/" + coordinator.GetInstructor () + "/" +
				coordinator.GetRoom ()
			);

			Cursor<string> result = R.Db ("root").Table ("diagrams")
				.Filter (R.HashMap ("name", coordinator.GetRoom ()))
				.GetField ("type")
				.RunCursor<string> (conn);
					
			foreach (var type in result){
				coordinator.SetDiagramType(type);
			}
				
			SceneManager.LoadScene (coordinator.GetDiagramType ());

			break;
		default:
			Debug.Log ("Error in table name OnClick");
			break;
		}
	}
		
}
