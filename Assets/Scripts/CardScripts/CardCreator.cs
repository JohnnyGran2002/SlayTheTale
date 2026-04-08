using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardCreator : Singleton<CardCreator>
{
    [SerializeField] private CardsInHand _cardInHandPrefab;

    public CardsInHand CreateCardInHand(Card card, Vector3 position, Quaternion rotation)
    {
        CardsInHand cardsInHand = Instantiate(_cardInHandPrefab, position, rotation);//ObjectPoolManager.SpawnObject(_cardInHandPrefab, position, rotation, ObjectPoolManager.PoolType.Cards);
        //small scale up animation
        cardsInHand.transform.localScale = Vector3.zero;
        cardsInHand.transform.DOScale(Vector3.one, 0.15f);
        //cards in hand always comes in intialised 
        cardsInHand.Setup(card);
        return cardsInHand;
    }
}
