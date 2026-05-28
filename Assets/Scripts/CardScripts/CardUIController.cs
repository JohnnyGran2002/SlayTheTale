using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CardUIController : MonoBehaviour
{
    
    [SerializeField] private BookUIController bookUI;
    [SerializeField] private List<CardData> cards;
    private List<Card> hand = new List<Card>();

    private void Start()
    {
        foreach (var data in cards)
        {
            hand.Add(new Card(data));
        }

        bookUI.OnUIReady += DrawCard;
    }

    private void Update()
    {
        
    }

    public void DrawAction(InputAction.CallbackContext context)
    {
        DrawCard();
    }
    public void DrawCard()
    {
        bookUI.FillBook(hand);
        Debug.Log($"Hand Count: {hand.Count}");
    }
}
