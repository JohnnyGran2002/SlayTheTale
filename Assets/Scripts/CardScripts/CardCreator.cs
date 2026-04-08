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
        GameObject cardGameObject = ObjectPoolManager.SpawnObject(_cardInHandPrefab.gameObject, position, rotation, ObjectPoolManager.PoolType.Cards);
        CardsInHand cardsInHand = cardGameObject.GetComponent<CardsInHand>();
        //small scale up animation
        cardsInHand.transform.localScale = Vector3.zero;
        cardsInHand.transform.DOScale(Vector3.one, 0.15f);
        //cards in hand always comes in intialised 
        cardsInHand.Setup(card);
        return cardsInHand;
    }

    public void ReturnCardToPool(CardsInHand cardsInHand)
    {
        if (cardsInHand == null) return;
        ObjectPoolManager.ReturnObjectToPool(cardsInHand.gameObject);
    }
}
