using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FishORamaEngineLibrary;

namespace FishORama
{
    /// CLASS: OrangeFish - this class is structured as a FishORama engine Token class
    /// It contains all the elements required to draw a token to screen and update it (for movement etc)
    public class Fish : IDraw
    {
        // CLASS VARIABLES
        // Variables hold the information for the class
        // NOTE - these variables must be present for the class to act as a TOKEN for the FishORama engine
        protected string textureID;               // Holds a string to identify asset used for this token
        protected float xPosition;                // Holds the X coordinate for token position on screen
        protected float yPosition;                // Holds the Y coordinate for token position on screen
        protected int xDirection;                 // Holds the direction the token is currently moving - X value should be either -1 (left) or 1 (right)
        protected int yDirection;                 // Holds the direction the token is currently moving - Y value should be either -1 (down) or 1 (up)
        protected Screen screen;                  // Holds a reference to the screen dimansions (width and height)
        public ITokenManager tokenManager;     // Holds a reference to the TokenManager - for access to ChickenLeg variable

        // *** ADD YOUR CLASS VARIABLES HERE *** 
        protected int teamNumber;                 // Team number, used to show score
        protected int fishNumber;                 // Fish number used for debugging purposes
        protected float xSpeed;
        protected float ySpeed;
        protected Vector2 idlePosition;
        protected float speed;
        protected float angle;
        protected Team team;
        protected Random _random;

        /// CONSTRUCTOR: OrangeFish Constructor
        /// The elements in the brackets are PARAMETERS, which will be covered later in the course
        protected Fish(string pTextureID, float pXpos, float pYpos, Screen pScreen, ITokenManager pTokenManager, int pTeamNumber, int pFishNumber, Team pTeam)
        {
            // State initialisation (setup) for the object
            textureID = pTextureID;
            xPosition = pXpos;
            yPosition = pYpos;
            xDirection = 1;
            yDirection = 1;
            screen = pScreen;
            tokenManager = pTokenManager;

            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***
            
            team = pTeam;
            fishNumber = pFishNumber;
            teamNumber = pTeamNumber;
            
            // Flips the sprite based on which team number is parsed.
            if (teamNumber == 2)
            { xDirection = -1; }
            
            idlePosition = new Vector2(pXpos, pYpos);
            xSpeed = 1;
            ySpeed = xSpeed;
            speed = 3;
            _random = new Random();
        }

        /// METHOD: Update - will be called repeatedly by the Update loop in Simulation
        /// Write the movement control code here
        public virtual void Update()
        {
            // Empty virtual method that will be overridden by inheriting classes
            // Called by Simulation.
        }

        /// METHOD: Draw - Called repeatedly by FishORama engine to draw token on screen
        /// DO NOT ALTER, and ensure this Draw method is in each token (fish) class
        /// Comments explain the code - read and try and understand each lines purpose
        public void Draw(IGetAsset pAssetManager, SpriteBatch pSpriteBatch)
        {
            Asset currentAsset = pAssetManager.GetAssetByID(textureID); // Get this token's asset from the AssetManager

            SpriteEffects horizontalDirection; // Stores whether the texture should be flipped horizontally

            if (xDirection < 0)
            {
                // If the token's horizontal direction is negative, draw it reversed
                horizontalDirection = SpriteEffects.FlipHorizontally;
            }
            else
            {
                // If the token's horizontal direction is positive, draw it regularly
                horizontalDirection = SpriteEffects.None;
            }

            // Draw an image centered at the token's position, using the associated texture / position
            pSpriteBatch.Draw(currentAsset.Texture,                                             // Texture
                              new Vector2(xPosition, yPosition * -1),                                // Position
                              null,                                                             // Source rectangle (null)
                              Color.White,                                                      // Background colour
                              0f,                                                               // Rotation (radians)
                              new Vector2(currentAsset.Size.X / 2, currentAsset.Size.Y / 2),    // Origin (places token position at centre of sprite)
                              new Vector2(1, 1),                                                // scale (resizes sprite)
                              horizontalDirection,                                              // Sprite effect (used to reverse image - see above)
                              1);                                                               // Layer depth
        }
    }
}
