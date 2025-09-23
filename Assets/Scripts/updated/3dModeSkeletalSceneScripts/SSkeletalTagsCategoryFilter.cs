using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SSkeletalTagsCategoryFilter : MonoBehaviour
{
    private Dictionary<string, List<string>> categoryToBoneNames = new();
    private VisualElement root;
    private List<Transform> allTags = new();
    private bool allTagsVisible = true; // State for toggling

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Define category mappings
        categoryToBoneNames["LongBones"] = new() { "tagHumerus", "tagRadius", "tagUlna", "tagFemur", "tagTibia", "tagFibula" };
        categoryToBoneNames["ShortBones"] = new() { "tagCarpals", "tagTarsal" };
        categoryToBoneNames["FlatBones"] = new() { "tagSkull", "tagScapula", "tagSternum", "tagRibs" };
        categoryToBoneNames["IrregularBones"] = new() { "tagSpine", "tagCoccyx", "tagPelvicGirdle" };
        categoryToBoneNames["Axial"] = new() { "tagSkull", "tagSpine", "tagRibs", "tagSternum", "tagCoccyx" };
        categoryToBoneNames["Appendicular"] = new()
        {
            "tagClavicle", "tagScapula", "tagHumerus", "tagRadius", "tagUlna", "tagCarpals", "tagMetacarpals", "tagPhalange",
            "tagFemur", "tagPatella", "tagTibia", "tagFibula", "tagTarsal", "tagMetatarsal", "tagPelvicGirdle"
        };
        categoryToBoneNames["AllTags"] = new() {
            "tagClavicle", "tagScapula", "tagHumerus", "tagRadius", "tagUlna", "tagCarpals", "tagMetacarpals", "tagPhalange", "tagFemur", "tagPatella", "tagTibia",
            "tagFibula", "tagTarsal", "tagMetatarsal", "tagSkull", "tagSpine", "tagRib", "tagRibs", "tagSternum", "tagCoccyx", "tagPelvicGirdle"
        };

        // Collect all tag GameObjects
        var tagsContainer = GameObject.Find("TagsContainer").transform;
        foreach (Transform tag in tagsContainer)
        {
            allTags.Add(tag);
            Debug.Log("Collected Tag: " + tag.name);
        }

        // Register VisualElements (not Buttons)
        RegisterClickableVisual("longBonesBtn", "LongBones");
        RegisterClickableVisual("shortBonesBtn", "ShortBones");
        RegisterClickableVisual("flatBonesBtn", "FlatBones");
        RegisterClickableVisual("irregBonesBtn", "IrregularBones");
        RegisterClickableVisual("axialBonesBtn", "Axial");
        RegisterClickableVisual("appendBonesBtn", "Appendicular");

        // Special toggle for all tags
        var tagBtn = root.Q<VisualElement>("tagBtn");
        if (tagBtn != null)
        {
            tagBtn.RegisterCallback<ClickEvent>(evt =>
            {
                allTagsVisible = !allTagsVisible;
                Debug.Log("Toggling All Tags: " + allTagsVisible);
                foreach (Transform bone in allTags)
                {
                    bone.gameObject.SetActive(allTagsVisible);
                }
            });
        }
        else
        {
            Debug.LogWarning("tagBtn not found in UI");
        }

        //FOR TAP ME ACT PAGE SET UP - hiding / hide all the tags 
        // SceneData.showTapActPage = true;
        if (SceneData.showTapActPage)
        {
            //hide all tags, because you're now in TapMeAct page
            foreach (Transform bone in allTags)
            {
                bone.gameObject.SetActive(false);
            }
            // SceneData.showTapActPage = false;
        }

        Debug.Log("lastline of enable() in sketaltagcateggorfilter.cs");
    }

    void RegisterClickableVisual(string btnName, string categoryKey)
    {
        var ve = root.Q<VisualElement>(btnName);
        if (ve != null)
        {
            ve.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log($"Clicked: {btnName} => Category: {categoryKey}");
                ShowCategory(categoryKey);
                allTagsVisible = false; // Reset toggle when filtering
            });
        }
        else
        {
            Debug.LogWarning($"VisualElement not found in UI: {btnName}");
        }
    }

    void ShowCategory(string category)
    {
        if (!categoryToBoneNames.ContainsKey(category))
        {
            Debug.LogWarning("Category not found: " + category);
            return;
        }

        var visibleNames = categoryToBoneNames[category];
        Debug.Log($"Showing category: {category} — {visibleNames.Count} bones");

        foreach (Transform bone in allTags)
        {
            bool shouldShow = visibleNames.Contains(bone.name);
            bone.gameObject.SetActive(shouldShow);

            Debug.Log((shouldShow ? "Showing: " : "Hiding: ") + bone.name);
        }
    }
}
