using UnityEngine;
using UnityEngine.UIElements;

public class CardView : VisualElement
{
    private readonly Label _nameLabel;
    private readonly Label _descriptionLabel;
    private readonly Label _costLabel;
    private readonly Label _typeLabel;
    private readonly VisualElement _artwork;
    private readonly VisualElement _background;
    private readonly VisualElement _shadow;

    public CardView(VisualTreeAsset template)
    {
        template.CloneTree(this);
        style.alignItems = Align.Center;
        // Check naming on elements
        _nameLabel = this.Q<Label>("Label_Name");
        _descriptionLabel = this.Q<Label>("Label_Description");
        _costLabel = this.Q<Label>("Label_Cost");
        _typeLabel = this.Q<Label>("Label_Type");
        _artwork = this.Q<VisualElement>("Artwork");
        _background = this.Q<VisualElement>("Background_Card");
        _shadow = this.Q<VisualElement>("Shadow_Card");
    }

    public void Bind(Card card)
    {
        _nameLabel.text = card.NameText;
        _descriptionLabel.text = card.DescriptionText;
        _costLabel.text = card.Mana.ToString();
        var c = card.CardType.ToString()[0];
        _typeLabel.text = c.ToString().ToUpper();
        _artwork.style.backgroundImage = card.CardArt != null ? new StyleBackground(card.CardArt) : null;
        _background.style.backgroundImage = card.Background != null ? new StyleBackground(card.Background) : null;
        _shadow.style.backgroundImage = card.Shadow != null ? new StyleBackground(card.Shadow) : null;
    }
}
