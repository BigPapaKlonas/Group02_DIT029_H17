using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public GameObject playerTransform;

    void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Move camera right (relative to where it's facing)
            playerTransform.transform.position += playerTransform.transform.right * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            // Move camera left (relative to where it's facing) by inverting .right
            playerTransform.transform.position -= playerTransform.transform.right * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Move camera forward (relative to where it's facing)
            playerTransform.transform.position += playerTransform.transform.forward * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Move camera back (relative to where it's facing) by inverting .forward
            playerTransform.transform.position -= playerTransform.transform.forward * 2.5f * Time.deltaTime;
        }
    }
}
