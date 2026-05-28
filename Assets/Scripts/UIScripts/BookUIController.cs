using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BookUIController : MonoBehaviour
{
    [SerializeField] private UIDocument ui;
    [SerializeField] private VisualTreeAsset cardTemplate;
    private VisualElement _root;
    private  VisualElement _overlay;
    private VisualElement _overlayCard;

    private readonly List<VisualElement> _cardSlots = new ();
    private List<Card> _cards = new();
    private const int Columns = 4;
    private const int Rows = 2;
    private int _currentRow = 0;
    private int _currentColumn = 0;
    private int SelectedIndex => _currentRow * Columns + _currentColumn;

    private void Awake()
    {
        _root = ui.rootVisualElement;
        _overlay = _root.Q<VisualElement>("Overlay");
        _overlayCard = _overlay.Q<VisualElement>("OverlayCardSlot");
        for (int i = 0; i < 8; i++)
        {
            var slot = _root.Q<VisualElement>($"CardSlot_{i + 1}");
            _cardSlots.Add(slot);
        }

        _currentColumn = 2;
        // Vänta på första layout passet
        _root.RegisterCallback<GeometryChangedEvent>(OnGeometryReady);
    }
    private bool _initialized;
    public Action OnUIReady;
    private void OnGeometryReady(GeometryChangedEvent evt)
    {
        // Kör bara en gång
        if (_initialized)
            return;

        _initialized = true;

        // viktigt: unsubscribe
        _root.UnregisterCallback<GeometryChangedEvent>(OnGeometryReady);

        OnUIReady?.Invoke();
    }
    private void FitCardToSlot(VisualElement card, VisualElement slot)
    {
        float slotWidth = slot.layout.width;
        float cardWidth = 200f;
        float scale = slotWidth / cardWidth;

        card.style.scale = new StyleScale(new Scale(new Vector2(scale, scale)));
    }

    public void FillBook(List<Card> cards)
    {
        if(_cards.Count != cards.Count)
        {
            _cards.Clear();
            _cards = cards;
        }
        for (int i = 0; i < 8; i++)
        {
            var slot = _cardSlots[i];
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
        
        var slot = _root.Q<VisualElement>($"CardSlot_{slotIndex}");

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
            _currentColumn++;
        }
        else if (input.x < -0.5f)
        {
            _currentColumn--;
        }
        // Vertikalt
        if (input.y < -0.5f)
        {
            _currentRow++;
        }
        else if (input.y > 0.5f)
        {
            _currentRow--;
        }
        _currentRow = Mathf.Clamp(_currentRow, 0, Rows - 1);
        _currentColumn = Mathf.Clamp(_currentColumn, 0, Columns - 1);
        
        UpdateSelectionVisual();
    }
    
    private void UpdateSelectionVisual()
    {
        var slot = _cardSlots[SelectedIndex];
        
        if (slot.childCount == 0)
            return;
        // skapa kopia
        //var clone = _root.Q<VisualElement>("OverlayCardSlot");
        _overlayCard.RemoveFromClassList("selectedCard");
        _overlayCard.schedule.Execute(() =>
        {
            _overlayCard.Clear();
            var card = new CardView(cardTemplate);
            card.Bind(_cards[SelectedIndex]);
            _overlayCard.Add(card);
            //FitCardToSlot(card, clone);

            // world position
            Rect slotRect = slot.worldBound;
            Rect overlayRect = _overlay.worldBound;

            Vector2 localPos = new Vector2(
                slotRect.x - overlayRect.x,
                slotRect.y - overlayRect.y
            );
            
            _overlayCard.style.position = Position.Absolute;
            _overlayCard.style.left = localPos.x;
            _overlayCard.style.top = localPos.y;

            // _overlay.Add(clone);
            // Vänta en frame innan selected-class
            _overlayCard.schedule.Execute(() =>
            {
                _overlayCard.AddToClassList("selectedCard");
            }).ExecuteLater(1);
        }).ExecuteLater(150);
        
    }
}
