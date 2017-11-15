using UnityEngine;
using System.Collections.Generic;

public class Road : Pathfinding
{
    public GameObject roadPiecePrefab;
    public Vector3 start;
    public Vector3 end;
    private bool pathMade = false;
    List<GameObject> roadPieces = new List<GameObject>();

    private void Update()
    {
        FindPath(start, end);
       
        if(Path.Count > 0)
        {
            if (!pathMade)
            {
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    GameObject roadObject = (GameObject)Instantiate(
                        roadPiecePrefab,
                        Path[i],
                        transform.rotation
                    );
                    roadPieces.Add(roadObject);
                }
                pathMade = true;
            }
            RotateRoadPieces(roadPieces);
        }
    }

    public void Start()
    {
        pathMade = false;
    }


    void RotateRoadPieces(List<GameObject> roadPieces)
    {
        for (int i = 0; i < roadPieces.Count - 1; i++)
        {
            roadPieces[i].transform.LookAt(roadPieces[i + 1].transform);
        }
    }
}
