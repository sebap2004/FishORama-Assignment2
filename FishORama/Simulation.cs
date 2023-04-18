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

        List<Piranha> piranhaList;




        /// CONSTRUCTOR - for the Simulation class - run once only when an object of the Simulation class is INSTANTIATED (created)
        /// Use constructors to set up the state of a class
        public Simulation(IKernel pKernel)
        {
            kernel = pKernel;                   // Stores the game engine kernel which is passed to the constructor when this class is created
            screen = kernel.Screen;             // Sets the screen variable in Simulation so the screen dimensions are accessible

            // *** ADD OTHER INITIALISATION (class setup) CODE HERE ***

            piranhaList = new List<Piranha>();


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
                if (i < 3)
                {
                    initXpos = -300;
                    initYpos = 150 - (150 * i);
                    Piranha currentFish = new("Piranha1", initXpos, initYpos, screen, tokenManager, 1, i+1);
                    piranhaList.Add(currentFish);
                    kernel.InsertToken(currentFish);
                }
                else
                {
                    initXpos = 300;
                    initYpos = 150 - (150 * (i-3));
                    Piranha currentFish = new("Piranha1", initXpos, initYpos, screen, tokenManager, 2, i-2);
                    piranhaList.Add(currentFish);
                    kernel.InsertToken(currentFish);
                }
            }
        }

        /// METHOD: Update - called 60 times a second by the FishORama engine when the program is running
        /// Add all tokens so Update is called on them regularly
        public void Update(GameTime gameTime)
        {

            // *** ADD YOUR UPDATE CODE HERE ***
            // Each fish object (sitting in a variable) must have Update() called on it here




        }
    }
}
