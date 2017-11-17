using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

public class Coordinator : MonoBehaviour
{
    private Button uplButton;
    private string parentTopic;

    private MqttClient client;
    private MqttClientDAVE daveClient;
    public static Coordinator coordinator;

	// Creation of a instance of our database and the connection to it to be used in our classes.
	public static RethinkDB R;
	public static Connection conn;

	// Private variables to hold onto data during the lifecycle of teh application.
	private string instructor;
	private string diagram;
	private string student;
	private string sessionJSON;
	private string diagramType;
	private bool instructorBool;

    void Start()
    {
		// RethinkDB
		DatabaseConnection ();
		// Mqtt
        EstablishConnection();
    }

    void DatabaseConnection ()
    {
      Debug.Log ("--- Starting Connection ---");
	  // Setup of variables for database connection.
	  R = RethinkDB.R;
	  // Change IP when deployed to AWS.
		conn = R.Connection ().Hostname ("54.93.235.175").Port (28015).Timeout (60).Connect ();

      var result = R.Now().Run<DateTimeOffset>(conn);

      Debug.Log ("--- Connection with result: " + result + " ---");

    }

	void Update () {
		
	}

	void Awake(){
		MakeThisTheOnlyCoordinator();
	}

	void MakeThisTheOnlyCoordinator() {
		if (coordinator == null) {
			DontDestroyOnLoad(gameObject);
			coordinator = this;
		} else if (coordinator != this){
			Destroy(gameObject);
		}
	}

	// Publish to broker
	public void Publish(string PublishTopic, string PublishMsg, Boolean retainMsg){
		Debug.Log ("Publishing to: " + PublishTopic.Replace (" ", "").ToLower ());
		daveClient.Publish(PublishTopic.Replace(" ", "").ToLower(), PublishMsg, retainMsg);
	}

	// Subscribing to broker
	public void Subscribe(string SubscribeTopic){
		Debug.Log ("Suscribing to: " + SubscribeTopic.Replace (" ", "").ToLower ());
		daveClient.Subscribe(SubscribeTopic.Replace(" ", "").ToLower());
	}

	public MqttClient GetMqttClient(){
		return this.client;
	}

	public MqttClientDAVE GetDaveClient(){
		return this.daveClient;
	}


	public void EstablishConnection()
	{

		// Creates a MqttClientDAVE with the following credentials
		// Change IP when deployed to AWS.
		this.daveClient = new MqttClientDAVE("18.216.88.162", 1883, System.Guid.NewGuid().ToString());

		this.client = this.daveClient.GetMqttClient();
		// Assign handler for handling the receiving messages
		this.daveClient.GetMqttClient().MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

	}

	void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
	{
		Debug.Log("Received\r\n" + "Topic: " + e.Topic + "\r\n" + "Message: " +
			System.Text.Encoding.UTF8.GetString(e.Message));

		CheckReceived(e);
	}

	// Calls helper methods to check the received message
	private void CheckReceived(MqttMsgPublishEventArgs e)
	{
		State(e);
		Processes(e);
		Diagram(e);
		Students(e);
		Instructors(e);
	}

	void State(MqttMsgPublishEventArgs e)
	{
		if (e.Topic == parentTopic + "/state" && System.Text.Encoding.UTF8.GetString(e.Message) == "start")
			Debug.Log("State: " + System.Text.Encoding.UTF8.GetString(e.Message));
		//Pause animation and simulation

		else if (e.Topic == parentTopic + "/state" && System.Text.Encoding.UTF8.GetString(e.Message) == "pause")
			Debug.Log("State: " + System.Text.Encoding.UTF8.GetString(e.Message));
		//Pause animation and simulation

	}

	void Processes(MqttMsgPublishEventArgs e)
	{
		if (e.Topic == parentTopic + "/processes")
		{
			Debug.Log("Processes: " + System.Text.Encoding.UTF8.GetString(e.Message));
			//Render systemboxes
		}
	}

	void Diagram(MqttMsgPublishEventArgs e)
	{
		if (e.Topic == parentTopic + "/diagram")
		{
			Debug.Log("Messages: " + System.Text.Encoding.UTF8.GetString(e.Message));
			//Render message
		}
	}

	void Students(MqttMsgPublishEventArgs e)
	{
		if (e.Topic == parentTopic + "/students")
		{
			Debug.Log("Students: " + System.Text.Encoding.UTF8.GetString(e.Message));
			//Add student name to scene
		}
	}

	void Instructors(MqttMsgPublishEventArgs e){
		//if(e.topic == "instructors"){
		//  Debug.Log("Instructors: " + System.Text.Encoding.UTF8.GetString(e.Message));
		// Add instructors to Scene.
		//}
	}

	private void RenderSystemBoxes(JSONSequence JSONSequence)
	{
		uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
	}

	private void RenderMessages(JSONSequence JSONSequence)
	{
		uplButton.GetComponent<StartMessages>().NewMessage(JSONSequence);
	}

	// get/set methods added: 
	public void SetInstructor (string instructor)
	{
		this.instructor = instructor;
	}
	public string GetInstructor ()
	{
		return this.instructor;
	}
	public void SetDiagram (string diagram)
	{
		this.diagram = diagram;
	}
	public string GetDiagram ()
	{
		return this.diagram;
	}
	public void SetStudent (string student)
	{
		this.student = student;
	}
	public string GetStudent ()
	{
		return this.student;
	}
	public void SetSessionJson (string json)
	{
		this.sessionJSON = json;
	}
	public string GetSessionJson ()
	{
		return this.sessionJSON;
	}
	public void SetDiagramType (string type)
	{
		this.diagramType = type;
	}
	public string GetDiagramType ()
	{
		return this.diagramType;
	}
	public void SetInstructorBool (bool instructorBool) 
	{
		this.instructorBool = instructorBool;
	}
	public bool GetInstructorBool ()
	{
		return this.instructorBool;
	}

}
