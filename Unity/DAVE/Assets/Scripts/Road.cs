using UnityEngine;
using System.Collections.Generic;

public class Road : Pathfinding
{
    private bool pathMade = false;
    private List<GameObject> roadPieces = new List<GameObject>();

    public GameObject roadPiecePrefab;
    public Vector3 start;
    public Vector3 end;
    public string relationshipType;

    public Material composition;
    public Material inheritance;
    public Material street;


    public string subclass;


    private void Update()
    {
        FindPath(start, end);
       
        if(Path.Count > 0)
        {
            if (!pathMade)
            {
                Debug.Log(Path.Count);
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    GameObject roadObject = (GameObject)Instantiate(
                        roadPiecePrefab,
                        Path[i],
                        transform.rotation
                    );

                    roadObject.SetActive(true);
                    roadPieces.Add(roadObject);
                }
                pathMade = true;
            }
            RotateRoadPieces(roadPieces);
        }
    }

    void RotateRoadPieces(List<GameObject> roadPieces)
    {
        for (int i = 0; i < roadPieces.Count - 1; i++)
        {
            roadPieces[i].transform.LookAt(roadPieces[i + 1].transform);

            SetRelationshipType(roadPieces[i]);
        }
    }

    void SetRelationshipType(GameObject roadPiece)
    {
        if (relationshipType.Equals("composition"))
        {
            roadPiece.GetComponent<Renderer>().material = composition;
        }
        else if (relationshipType.Equals("aggregation"))
        {
            // Texture making in progress
        }
        else if (relationshipType.Equals("inheritance") ||
            relationshipType.Equals("generalization"))
        {
            roadPiece.GetComponent<Renderer>().material = inheritance;
            
        }
        else if (relationshipType.Equals("association"))
        {
            // Texture making in progress
        }
        else if (relationshipType.Equals("realization"))
        {
            // Texture making in progress
        }
        else
        {
            roadPiece.GetComponent<Renderer>().material = street;
        }
    }
}
