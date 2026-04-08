using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//place for desabling interactions
public class Interactions : Singleton<Interactions>
{
    public bool PlayerIsDragging { get; set; } = false;
    public bool ActionIsPerforming { get; set; } = false;

    //return true when action is not being performed
    public bool PlayerCanInteract()
    {
        if (ActionIsPerforming) return false;
        return true;
    }

    //check if player can hover card
    public bool PlayerCanHover()
    {
        if (PlayerIsDragging) return false;
        return true;
    }
}
