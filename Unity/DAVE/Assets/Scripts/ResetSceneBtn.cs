using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script simply loads the current scene, thus 'resets' the scene
/// </summary>
public class ResetSceneBtn : MonoBehaviour
{

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
