using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandManager _handManager;

    [SerializeField] private Transform drawPileTransform;
    [SerializeField] private Transform discardPileTransform;

    [SerializeField] private List<CardData> ownedCards = new List<CardData>();

    private readonly List<Card> drawPile = new List<Card>();
    private readonly List<Card> discardPile = new List<Card>();
    private readonly List<Card> handCards = new List<Card>();

    public GameEvent UpdateMana;

    private void Start()
    {
        Setup(ownedCards);
    }

    //add to draw pile all the cards from list of cardData
    public void Setup(List<CardData> ownedCards)
    {
        foreach (CardData cardData in ownedCards)
        {
            Card card = new Card(cardData);
            drawPile.Add(card);
        }
    }

    //So that GameEventListener can acces the coroutine function
    public void PlayCardEvent(Component sender, object data)
    {
        if (data is Card)
        {
            Card playedCard = (Card)data;
            StartCoroutine(PlayCard(playedCard));
        }
    }

    private IEnumerator PlayCard(Card playedCard)
    {
        //remove the played card from cards in hand list
        handCards.Remove(playedCard);
        //remove the card prefab from hand
        CardsInHand cardsInHand = _handManager.RemoveCard(playedCard);
        discardPile.Add(playedCard);
        yield return DiscardCard(cardsInHand);

        UpdateMana.Raise(this, -playedCard.Mana);

        for (int i = 0; i < playedCard.Effects.Count; i++)
        {
            playedCard.Effects[i].PerformEffect();
            yield return new WaitForSeconds(0.1f);
        }
    }

    //So that GameEventListener can acces the coroutine function
    public void DrawCardsEvent(Component sender, object data)
    {
        if (data is int)
        {
            int amount = (int)data;
            StartCoroutine(DrawCards(amount));
        }
    }

    private IEnumerator DrawCards(int amount)
    {
        //calculate how many cards can actually be drawn from drawPile before we need to refill drawPile with our discardPile cards
        int actualAmount = Mathf.Min(amount, drawPile.Count);

        //how many cards were not drawn so that we can draw them after the refill
        int notDrawnAmount = amount - actualAmount;

        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        if (notDrawnAmount > 0)
        {
            yield return RefillDeck();
            //draw cards equal to the not drawn amount
            if (drawPile.Count > notDrawnAmount)
            {
                for (int i = 0; i < notDrawnAmount; i++)
                {
                    yield return DrawCard();
                }
            }
            else if (drawPile.Count > 0)
            {
                for (int i = 0; i < drawPile.Count; i++)
                {
                    yield return DrawCard();
                }
            }
            else
            {
                Debug.Log("Unable to draw more cards");
            }
        }
    }

    private IEnumerator DrawCard()
    {
        //extionsion method Draw()
        Card card = drawPile.Draw();
        //add the card the hand list
        handCards.Add(card);
        //create the card prefab and add it to the cards in hand
        CardsInHand cardVisibility = CardCreator.Instance.CreateCardInHand(card, drawPileTransform.position, drawPileTransform.rotation);
        //yield return is used instead of StartCoroutine because yield return waits for the IEnumerator to finish before moving on
        yield return _handManager.AddCard(cardVisibility);
    }

    //So that GameEventListener can acces the IEnumerator function
    public void DiscardAllCardsEvent(Component sender, object data)
    {
        StartCoroutine(DiscardAllCards());
    }

    private IEnumerator DiscardAllCards()
    {
        //go through each card in hand and add them to discardPile then remove the card prefabs from hand
        foreach (Card card in handCards)
        {
            discardPile.Add(card);
            CardsInHand cardVisibility = _handManager.RemoveCard(card);
            //yield return waits for the IEnumerator to finish before moving on
            yield return DiscardCard(cardVisibility);
        }
        handCards.Clear();
    }


    private IEnumerator DiscardCard(CardsInHand cardVisibility)
    {
        cardVisibility.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardVisibility.transform.DOMove(discardPileTransform.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardVisibility.gameObject);
    }

    //Adds all cards from discard pile to draw pile
    private IEnumerator RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        return null;
    }


}
