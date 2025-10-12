using UnityEngine;
using UnityEngine.UIElements;

public class TagsManager : MonoBehaviour
{
    public GameObject tagsObject; // assign in Inspector

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button toggleButton = root.Q<Button>("tagBtn");
        Button finishBtn = root.Q<Button>("finishBtn");

        if (SceneData.showTapActPage)
        {
            HideTags();
        }

        // Instead of hiding go w/o confirmation, i moved the script in
        // _3dModeSceneManager.cs 94

        // finishBtn?.RegisterCallback<ClickEvent>(evt => HideTags());
        toggleButton.clicked += ToggleTagsVisibility;
    }

    private void HideTags()
    {
        if (tagsObject != null)
        {
            tagsObject.SetActive(false);
        }
    }

    private void ToggleTagsVisibility()
    {
        if (tagsObject != null)
        {
            tagsObject.SetActive(!tagsObject.activeSelf);
        }
    }
}
