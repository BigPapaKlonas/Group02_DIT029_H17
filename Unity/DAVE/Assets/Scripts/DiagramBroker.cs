using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiagramBroker
{
    private Button uplButton;

    public DiagramBroker(string nJson, float houseOffset)
    {
        // Creates a JsonParser for the parsing.
        JsonParser jsonParser = new JsonParser(nJson);
        uplButton = GameObject.Find("Upload").GetComponent<Button>();

        Debug.Log("Diagram type: " + jsonParser.GetDiagramType());

        //ConnectionManager.coordinator.SetDiagramType(jsonParser.GetDiagramType());

        Debug.Log(nJson);
        
        /*
        nJson = jsonParser.AddMetaToSequence("root/" +
            ConnectionManager.coordinator.GetInstructor().Replace(" ", "").ToLower() + "/" +
            ConnectionManager.coordinator.GetDiagram().Replace(" ", "").ToLower()
        );
        **/

        // ConnectionManager.coordinator.SetSessionJson(nJson);

        // Insert();

        if (SceneManager.GetActiveScene().name == "Start")
        {
            SceneManager.LoadScene("class_diagram");
        }

        switch (jsonParser.GetDiagramType())
        {
            case "sequence_diagram":
                Debug.Log("Sequence");
                RenderSequence(jsonParser.ParseSequence());
                break;
            case "class_diagram":
                Debug.Log("Class");
                RenderClassDiagram(jsonParser.ParseClass(), houseOffset);
                break;
            case "deployment_diagram":
                Debug.Log("deployment");
                RenderDeployment(jsonParser.ParseDeployment());
                break;
            default:
                Debug.Log("Invalid diagram type");
                break;
        }
    }

    public void RenderSequence(JSONSequence JSONSequence)
    {
        RenderSystemBoxes(JSONSequence);
        RenderMessages(JSONSequence);
    }

    public void RenderClassDiagram(JSONClass JSONClass, float houseOffset)
    {
        string id = Guid.NewGuid().ToString("N");
        RenderClasses(JSONClass, id, houseOffset);
        RenderRelationships(JSONClass, id);
    }

    public void RenderDeployment(JSONDeployment JSONDeployment)
    {
        //Placeholder
    }

    private void RenderSystemBoxes(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(JSONSequence);
    }

    private void RenderMessages(JSONSequence JSONSequence)
    {
        uplButton.GetComponent<StartMessages>().NewMessage(JSONSequence);
    }

    public void RenderClasses(JSONClass JSONClass, string id, float offset)
    {
        uplButton.GetComponent<RenderClasses>().AddHouse(JSONClass, id, offset);
    }

    public void RenderRelationships(JSONClass JSONClass, string id)
    {
        uplButton.GetComponent<RenderClassRelationship>().AddRelationship(JSONClass, id);
    }

    /* 
     * Insert to the Database.
     * when ran in a Coroutine The full ConnectionManager.<variable> needs to be present.
     */
    void Insert()
    {

        string instructor = ConnectionManager.coordinator.GetInstructor();
        string diagram = ConnectionManager.coordinator.GetRoom();
        string diagramType = ConnectionManager.coordinator.GetDiagramType();

        /* 
		 * If database contains the instructor name: 
		 * Update instructors.diagrams
		 * and add the diagram to diagrams table.
		 * Else:
		 * Add both to instructors and diagrams tables.
		*/
        if (ConnectionManager.R.Db("root").Table("instructors").GetField("name")
            .Contains(instructor).Run(ConnectionManager.conn))
        {
            ConnectionManager.R.Db("root")
                .Table("diagrams").Insert(ConnectionManager.R.Array(
                    ConnectionManager.R.HashMap("name", diagram)
                    .With("type", diagramType)
                    .With("instructor", instructor)
                ))
                .Run(ConnectionManager.conn);

            ConnectionManager.R.Db("root")
                .Table("instructors")
                .Filter(row => row.G("name").Eq(instructor))
                .Update(ConnectionManager.R.HashMap("diagrams", ConnectionManager.R.Array(diagram)))
                .Run(ConnectionManager.conn);

        }
        else
        {

            ConnectionManager.R.Db("root")
                .Table("diagrams").Insert(ConnectionManager.R.Array(
                    ConnectionManager.R.HashMap("name", diagram)
                    .With("type", diagramType)
                    .With("instructor", instructor)
                ))
                .Run(ConnectionManager.conn);

            ConnectionManager.R.Db("root")
                .Table("instructors").Insert(ConnectionManager.R.Array(
                    ConnectionManager.R.HashMap("name", instructor)
                    .With("diagrams", ConnectionManager.R.Array())
                )
                )
                .Run(ConnectionManager.conn);
        }
    }
}

