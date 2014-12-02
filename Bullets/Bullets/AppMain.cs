using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;

namespace Bullets
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static Obstacle[]	obstacles;
		public static Player 		player;
		private static Background 	background;
		//Possible future implementation
		//private static Powerup 		powerup;
		
		public static void Main (string[] args)
		{
			Initialize ();
			
			//Game Loop
			bool quitGame = false;
			while (!quitGame)
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render ();
				UISystem.Render ();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();

			}
		}

		public static void Initialize ()
		{
			//Set up director and UISystem
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Create background
			background = new Background(gameScene);
			
			//Set the ui Scene
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			
			//Score implementation goes here (Milestone 2)
			
	
			player = new Player(gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}

		public static void Update ()
		{	
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			
			//If tapped, inform the player.
			if(touches.Count > 0)
				player.position = new Vector2(touches[0].X,touches[0].Y);
			
			//Update the player.
			Player.Update();
		}
	}
}
