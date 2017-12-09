using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using System;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using System.Collections.Generic;

public class ConnectionManager : MonoBehaviour
{
    private Button uplButton;

    private MqttClient client;
    private MqttClientDAVE daveClient;
	public static ConnectionManager coordinator;

	/* 
	 * Creation of a instance of our database and the connection to it to be used in our classes.
	 * The reason behind calling the RethinkDb R is that it is a convention for the RethinkDb database. 
	 * makes it easier for us to follow along in tutorials and documentation.
	 */
	public static RethinkDB R;
	public static Connection conn;

	/* 
	 * Private variables to hold onto data 
	 * during the lifecycle of the application.
	 */
	private string instructor;
	private string room;
    private string student;
	private string roomType;
	private bool instructorBool;
    private string parentTopic;
    private Queue<JsonObject> selectedJSONS = new Queue<JsonObject>();

    public struct JsonObject        // JsonObject structure  
    {
        public string json;
        public string diagramType;
    }

    /*
     * Authentication for instructor.
     */
    public static bool auth;


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

	void Awake(){
		MakeThisTheOnlyCoordinator();
            
        // Saving player and DAVEPathfinder to avoid errors when loading CD 
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Map"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Terrain"));
    }

	void MakeThisTheOnlyCoordinator() {
		if (coordinator == null) {
			DontDestroyOnLoad(gameObject);
            coordinator = this;
		} else if (coordinator != this){
			Destroy(gameObject);
		}
	}

	/*
	 * Publish to broker
	 */
	public void Publish(string PublishTopic, string PublishMsg, Boolean retainMsg){
		Debug.Log ("Publishing to: " + PublishTopic.Replace (" ", "").ToLower ());
		daveClient.Publish(PublishTopic.Replace(" ", "").ToLower(), PublishMsg, retainMsg);
	}

	/*
	 * Subscribe to broker
	 */
	public void Subscribe(string SubscribeTopic){
		Debug.Log ("Suscribing to: " + SubscribeTopic.Replace (" ", "").ToLower ());
		daveClient.Subscribe(SubscribeTopic.Replace(" ", "").ToLower());
	}

    /*
    * Unsubscribe to topic
    */
    public void Unsubscribe(string UnsubscribeTopic)
    {
        Debug.Log("Unsubscribed from topic: " + UnsubscribeTopic.Replace(" ", "").ToLower());
        daveClient.Unsubscribe(UnsubscribeTopic.Replace(" ", "").ToLower());
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
        this.daveClient = new MqttClientDAVE("13.59.108.164", 1883, Guid.NewGuid().ToString());

        this.client = this.daveClient.GetMqttClient();
    }


    /* 
	 * get/set methods 
	 */
    public void SetInstructor (string instructor)
	{
		this.instructor = instructor;
        UpdateParentTopic();
    }
    public string GetInstructor ()
	{
		return this.instructor;
	}
	public void SetRoom (string room)
	{
		room.Replace(" ", "").ToLower();
		this.room = room;
        UpdateParentTopic();
    }
    public string GetRoom ()
	{
		return this.room;
	}
	public void SetStudent (string student)
	{
		this.student = student;
    }
    public string GetStudent ()
	{
		return this.student;
	}
	public void AddSelectedJson (string json)
	{
        selectedJSONS.Enqueue(
            new JsonObject()
            {
                json = json,
                diagramType = new JsonParser(json).GetDiagramType() // Gets the diagram type
            });
    }
    public Queue<JsonObject> GetSelectedJsons ()
	{
		return selectedJSONS;
	}
    public string GetDiagramType ()
	{
		return this.roomType;
	}
	public void SetInstructorBool (bool instructorBool) 
	{
		this.instructorBool = instructorBool;
    }
	public bool GetInstructorBool ()
	{
		return this.instructorBool;
	}
    public string GetParentTopic()
    {
        return parentTopic;
    }

    //Updates the ParentTopic when either the room or the instructor are updated
    private void UpdateParentTopic ()
    {
        if (coordinator.GetInstructor() != null && coordinator.GetRoom() != null)
        {
            parentTopic = "root/" + coordinator.GetInstructor().ToLower() + "/"
                        + coordinator.GetRoom().ToLower();
        }
    }
}
