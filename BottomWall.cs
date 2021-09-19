using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomWall : Wall
{
    //Resets game state if player goes out of bounds
    public override void WallLogic()
    {
        gc.ResetGame();
    }
}
