using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuPlayerController : PlayerController {

    public override void update()
    {
        base.update();
        if (input.GetButtonDown("A"))
        {
            print("input " + inputIndex + "pressed A!");
        }
        if (input.GetButtonDown("DpadUp"))
        { 
            print("player " + playerNumber + " pressed Up!");
        }
        if (input.GetButtonDown("DpadDown"))
        {
            print("player " + playerNumber + " pressed Down!");
        }
    }
}