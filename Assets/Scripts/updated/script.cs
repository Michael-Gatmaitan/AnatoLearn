using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void OnEnable()
    {
        var ui = GetComponent<UIDocument>();
        if (ui == null) return;

        var root = ui.rootVisualElement;
        var Button = root.Q<Button>("goBack");

        if (Button != null)
        {
            Button.clicked += () =>
            {
                SceneManager.LoadScene("SplashScreen");
            };
        }
        else
        {
            Debug.LogWarning("startButton not found in the UI.");
        }
    }
}
