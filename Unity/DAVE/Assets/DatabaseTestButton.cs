using UnityEngine;
using RethinkDb.Driver.Net;
using RethinkDb;
using System;

public class DatabaseTestButton : MonoBehaviour
{

	void Start ()
	{
		Debug.Log ("--- Starting Connection ---");
		var R = RethinkDb.Driver.RethinkDB.R;
		var conn = R.Connection ().Hostname ("127.0.0.1").Port (28015).Timeout (60).Connect ();

		var result = R.Now().Run<DateTimeOffset>(conn);

		R.Db("root").Table("instructors").Insert(R.Array(
			R.HashMap("name:", "Joacim"),
			R.HashMap("name:", "Shaun"),
			R.HashMap("name:", "Erik"),
			R.HashMap("name:", "Justin")
		)).Run(conn);
			
		Debug.Log ("--- Ending Connection with result: " + result + " ---");
	}
		
}

