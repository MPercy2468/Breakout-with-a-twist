using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Contains methods to control the ball and ball specific data
    //Attached to ball GameObject in scene

    //public variable assigned in engine editor
    public float defaultSpeed;

    [SerializeField]
    AudioSource slowdownSound;
    [SerializeField]
    AudioSource hitSound;
    [SerializeField]
    AudioSource landSound;

    float previousSpeedValue = 0;

    GameController gc;
    Rigidbody2D rb;

    //Contains raycast hit data for the next calculated collision point
    RaycastHit2D collisionHit;

    bool isPlayerAiming = false;
    float playerAimTime = 0;
    Vector2 aimDir;
    //Number of times the player can adjust their aim before touching the paddle again
    int aims = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gc.GetIsBallCaught())
        {
            aims = 1;
            return;
        }
        gc.IncreasePaddleBounces(1);
        gc.CatchBall();
        landSound.Play();
        aims = 1;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = GameObject.Find("_GameController").GetComponent<GameController>();
        previousSpeedValue = defaultSpeed;
    }

    private void Update()
    {
        if (collisionHit.point.Equals(Vector2.zero)||gc.GetIsBallCaught())
        {
            return;
        }
        if (!isPlayerAiming)
        {
            if (Input.GetButtonDown("Catch") && aims > 0)
            {
                isPlayerAiming = true;
                playerAimTime = 1.35f;
                aims--;
                aimDir = Vector2.down;
                slowdownSound.Play();
            }
        }
        else
        {
            TickAimTime();
            if(playerAimTime > 0)
            {
                PlayerAimLogic();
                if (Input.GetButtonDown("Fire"))
                {
                    isPlayerAiming = false;
                    playerAimTime = 0;
                    gc.EraseAimLine();
                    CalculateNewCollisionHit(aimDir);
                    SetSpeed(previousSpeedValue, aimDir);
                    slowdownSound.Stop();
                }
            }
        }
        if (Vector2.Distance(transform.position, collisionHit.point) < 0.4)
        {
            Bounce();
        }
    }

    //Called when player is in aiming mode
    //Calculates a valid aim vector every frame
    void PlayerAimLogic()
    {
        //Slow ball
        SetSpeed(3);
        //Get direction of mouse position relative to ball
        Vector2 mouseDir = Input.mousePosition;
        mouseDir = gc.GetMainCam().ScreenToWorldPoint(mouseDir);
        mouseDir = mouseDir - (Vector2)transform.position;
        mouseDir = mouseDir.normalized;
        //Get dot product of down vector and mouse direction
        //Assign mouse direction to caught launch vector if dot product is valid
        float dot = Vector2.Dot(Vector2.down, mouseDir);
        if (dot > 0.15f)
        {
            aimDir = mouseDir;
        }
        //Draw aim line
        gc.DrawAimLine(aimDir);
    }

    //Called to force the player out of aim mode
    void CancelPlayerAim()
    {
        SetSpeed(previousSpeedValue);
        isPlayerAiming = false;
        gc.EraseAimLine();
        slowdownSound.Stop();
    }

    //Called while in aim mode every frame
    //Reduces the time the player has to aim by deltatime
    void TickAimTime()
    {
        if(playerAimTime > 0)
        {
            playerAimTime -= Time.deltaTime;
        }
        else
        {
            playerAimTime = 0;
            CancelPlayerAim();
        }
    }

    //Called to 
    void Bounce()
    {
        //Get reflected direction with collisionHit normal
        Vector2 newDir = Vector2.Reflect(rb.velocity.normalized, collisionHit.normal);
        //Destroy block if possible
        if (collisionHit.transform.gameObject.TryGetComponent(out Block b))
        {
            b.HitBlock();
            hitSound.Play();
        }
        //Interact with wall logic if possible
        if(collisionHit.transform.gameObject.TryGetComponent(out Wall w))
        {
            w.WallLogic();
        }
        //Get new collisionHit
        CalculateNewCollisionHit(newDir);
        //Assign reflected direction to ball
        SetDirection(newDir);
    }

    public void CalculateNewCollisionHit(Vector2 dir)
    {
        //collisionHit = Physics2D.Raycast(transform.position, dir, 100f, LayerMask.GetMask("Blocks", "Walls"));
        collisionHit = Physics2D.CircleCast((Vector2)transform.position+dir, 0.3f, dir, 100f, LayerMask.GetMask("Blocks", "Walls"));
    }

    
    public void ModifySpeed(float s)
    {
        if (isPlayerAiming)
        {
            previousSpeedValue += s;
        }
        else
        {
            //Modify ball velocity by s
            rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + s);
            //Save modified speed value
            if (rb.velocity.magnitude != 0)
            {
                previousSpeedValue = rb.velocity.magnitude;
            }
        }

    }
    
    public void SetSpeed(float s)
    {
        //Set ball velocity to s
        rb.velocity = rb.velocity.normalized * s;
    }
    public void SetSpeed(float s,Vector2 dir)
    {
        //Set ball velocity to s and change direction
        rb.velocity = dir * s;
    }

    public void SetDirection(Vector2 dir)
    {
        //Change ball direction, maintain speed
        rb.velocity = rb.velocity.magnitude * dir;
    }

    public void StopBall()
    {
        rb.velocity = Vector2.zero;
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }
    
    public void SetPreviousSpeed(float s)
    {
        previousSpeedValue = s;
    }
    public float GetPreviousSpeedValue()
    {
        return previousSpeedValue;
    }
}
