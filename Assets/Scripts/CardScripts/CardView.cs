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

    public CardView(VisualTreeAsset template)
    {
        template.CloneTree(this);
        
        // Check naming on elements
        _nameLabel = this.Q<Label>("Card_Name_Label");
        _descriptionLabel = this.Q<Label>("Card_Description_Label");
        _costLabel = this.Q<Label>("Card_Cost_Label");
        _typeLabel = this.Q<Label>("Card_Type_Label");
        _artwork = this.Q<VisualElement>("Artwork");
        _background = this.Q<VisualElement>("Card_Background");
    }

    public void Bind(Card card)
    {
        _nameLabel.text = card.NameText;
        _descriptionLabel.text = card.DescriptionText;
        _costLabel.text = card.Mana.ToString();
        _typeLabel.text = card.CardType.ToString();
        _artwork.style.backgroundImage = card.CardArt != null ? new StyleBackground(card.CardArt) : null;
    }
}
