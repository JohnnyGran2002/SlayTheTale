using UnityEngine;
using UnityEngine.InputSystem;

public class Teo_Test : MonoBehaviour
{
    public GameEvent cardsCleared;

    public void CardsCleared(InputAction.CallbackContext context)
    {
        if (context.performed)
            cardsCleared.Raise(this, null);
    }
}
