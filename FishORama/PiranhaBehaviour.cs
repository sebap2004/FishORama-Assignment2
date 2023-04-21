using System;
using System.Runtime.InteropServices;
using FishORamaEngineLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishORama;

public class PiranhaBehaviour
{
    public delegate void AteChicken(int fishNumber);
    public static event AteChicken ChickenAte;
    
    public delegate void TriggerNewGame();
    public static event TriggerNewGame roundTrigger;

    public delegate void EndRound();

    public static event EndRound RoundEnd;

    private bool ateAlready;
    private Piranha piranha;
    private FishState currentState;
    private Team team;
    
    
    public PiranhaBehaviour(Piranha pPiranha, Team pTeam)
    {
        piranha = pPiranha;
        team = pTeam;
        currentState = FishState.Idle;
    }
    
    public void Update()
    {
        Movement();
    }

    private Vector2 CalculateDirection(Vector2 startVec, Vector2 endVec)
    {
        Vector2 distanceVector = Vector2.Subtract(endVec, startVec);
        return distanceVector;
    }
    
    
    private void Movement()
    {
        // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***
            switch (currentState)
            {
                case (FishState.Idle):
                    // calculate x and y coordinates based on piranha.angle and radius
                    piranha.xPosition = piranha.idlePosition.X + 10 * (float)Math.Cos(piranha.angle);
                    piranha.yPosition = piranha.idlePosition.Y + 10 * (float)Math.Sin(piranha.angle);
                    piranha.angle += 0.05f;
                    if (piranha.tokenManager.ChickenLeg != null)
                    {
                        roundTrigger();
                    }
                    break;
                case (FishState.Chase):


                    Vector2 chickenPosition = new Vector2();
                    Vector2 currentPosition;
                    Vector2 distanceVector;
                    Vector2 directionVector;
                    if (piranha.tokenManager.ChickenLeg != null)
                    {
                        chickenPosition = new(piranha.tokenManager.ChickenLeg.Position.X, piranha.tokenManager.ChickenLeg.Position.Y);
                        currentPosition = new(piranha.xPosition, piranha.yPosition);
                        distanceVector = CalculateDirection(currentPosition, chickenPosition);
                        directionVector = Vector2.Normalize(distanceVector);
                        piranha.xPosition += directionVector.X * piranha.speed;
                        piranha.yPosition += directionVector.Y * piranha.speed;

                        if (distanceVector.Length() < 100 && piranha.tokenManager.ChickenLeg != null && !ateAlready)
                        {
                            ateAlready = true;
                            SetFishState(FishState.Return);
                            Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} got the leg!");
                            ChickenAte(team.teamNumber);
                            piranha.tokenManager.RemoveChickenLeg();
                        }
                    }
                    else
                    {
                        SetFishState(FishState.Return);
                        piranha.textureID = "Piranha2";
                        Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} missed the leg!");
                    }
                    
                    break;
                
                case (FishState.Return):
                    currentPosition = new(piranha.xPosition, piranha.yPosition);
                    distanceVector = Vector2.Subtract(piranha.idlePosition, currentPosition);
                    directionVector = Vector2.Normalize(distanceVector);
                    piranha.xPosition += directionVector.X * piranha.speed;
                    piranha.yPosition += directionVector.Y * piranha.speed;

                    if (distanceVector.Length() < 5)
                    {
                        piranha.xPosition = piranha.idlePosition.X;
                        piranha.yPosition = piranha.idlePosition.Y;
                        SetFishState(FishState.Idle);
                        piranha.calcRand = true;
                    }

                    ateAlready = false;
                    break;
                case (FishState.Win):
                    if (team.teamNumber == 1)
                    {
                        directionVector =
                            Vector2.Normalize(CalculateDirection(new Vector2(piranha.xPosition,piranha.yPosition),
                                new Vector2(piranha.xPosition,piranha.yPosition) + new Vector2(1200, 0)));
                    }
                    else
                    {
                        directionVector =
                            Vector2.Normalize(CalculateDirection(new Vector2(piranha.xPosition,piranha.yPosition),
                                new Vector2(piranha.xPosition,piranha.yPosition) + new Vector2(-1200, 0)));
                    }
                    piranha.xPosition += directionVector.X * piranha.speed;
                    piranha.yPosition += directionVector.Y * piranha.speed;
                    break;
            }
    }
    
    public enum FishState
    {
        Idle,
        Chase,
        Return,
        Win
    }
        
    public void SetFishState(FishState pFishState)
    {
        switch (pFishState)
        {
            case (FishState.Idle):
                currentState = FishState.Idle;
                piranha.textureID = "Piranha1";
                RoundEnd();
                break;
            case (FishState.Chase):
                currentState = FishState.Chase;
                piranha.speed = new Random().Next(4, 6);
                break;
            case (FishState.Return):
                currentState = FishState.Return;
                break;
            case(FishState.Win):
                currentState = FishState.Win;
                piranha.speed = 4;
                break;
        }
    }
    
}