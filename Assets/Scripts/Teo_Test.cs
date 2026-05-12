using UnityEngine;
using UnityEngine.InputSystem;

public class Teo_Test : MonoBehaviour
{
    public GameEvent cardsCleared, ping;

    public void CardsCleared(InputAction.CallbackContext context)
    {
        if (context.performed)
            cardsCleared.Raise(this, null);
    }

    public void PingSomething(InputAction.CallbackContext context)
    {
        string message = "Something";
        if (context.performed) ping.Raise(this, message);
    }
    public void PingSomethingElse(InputAction.CallbackContext context)
    {
        string message = "Something Else";
        if (context.performed) ping.Raise(this, message);
    }
}
