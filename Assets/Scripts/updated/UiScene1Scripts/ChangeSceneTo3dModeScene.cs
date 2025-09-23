using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneChangerGoToSampleScene : MonoBehaviour
{
    // This will be moved to IntegrateUI
    // Because we want it to be dynamic (pass a topic id or name)
    // to display what the user clicked

    // private void OnEnable()
    // {
    //     var ui = GetComponent<UIDocument>();
    //     if (ui == null)
    //         return;

    //     var root = ui.rootVisualElement;
    //     var _3dModeBtn = root.Q<Button>("3dModeBtn");

    //     if (_3dModeBtn != null)
    //     {
    //         _3dModeBtn.clicked += () =>
    //         {
    //             SceneManager.LoadScene("3dModeSkeletalScene");
    //         };
    //     }
    //     else
    //     {
    //         Debug.LogWarning("startButton not found in the UI.");
    //     }
    // }
}
