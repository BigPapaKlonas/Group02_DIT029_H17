using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowhead : MonoBehaviour {

    public Vector3 ArrowOrigin;
    public Vector3 ArrowTarget;
    private LineRenderer cachedLineRenderer;
    
    void Start(){

        
        //cachedLineRenderer = this.GetComponent<LineRenderer>();
        //cachedLineRenderer.widthCurve = new AnimationCurve(
        //    new Keyframe(0, 0.4f)
        //    , new Keyframe(0.9f, 0.4f) // neck of arrow
        //    , new Keyframe(0.91f, 1f)  // max width of arrow head
        //    , new Keyframe(1, 0f));  // tip of arrow
        //cachedLineRenderer.SetPositions(new Vector3[] {
        //     ArrowOrigin
        //     , Vector3.Lerp(ArrowOrigin, ArrowTarget, 0.9f)
        //     , Vector3.Lerp(ArrowOrigin, ArrowTarget, 0.91f)
        //     , ArrowTarget });
        //cachedLineRenderer = this.GetComponent<LineRenderer>();
        //cachedLineRenderer.startWidth = 1f;
        //cachedLineRenderer.endWidth = 0f;

    }
	// Update is called once per frame
	void Update () {
		
	}
}
