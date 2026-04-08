using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CardTypes
{
    attack,
    utility
}

public class CardsInHand : MonoBehaviour
{
    [SerializeField] private CardTypes _cardType;

    [SerializeField] private GameObject _cardGraphicsPrefab;

    [SerializeField] private SpriteRenderer _cardArt;

    [SerializeField] private TMP_Text _manaText;

    [SerializeField] private TMP_Text _nameText;

    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private LayerMask dropLayer;

    public Card Card { get; private set; }

    public GameEvent playCard;

    private Vector3 _cardStartPosition;
    private Quaternion _cardStartRotation;
    private int _distanceFromCamera = -10;
    

    //take a card and setup with the data of the card
    public void Setup(Card card)
    {
        Card = card;
        _manaText.text = card.Mana.ToString();
        _cardArt.sprite = card.CardArt;
        _nameText.text = card.NameText;
        _descriptionText.text = card.DescriptionText;
    }


    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        //when mouse hovers card deactivate the graphics
        _cardGraphicsPrefab.SetActive(false);
        //calculate the position of hoveredCardPrefab
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        //show the hoveredCardPrefab
        CardHoverSystem.Instance.ShowCard(Card, position);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        //hide the hoveredCardPrefab
        CardHoverSystem.Instance.HideCard();
        //when mouse leaves the hovered card activate the graphics
        _cardGraphicsPrefab.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;
        //activate the original card graphics
        _cardGraphicsPrefab.SetActive(true);
        //hide the hoveredCardPrefab
        CardHoverSystem.Instance.HideCard();
        //to remember the cards starting position before it gets dragged away
        _cardStartPosition = transform.position;
        _cardStartRotation = transform.rotation;
        //temporary solution
        transform.rotation = Quaternion.Euler(30, 0, 0);
        //set the z value to -10 with _distanceFromCamera to make it a bit closer to the camera
        transform.position = MouseUtilities.MousePositionInWorldSpace(_distanceFromCamera);
    }

    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;

        transform.position = MouseUtilities.MousePositionInWorldSpace(_distanceFromCamera);
    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;

        //if it hits something, like an enemy or a drop area, play the card
        if (ManaSystem.Instance.HaveEnoughMana(Card.Mana) && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
        {
            playCard.Raise(this, Card);
        }
        //return the card to the initial position and rotation
        else
        {
            transform.position = _cardStartPosition;
            transform.rotation = _cardStartRotation;
        }

        Interactions.Instance.PlayerIsDragging = false;
    }
}
