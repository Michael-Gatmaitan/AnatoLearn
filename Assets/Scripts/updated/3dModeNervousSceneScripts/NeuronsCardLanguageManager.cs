using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NeuronsCardLanguageManager : MonoBehaviour
{
    private Dictionary<string, VisualElement> neuronsCards = new();

    void Start()
    {
        var uiDoc = Object.FindFirstObjectByType<UIDocument>();
        if (uiDoc != null)
        {
            var root = uiDoc.rootVisualElement;

            // Register each fun facts card from funFactsCard1 to funFactsCard10
            for (int i = 1; i <= 4; i++)
            {
                string cardName = $"neuronsCard{i}";
                var card = root.Q<VisualElement>(cardName);
                if (card != null)
                    neuronsCards[cardName] = card;
            }

            // Default language
            SetLanguage("englishVersion");
        }
    }

    public void SetLanguage(string languageVersion)
    {
        foreach (var pair in neuronsCards)
        {
            string cardKey = pair.Key; // e.g. "funFactsCard1"
            VisualElement card = pair.Value;

            Label neuronName = card.Q<Label>("neuronNameText");
            Label description = card.Q<Label>("ffDescription");

            if (LocalizedText.NeuronsCardText.TryGetValue(cardKey, out var localized))
            {
                if (languageVersion == "tagalogVersion")
                {
                    if (neuronName != null)
                        neuronName.text = localized.tlNeuronName;
                    if (description != null)
                        description.text = localized.tlDescription;
                }
                else
                {
                    if (neuronName != null)
                        neuronName.text = localized.enNeuronName;
                    if (description != null)
                        description.text = localized.enDescription;
                }
            }
            else
            {
                Debug.LogWarning($"Missing localization for: {cardKey}");
            }
        }
    }
}
