using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowhead : MonoBehaviour {
    
    private LineRenderer cachedLineRenderer;
   
    // Use this for initialization
    void Start () {

        

        cachedLineRenderer = this.GetComponent<LineRenderer>();
        cachedLineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f)
            , new Keyframe(0.9f, 0.4f) 
            , new Keyframe(0.91f, 1f)  
            , new Keyframe(1, 0f));  
        
    }
    
	
	// Update is called once per frame
	void Update () {
        
        
    }

    public void changePos(Vector3 newPos) {
        
        Vector3 start = new Vector3(newPos.x, newPos.y, newPos.z - 1f);
        Vector3 end = new Vector3(newPos.x, newPos.y, newPos.z + 0.3f);
        cachedLineRenderer.SetPositions(new Vector3[] {
                start
                , Vector3.Lerp(start, end, 0.9f)
                , Vector3.Lerp(start, end, 0.91f)
                , end });
    }
}
