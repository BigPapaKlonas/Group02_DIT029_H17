using UnityEngine;

public class RenderMessageArrowhead : MonoBehaviour {
    
    private LineRenderer cachedLineRenderer;
   
    // Use this for initialization
    void Start () {
        cachedLineRenderer = this.GetComponent<LineRenderer>();
        cachedLineRenderer.SetPosition(0, new Vector3());
        cachedLineRenderer.SetPosition(1, new Vector3());
    }

    public void changePos(bool left, Vector3 newPos) {
        if (left) {
            cachedLineRenderer.widthCurve = new AnimationCurve(
                new Keyframe(0, 0f)
                , new Keyframe(0.9f, 0.4f) // neck of arrow
                , new Keyframe(0.91f, 1f)  // max width of arrow head
                , new Keyframe(1, 0.4f));
            Vector3 start = new Vector3(newPos.x, newPos.y, newPos.z - 0.2f);
            Vector3 end = new Vector3(newPos.x, newPos.y, newPos.z + 1.1f);
            cachedLineRenderer.SetPositions(new Vector3[] {
                    start
                    , Vector3.Lerp(start, end, 0.9f)
                    , Vector3.Lerp(start, end, 0.91f)
                    , end });
        }else{
            cachedLineRenderer.widthCurve = new AnimationCurve(
               new Keyframe(0, 0.4f)
               , new Keyframe(0.9f, 0.4f) // neck of arrow
               , new Keyframe(0.91f, 1f)  // max width of arrow head
               , new Keyframe(1, 0f));  // tip of arrow
            Vector3 start = new Vector3(newPos.x, newPos.y, newPos.z - 1f);
            Vector3 end = new Vector3(newPos.x, newPos.y, newPos.z + 0.3f);
            cachedLineRenderer.SetPositions(new Vector3[] {
                    start
                    , Vector3.Lerp(start, end, 0.9f)
                    , Vector3.Lerp(start, end, 0.91f)
                    , end });
        }
    }
}
