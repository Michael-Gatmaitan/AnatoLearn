using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class FunFactsCarousel : MonoBehaviour
{
    private VisualElement root;
    private VisualElement cardsContainer;
    private List<VisualElement> cards = new();
    private int currentCardIndex = 0;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        cardsContainer = root.Q<VisualElement>("funFactsCardsCon");

        // Clear in case of re-entry
        cards.Clear();

        foreach (var card in cardsContainer.Children())
        {
            cards.Add(card);
            card.style.display = DisplayStyle.None; // hide all at start

            // Find per-card buttons inside the card
            var prevBtn = card.Q<Button>("ffPrevBtn");
            var nextBtn = card.Q<Button>("ffNextBtn");

            if (prevBtn != null)
                prevBtn.RegisterCallback<ClickEvent>(evt => ShowPreviousCard());

            if (nextBtn != null)
                nextBtn.RegisterCallback<ClickEvent>(evt => ShowNextCard());
        }

        UpdateCardVisibility();
    }

    private void ShowNextCard()
    {
        currentCardIndex = (currentCardIndex + 1) % cards.Count;
        UpdateCardVisibility();
    }

    private void ShowPreviousCard()
    {
        currentCardIndex = (currentCardIndex - 1 + cards.Count) % cards.Count;
        UpdateCardVisibility();
    }

    private void UpdateCardVisibility()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].style.display = (i == currentCardIndex) ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
