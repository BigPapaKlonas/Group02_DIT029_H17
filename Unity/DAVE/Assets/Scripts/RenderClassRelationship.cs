using UnityEngine;

public class RenderClassRelationship : Pathfinding {

    public void AddRelationship(JSONClass json)
    {
        // Iterate over each relationship "packet" within JSON
        foreach (var relationship in json.Relationships)
        {
            // SubClass GameObject
            GameObject sub = GameObject.Find(relationship.Subclass);
            
            // SuperClass GameObject
            GameObject sup = GameObject.Find(relationship.Superclass);

            // Finding the Transform of the Doors to used as position for Road making
            Transform subDoor = sub.transform.Find("Door");
            Transform supDoor = sup.transform.Find("Door");

            // Getting an instance of the script responsible for making Road
            RenderRoad road = sub.GetComponentInChildren<RenderRoad>();
            road.startPos = supDoor.position;
            road.endPos = subDoor.position;
            road.relationshipType = relationship.Type;
        }
    }
}

