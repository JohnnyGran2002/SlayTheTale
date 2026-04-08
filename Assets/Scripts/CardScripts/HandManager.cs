using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class HandManager : MonoBehaviour
{
    [SerializeField] private float _maxCardsInHand;

    [SerializeField] private float _animationDuration;

    [SerializeField] private SplineContainer _splineContainer;

    [SerializeField] private Camera _mainCamera;

    private readonly List<CardsInHand> _cards = new List<CardsInHand>();

    //take CardVisibility and add to list
    public IEnumerator AddCard(CardsInHand cardVisibility)
    {
        _cards.Add(cardVisibility);
        yield return UpdateCardPosition(_animationDuration);
    }

    
    public CardsInHand RemoveCard(Card card)
    {
        //looks for the card in all CardsInHand
        CardsInHand cardVisiblity = GetCardsInHand(card);
        if (cardVisiblity == null) return null;
        //remove the card prefab from hand
        _cards.Remove(cardVisiblity);
        //update the postition of all cards in hand
        StartCoroutine(UpdateCardPosition(_animationDuration));
        return cardVisiblity;
    }

    private CardsInHand GetCardsInHand(Card card)
    {
        //look through all the cards in hand until finding the first matching card or return default if no match where found
        return _cards.Where(cardVisibility => cardVisibility.Card == card).FirstOrDefault();
    }

    //iterate through all cards in hand and place them at correct position and rotation on the curve
    private IEnumerator UpdateCardPosition(float duration)
    {
        //check if there are cards in cards list
        if (_cards.Count == 0) yield break;

        //calculate spacing between cards(1f is the lenght of Spline)
        float cardSpacing = 1f / _maxCardsInHand;

        //start position of first card in spline(0.5f is middle of Spline), disribute evenly in both directions
        float firstCardPosition = 0.5f - (_cards.Count - 1) * cardSpacing / 2;

        //reference to spline of the splineContainer
        Spline spline = _splineContainer.Spline;

        //go trhough each card and calculate the correct position and rotation on the spline
        for (int i = 0; i < _cards.Count; i++)
        {
            //calcutate the position
            float position = firstCardPosition + i * cardSpacing;

            //spline method to get the psotion as Vector3(vonverts float position to wold position)
            Vector3 splinePosition = spline.EvaluatePosition(position);

            //direction of the spline at position(used for calculating rotation)
            Vector3 foward = spline.EvaluateTangent(position);

            //direction wich is indicated by the three lines in the scene wiew that is set to face way from the Camera(used for calculating rotation)
            //currently not used migt be needed later
            //Vector3 up = spline.EvaluateUpVector(position);

            //TODO: look at main camera
            //calculate the rotation, foward direction is set to the up Vector(away from the camera), calculate the third direction using Cross Product with the up and foward Vector
            Quaternion rotation = Quaternion.LookRotation(-_mainCamera.transform.position, Vector3.Cross(-_mainCamera.transform.position, foward).normalized);

            //start of the replacement
            /*_cards[i].transform.position = Vector3.Lerp(transform.position, new Vector3(splinePosition.x * 4, splinePosition.y) + Vector3.back, duration);
            _cards[i].transform.rotation = Quaternion.Lerp(_cards[i].transform.rotation, rotation, duration);*/

            //move cards(first the parameter is the destination, and the second it the duration of the movement)
            _cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            //rotate cards
            _cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }

        yield return new WaitForSeconds(duration);
    }
}
