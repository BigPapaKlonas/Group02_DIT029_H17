using UnityEngine;

public class Road : Pathfinding
{
    public GameObject roadPiecePrefab;
    public Vector3 start;
    public Vector3 end;
    private bool pathMade = false;

    private void Update()
    {
        FindPath(start, end);
       
        if(Path.Count > 0)
        {
            if (!pathMade)
            {
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    GameObject classHouse = (GameObject)Instantiate(
                        roadPiecePrefab,
                        Path[i],
                        this.transform.rotation
                    );
                    RotateRoadPiece(Path[i], Path[i + 1], classHouse);
                }
                pathMade = true;
            }
        }
    }

    public void Start()
    {
        pathMade = false;
    }


    void RotateRoadPiece(Vector3 currentPiece, Vector3 nextPiece, GameObject roadPiece)
    {
        if(currentPiece == start)
        {
            roadPiece.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (currentPiece == end)
        {
            roadPiece.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if(currentPiece.x < nextPiece.x)
        {
            roadPiece.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(currentPiece.z < nextPiece.z)
        {
            roadPiece.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
