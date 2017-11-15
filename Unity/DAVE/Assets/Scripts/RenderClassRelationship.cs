using UnityEngine;

public class RenderClassRelationship : Pathfinding {

    public void AddRelationship(JSONClass json)
    {

        foreach (var relationship in json.Relationships)
        {
            GameObject sub = GameObject.Find(relationship.Subclass);
            GameObject sup = GameObject.Find(relationship.Superclass);

            Vector3 subDoor = sub.transform.Find("BottomFrontDoor").transform.position;
            Vector3 supDoor = sup.transform.Find("BottomFrontDoor").transform.position;

            Road road = sub.GetComponentInChildren<Road>();
            road.start = supDoor;
            road.end = subDoor;
            road.relationshipType = relationship.Type;
        }
    }
}

