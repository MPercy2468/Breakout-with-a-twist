using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //Class to be inherited by any wall scripts
    //Also used to identify walls

    protected GameController gc;
    private void Awake()
    {
        gc = GameObject.Find("_GameController").GetComponent<GameController>();
    }
    //Wall logic to be called by ball on collision
    public virtual void WallLogic() { }
}
