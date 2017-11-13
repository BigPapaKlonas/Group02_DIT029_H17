using UnityEngine;

public class RenderClassRelationship : MonoBehaviour {

    public void AddRelationship(JSONClass json)
    {
        foreach (var relationship in json.Relationships)
        {
            GameObject sub = GameObject.Find(relationship.Subclass);
            GameObject dom = GameObject.Find(relationship.Superclass);

          
            
        }
    }
}
