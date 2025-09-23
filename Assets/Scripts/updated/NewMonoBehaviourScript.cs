using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SceneChangerTT : MonoBehaviour
{
    private void OnEnable()
    {
        var ui = GetComponent<UIDocument>();
        if (ui == null) return;

        var root = ui.rootVisualElement;
        var startButton = root.Q<Button>("startButton");

        if (startButton != null)
        {
            startButton.clicked += () =>
            {
                SceneManager.LoadScene("Scene2");
            };
        }
        else
        {
            Debug.LogWarning("startButton not found in the UI.");
        }
    }
}
