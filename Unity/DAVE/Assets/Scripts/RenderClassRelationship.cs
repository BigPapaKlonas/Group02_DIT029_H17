using UnityEngine;

public class RenderClassRelationship : Pathfinding {

    public GameObject roadPiecePrefab;
    public Material mat1, mat2, mat3;

    public void AddRelationship(JSONClass json)
    {

        foreach (var relationship in json.Relationships)
        {
            GameObject sub = GameObject.Find(relationship.Subclass);
            GameObject sup = GameObject.Find(relationship.Superclass);

            Vector3 subDoor = new Vector3(
                sub.transform.Find("BottomFrontDoor").position.x,
                0,
                sub.transform.Find("BottomFrontDoor").position.z);

            Vector3 supDoor = new Vector3(
                sup.transform.Find("BottomFrontDoor").position.x,
                0,
                sup.transform.Find("BottomFrontDoor").position.z);

            Vector3 infrontSub = new Vector3(
                subDoor.x - 2,
                0,
                subDoor.z);

            Vector3 infrontSup = new Vector3(
               supDoor.x - 2,
               0,
               supDoor.z);

            Road road = sub.GetComponentInChildren<Road>();
            road.start = supDoor;
            road.end = subDoor;
        }
    }

    void SetRelationshipType(LineRenderer road, string type)
    {
        if (type.Equals("composition"))
        {
            road.material = mat1;
        }

        else if (type.Equals("inheritance") | type.Equals("generalization"))
        {
            road.material = mat2;
        }
        else
        {
            road.material = mat3;
        }
        road.textureMode = LineTextureMode.Stretch;
        road.material.SetTextureScale("_MainTex", new Vector2(1.0f, 1.0f));
    }
}

