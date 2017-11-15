using UnityEngine;

public class RenderClassRelationship : Pathfinding {

    public void AddRelationship(JSONClass json)
    {

        foreach (var relationship in json.Relationships)
        {
            GameObject sub = GameObject.Find(relationship.Subclass);
            GameObject sup = GameObject.Find(relationship.Superclass);

            Transform subDoor = sub.transform.Find("Door");
            Transform supDoor = sup.transform.Find("Door");

            Road road = sub.GetComponentInChildren<Road>();
            road.start = supDoor.position;
            road.end = subDoor.position;
            road.relationshipType = relationship.Type;
        }
    }
}

