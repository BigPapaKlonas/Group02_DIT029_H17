using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// This script simply loads the current scene, thus 'resets' the scene
/// </summary>
public class ResetSceneBtn : MonoBehaviour, IPointerDownHandler
{

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnPointerDown(PointerEventData eventData) { }

    private void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
