using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FishORamaEngineLibrary;

namespace FishORama
{
    /// CLASS: Simulation class - the main class users code in to set up a FishORama simulation
    /// All tokens to be displayed in the scene are added here
    public class Simulation : IUpdate, ILoadContent
    {
        // CLASS VARIABLES
        // Variables store the information for the class
        private IKernel kernel;                 // Holds a reference to the game engine kernel which calls the draw method for every token you add to it
        private Screen screen;                  // Holds a reference to the screeen dimensions (width, height)
        private ITokenManager tokenManager;     // Holds a reference to the TokenManager - for access to ChickenLeg variable

        /// PROPERTIES
        public ITokenManager TokenManager      // Property to access chickenLeg variable
        {
            set { tokenManager = value; }
        }

        // *** ADD YOUR CLASS VARIABLES HERE ***
        // Variables to hold fish will be declared here

        private Team Team1;
        private Team Team2;
        private Referee referee;
        private List<Fish> FishList;
        
        


        /// CONSTRUCTOR - for the Simulation class - run once only when an object of the Simulation class is INSTANTIATED (created)
        /// Use constructors to set up the state of a class
        public Simulation(IKernel pKernel)
        {
            kernel = pKernel;                   // Stores the game engine kernel which is passed to the constructor when this class is created
            screen = kernel.Screen;             // Sets the screen variable in Simulation so the screen dimensions are accessible
            
            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***
            
            Team1 = new Team(1);
            Team2 = new Team(2);
            FishList = new List<Fish>();
            referee = new Referee(Team1, Team2);
            Referee.LegPlace += PlaceLeg;
        }

        /// METHOD: LoadContent - called once at start of program
        /// Create all token objects and 'insert' them into the FishORama engine
        public void LoadContent(IGetAsset pAssetManager)
        {
            // *** ADD YOUR NEW TOKEN CREATION CODE HERE ***
            // Code to create fish tokens and assign to thier variables goes here
            // Remember to insert each token into the kernel

            int initXpos;
            int initYpos;
            
            for(int i = 0; i < 6; i++)
            {
                if (i >= 3)
                {
                    initXpos = -300;
                    initYpos = 150 - (150 * (i-3));
                    FishBehaviour currentFish = new("Piranha1", initXpos, initYpos, screen, tokenManager, 2, i-2, Team1);
                    Team1.teamMembers.Add(currentFish);
                    FishList.Add(currentFish);
                    kernel.InsertToken(currentFish);
                }
                else
                {
                    initXpos = 300;
                    initYpos = 150 - (150 * i);
                    FishBehaviour currentFish = new("Piranha1", initXpos, initYpos, screen, tokenManager, 1, i+1, Team2);
                    Team2.teamMembers.Add(currentFish);
                    FishList.Add(currentFish);
                    kernel.InsertToken(currentFish);
                }
            }
            
            referee.StartGame();
        }

        /// METHOD: Update - called 60 times a second by the FishORama engine when the program is running
        /// Add all tokens so Update is called on them regularly
        public void Update(GameTime gameTime)
        {
            // *** ADD YOUR UPDATE CODE HERE ***
            // Each fish object (sitting in a variable) must have Update() called on it here
            
            referee.Update();
            UpdateScore(Team1.TeamScore, Team2.TeamScore);
            foreach (Fish piranha in FishList)
            {
                piranha.Update();
            }
        }

        private void PlaceLeg()
        {
            ChickenLeg newChickenLeg = new ChickenLeg("ChickenLeg", 0, 0);
            tokenManager.SetChickenLeg(newChickenLeg);
            kernel.InsertToken(newChickenLeg);
        }
        private void RemoveChickenLeg()
        {
            if (tokenManager.ChickenLeg != null)
            {
                kernel.RemoveToken(tokenManager.ChickenLeg);
                tokenManager.SetChickenLeg(null);
            }
        }

        private void UpdateScore(int pScore1, int pScore2)
        {
            kernel.UpdateScoreText(pScore1, pScore2);
        }
    }
}
