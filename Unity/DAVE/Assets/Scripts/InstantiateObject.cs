using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    void Start()
    {
        // Creates new ParallelBox by loading it from the "Resources" folder
        // Add if statement in the future to check whether or not "par" and "seq" nodes exists
        GameObject parBox = (GameObject)Instantiate(Resources.Load("ParallelBox"));
        parBox.SetActive(true);
    }
}
