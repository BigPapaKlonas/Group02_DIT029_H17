using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Pathfinding
{
    public GameObject roadPiecePrefab;
    static List<Vector3> paths = new List<Vector3>();
    Vector3 start = new Vector3(-200, -200, -200);
    Vector3 end = new Vector3(-200, -200, -200);
    string device = "havana";

    public void SetStartPosition(Vector3 startPosition)
    {
        this.start = startPosition;
    }

    public void SetEndPosition(Vector3 endPosition)
    {
        this.end = endPosition;
    }

    public void SetDevice(string Device)
    {
        this.device = Device;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.O))
        {
            FindPath(start, end);
        }
        FindPath(start, end);


        for (int i = 0; i < Path.Count - 1; i++)
        {
            Debug.DrawLine(Path[i], Path[i + 1], Color.red);
            GameObject classHouse = (GameObject)Instantiate(
                roadPiecePrefab,
                Path[i],
                this.transform.rotation
            );
            RotateRoadPiece(Path[i], Path[i + 1], classHouse);
        }
    
        paths = Path;
        Debug.Log("to string in update " + paths.ToString());
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

    public List<Vector3> GetPaths()
    {
        Debug.Log("to string in Getter " + paths.ToString());
        Debug.Log(paths.Count);
        return paths;
    }

    public void SetPaths(List<Vector3> newPath)
    {
        paths = newPath;
    }

    /*
    private void LateUpdate()
    {

        for (int i = 0; i < Path.Count - 1; i++)
        {
            Debug.DrawLine(paths[i], paths[i + 1], Color.red);
            GameObject classHouse = (GameObject)Instantiate(
                roadPiecePrefab,
                paths[i],
                this.transform.rotation
            );
            RotateRoadPiece(paths[i], paths[i + 1], classHouse);
        }
    }**/
}
