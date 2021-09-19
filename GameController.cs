using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Responsible for controlling macro game functions
    //Start game, reset ball, modify score, etc...
    //Holds important game data
    //Attached to _GameController GameObject in scene

    //Serialized variables assined in engine editor
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    Ball ball;
    [SerializeField]
    Paddle paddle;
    [SerializeField]
    int lives;

    int score = 0;
    int totalPaddleBounces = 0;
    bool isBallCaught = false;
    bool isLose = false;
    bool isWin = false;
    Vector2 caughtLaunchDir = Vector2.zero;
    LineRenderer aimLine;

    private void Awake()
    {
        aimLine = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        CatchBall();
    }

    private void Update()
    {
        if (isWin)
        {
            return;
        }
        else
        {
            if(score == 252)
            {
                WinGame();
            }
        }
        if (isBallCaught)
        { 
            CaughtBallLogic();
            if (Input.GetButtonDown("Fire"))
            {
                isBallCaught = false;
                EraseAimLine();
                LaunchBall(caughtLaunchDir);
            }
        }
    }

    //Called to reset the game, like when the ball goes out bounds
    public void ResetGame()
    {
        lives--;
        if(lives <= 0)
        {
            LoseGame();
            CatchBall();
            return;
        }
        CatchBall();
        totalPaddleBounces = 0;
    }

    //Called to increase sucessful landings
    public void IncreasePaddleBounces(int i)
    {
        //Increase paddle bounces by i
        //Increase ball speed at 4 and 12 total bounces
        totalPaddleBounces += i;
        if(totalPaddleBounces == 4)
        {
            ball.ModifySpeed(1);
        }
        else if(totalPaddleBounces == 12)
        {
            ball.ModifySpeed(2);
        }
    }

    void LaunchBall(Vector2 dir)
    {
        //Launch ball in specified direction
        ball.CalculateNewCollisionHit(dir);
        ball.SetSpeed(ball.GetPreviousSpeedValue(),dir);
    }

    //Called while ball is landed and awaiting player input
    void CaughtBallLogic()
    {
        //Stop ball and place it over paddle
        ball.StopBall();
        ball.SetPosition(paddle.GetCaughtPosition());
        //Get direction of mouse position relative to ball
        Vector2 mouseDir = Input.mousePosition;
        mouseDir = mainCam.ScreenToWorldPoint(mouseDir);
        mouseDir = mouseDir - (Vector2)ball.transform.position;
        mouseDir = mouseDir.normalized;
        //Get dot product of up vector and mouse direction
        //Assign mouse direction to caught launch vector if dot product is valid
        float dot = Vector2.Dot(Vector2.up, mouseDir);
        if(dot > 0.3f)
        {
            caughtLaunchDir = mouseDir;
        }
        //Draw aim line
        DrawAimLine(caughtLaunchDir);
    }


    //Called to render the guiding aim line for the player
    public void DrawAimLine(Vector2 dir)
    {
        aimLine.enabled = true;
        //Assign position count
        aimLine.positionCount = 3;
        //Assign start point
        aimLine.SetPosition(0, (Vector2)ball.transform.position+dir*0.25f);
        //Assign midpoint
        RaycastHit2D hit = Physics2D.CircleCast(ball.transform.position, 0.4f, dir, 100f, LayerMask.GetMask("Blocks", "Walls"));
        float d = Vector2.Distance(ball.transform.position, hit.point);
        aimLine.SetPosition(1,(Vector2)ball.transform.position+dir*d);
        //Assign end point
        aimLine.SetPosition(2, hit.point);
    }

    //Called to remove the guiding aim line
    public void EraseAimLine()
    {
        aimLine.enabled = false;
    }

    //Called to make ball catch bool true
    //Enables CaughtBallLogic to be run in update method
    public void CatchBall()
    {
        isBallCaught = true;
    }

    //Called to modify score value
    public void ModifyScore(int s)
    {
        score += s;
    }

    //Called to run logic for when player loses
    void LoseGame()
    {
        Time.timeScale = 0;
        isLose = true;
    }
    //Called to run logic when player wins
    void WinGame()
    {
        Time.timeScale = 0;
        isWin = true;
    }

    //Getter methods
    public bool GetIsBallCaught()
    {
        return isBallCaught;
    }
    public int GetScore()
    {
        return score;
    }    
    public int GetBounces()
    {
        return totalPaddleBounces;
    }
    public Camera GetMainCam()
    {
        return mainCam;
    }
    public Ball GetBall()
    {
        return ball;
    }

    public Paddle GetPaddle()
    {
        return paddle;
    }
    public int GetLives()
    {
        return lives;
    }
    public bool GetIsLose()
    {
        return isLose;
    }
    public bool GetIsWin()
    {
        return isWin;
    }

}
