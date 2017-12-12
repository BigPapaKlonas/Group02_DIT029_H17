using UnityEngine;
using System.Collections.Generic;

// Creates the road from and to houses
public class RenderRoad : Pathfinding
{
    // Boolean to keep track if road has been made
    private bool roadMade = false;
    // List of RoadPiece objects that make up a road/path
    private List<GameObject> roadPieces = new List<GameObject>();

    public GameObject roadPiecePrefab;  // RoadPiece prefab
    public Vector3 startPos;            // Road Starting position
    public Vector3 endPos;              // Road ending position 
    public string relationshipType;     // Type of relationship

    // Materials for the different relationship types
    public Material aggregation;
    public Material association;
    public Material directedAssociation;
    public Material composition;
    public Material generalization;
    public Material realization;

    private void Update()
    {
        // Asynchronous method from the Pathfinding class
        // Creates a Vector3 list called Path by adding points from startPos to endPos
        FindPath(startPos, endPos);
       
        // Waiting until FindPath return 
        if(Path.Count > 0)
        {
            // Only add new RoadPiece objects if road hasn't been made yet
            if (!roadMade)
            {
                // Create a new RoadPiece object for every Vector3 point in the Path
                foreach(var pathPoint in Path)
                {
                    GameObject roadObject = Instantiate(
                        roadPiecePrefab,
                        pathPoint,
                        transform.rotation
                    );
                   
                    // Add game object to List
                    roadPieces.Add(roadObject);
                }

                // Set to true, to stop from foreach loop executing againg
                roadMade = true;
            }

            // Call function to rotate all the GameObject in list towards each other
            RotateRoadPieces(roadPieces);
        }
    }

    /*
     * Rotates the individual RoadPieces towards their upcoming neighbour
     **/
    void RotateRoadPieces(List<GameObject> roadPieces)
    {
        // For loop used to get the current and the upcoming GameObject from a list
        for (int i = 0; i < roadPieces.Count - 1; i++)
        {
            // Rotate current RoadPiece object towards the next one in a list
            roadPieces[i].transform.LookAt(roadPieces[i + 1].transform);

            // Call function to change the material for the current RoadPiece
            SetRelationshipType(roadPieces[i]);
        }

        // Setting  relationship type for the last road piece
        SetRelationshipType(roadPieces[roadPieces.Count - 1]);
    }

    /*
     * Changes RoadPiece object material based on the type of relationship
     **/
    void SetRelationshipType(GameObject roadPiece)
    {
        if (relationshipType.Equals("composition"))
        {
            roadPiece.GetComponent<Renderer>().material = composition;
        }
        else if (relationshipType.Equals("aggregation"))
        {
            roadPiece.GetComponent<Renderer>().material = aggregation;
        }
        else if (relationshipType.Equals("inheritance") ||
            relationshipType.Equals("generalization"))
        {
            roadPiece.GetComponent<Renderer>().material = generalization;
        }
        else if (relationshipType.Equals("directed_association"))
        {
            roadPiece.GetComponent<Renderer>().material = directedAssociation;
        }
        else if (relationshipType.Equals("realization"))
        {
            roadPiece.GetComponent<Renderer>().material = realization;

        }
        else
        {
            roadPiece.GetComponent<Renderer>().material = association;
        }
    }
}
