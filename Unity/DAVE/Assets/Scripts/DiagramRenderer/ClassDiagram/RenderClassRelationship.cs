using UnityEngine;

public class RenderClassRelationship : MonoBehaviour {

    public void AddRelationship(JSONClass json, string uniqueId)
    {
        // Iterate over each relationship "packet" within JSON
        foreach (var relationship in json.Relationships)
        {
            
            // SubClass GameObject
            GameObject sub = GameObject.Find(relationship.Subclass + uniqueId);
            
            // SuperClass GameObject
            GameObject sup = GameObject.Find(relationship.Superclass + uniqueId);

            // Finding the Transform of the Doors to used as position for Road making
            Transform subDoor = sub.transform.Find("BottomFrontDoor");
            Transform supDoor = sup.transform.Find("BottomFrontDoor");

            // Getting an instance of the script responsible for making Road
            RenderRoad road = sub.GetComponentInChildren<RenderRoad>();
            road.startPos = subDoor.position;
            road.endPos = supDoor.position;
            road.relationshipType = relationship.Type;
        }
    }
}

