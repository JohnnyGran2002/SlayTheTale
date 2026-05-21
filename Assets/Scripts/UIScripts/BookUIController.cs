using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BookUIController : MonoBehaviour
{
    [SerializeField] private UIDocument ui;
    [SerializeField] private VisualTreeAsset cardTemplate;

    private VisualElement root;

    private readonly List<VisualElement> cardSlots = new ();
    private const int columns = 4;
    private const int rows = 2;
    private int currentRow = 0;
    private int currentColumn = 0;
    private int selectedIndex => currentRow * columns + currentColumn;

    private void Awake()
    {
        root = ui.rootVisualElement;

        for (int i = 0; i < 8; i++)
        {
            var slot = root.Q<VisualElement>($"CardSlot_{i + 1}");
            cardSlots.Add(slot);
        }
    }
    private void FitCardToSlot(VisualElement card, VisualElement slot)
    {
        float slotWidth = slot.resolvedStyle.width;
        float cardWidth = 200f; // din design width

        float scale = slotWidth / cardWidth;

        card.style.scale = new StyleScale(new Scale(new Vector2(scale, scale)));
    }

    public void FillBook(List<Card> cards)
    {
        for (int i = 0; i < 8; i++)
        {
            var slot = cardSlots[i];
            slot.Clear();

            if (i >= cards.Count)
                continue;

            var cardView = new CardView(cardTemplate);
            cardView.Bind(cards[i]);
            
            slot.Add(cardView);
            
            FitCardToSlot(cardView, slot);
        }
        UpdateSelectionVisual();
    }

    public void AddCardToSlot(Card card, int slotIndex)
    {
        var slot = root.Q<VisualElement>($"CardSlot_{slotIndex}");

        slot.Clear();

        var view = new CardView(cardTemplate);
        view.Bind(card);

        slot.Add(view);
    }

    public void NavigateCards(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Horisontellt
        if (input.x > 0.5f)
        {
            currentColumn++;
        }
        else if (input.x < -0.5f)
        {
            currentColumn--;
        }
        // Vertikalt
        if (input.y < -0.5f)
        {
            currentRow++;
        }
        else if (input.y > 0.5f)
        {
            currentRow--;
        }
        currentRow = Mathf.Clamp(currentRow, 0, rows - 1);
        currentColumn = Mathf.Clamp(currentColumn, 0, columns - 1);
        UpdateSelectionVisual();
    }
    private void UpdateSelectionVisual()
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            cardSlots[i].RemoveFromClassList("selectedCard");

            if (i == selectedIndex)
            {
                cardSlots[i].AddToClassList("selectedCard");
            }
        }
    }
}
