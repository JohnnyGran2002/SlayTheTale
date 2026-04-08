using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CardHoverSystem : Singleton<CardHoverSystem>
{
    [SerializeField] private CardsInHand _hoveredCardInHand;
    [SerializeField] private Camera _mainCamera;

    //enable the hoveredCardInHand Prefab
    public void ShowCard(Card card, Vector3 position)
    {
        _hoveredCardInHand.gameObject.SetActive(true);
        _hoveredCardInHand.Setup(card);
        _hoveredCardInHand.transform.position = position;
        _hoveredCardInHand.transform.rotation = _mainCamera.transform.rotation;
    }

    //disable the hoveredCardInHand Prefab
    public void HideCard()
    {
        _hoveredCardInHand.gameObject.SetActive(false);
    }
}
