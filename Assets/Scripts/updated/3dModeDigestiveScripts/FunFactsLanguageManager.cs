using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class FunFactsLanguageManager : MonoBehaviour
{
    private Dictionary<string, VisualElement> funFactCards = new();

    void Start()
    {
        var uiDoc = Object.FindFirstObjectByType<UIDocument>();
        if (uiDoc != null)
        {
            var root = uiDoc.rootVisualElement;

            // Register each fun facts card from funFactsCard1 to funFactsCard10
            for (int i = 1; i <= 10; i++)
            {
                string cardName = $"funFactsCard{i}";
                var card = root.Q<VisualElement>(cardName);
                if (card != null)
                    funFactCards[cardName] = card;
            }

            // Default language
            SetLanguage("englishVersion");
        }
    }

    public void SetLanguage(string languageVersion)
    {
        foreach (var pair in funFactCards)
        {
            string cardKey = pair.Key; // e.g. "funFactsCard1"
            VisualElement card = pair.Value;

            Label header = card.Q<Label>("ffHeaderText");
            Label description = card.Q<Label>("ffDescription");
            Label didYouKnowText = card.Q<Label>("didYouKnowText");

            if (LocalizedText.FunFacstsText.TryGetValue(cardKey, out var localized))
            {
                if (languageVersion == "tagalogVersion")
                {
                    if (header != null) header.text = localized.tlHeader;
                    if (description != null) description.text = localized.tlDescription;
                    if (didYouKnowText != null) didYouKnowText.text = localized.tlDidYouKnow;
                }
                else
                {
                    if (header != null) header.text = localized.enHeader;
                    if (description != null) description.text = localized.enDescription;
                    if (didYouKnowText != null) didYouKnowText.text = localized.enDidYouKnow;
                }
            }
            else
            {
                Debug.LogWarning($"Missing localization for: {cardKey}");
            }
        }
    }
}
