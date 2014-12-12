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
		public static Sce.PlayStation.HighLevel.GameEngine2D.Scene gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label scoreLabel;
		
		private static Obstacle[] obstacles;
		public static Player player;
		public static Enemy enemy;
		private static Background background;
		public static List<Projectile> proj;
		public static List<Enemy> enemyList;
		
		public static int score;
		public static int level; 
		public static int levelEnemyAmount;
		
		//Game states which make up the game engine
		private const int GAME_STATE_TITLE = 10;
		private const int  GAME_STATE_MENU = 20;
		private const int  GAME_STATE_HTP = 30;
		private const int  GAME_STATE_NEW_GAME = 40;
		private const int  GAME_STATE_NEW_LEVEL = 50;
		private const int  GAME_STATE_PLAY_GAME = 60;
		private const int  GAME_STATE_PLAYER_DIE = 70;
		private const int  GAME_STATE_GAME_OVER = 80;
		
		public static int currentGameState = 0;
		//Possible future implementation
		//private static Powerup 		powerup;
			
		public static void Main (string[] args)
		{
			Initialize ();
			//Switch the game state to title
			switchGameState(GAME_STATE_NEW_GAME);
			
			//Game Loop
			bool quitGame = false;
			while (!quitGame) {		
				Director.Instance.Update ();
				runGame();
				Director.Instance.Render ();
				//UISystem.Render ();			
				Director.Instance.GL.Context.SwapBuffers ();
				Director.Instance.PostSwap ();
			}
			Director.Terminate();
		}
		
		public static void gameStateTitle(){}
		public static void gameStateMenu(){}
		public static void gameStateHTP(){}
		public static void gameStatePlayerDie(){}
		public static void gameStateGameOver(){}
		
		public static void runGame(){
			switch(currentGameState){	
				case GAME_STATE_TITLE:
					gameStateTitle();
					break;			
				case GAME_STATE_MENU:
					gameStateMenu();
					break;
				case GAME_STATE_HTP:
					gameStateHTP();
					break;
				case GAME_STATE_NEW_GAME:
					gameStateNewGame();
					break;			
				case GAME_STATE_NEW_LEVEL:
					gameStateNewLevel();
					break;		
				case GAME_STATE_PLAY_GAME:
					gameStatePlayGame();
					break;		
				case GAME_STATE_PLAYER_DIE:
					gameStatePlayerDie();
					break;
				case GAME_STATE_GAME_OVER:
					gameStateGameOver();
					break;
			}
		}
		
		public static void switchGameState(int newState){currentGameState = newState;}

		public static void Initialize (){
			//Set up director and UISystem
			Director.Initialize ();
			UISystem.Initialize (Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene ();
			gameScene.Camera.SetViewFromViewport ();

//			Vector2 testPos = new Vector2(500f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
//			testProjectile = new Projectile(gameScene, testPos);
			
			//Run the scene.
			Director.Instance.RunWithScene (gameScene, true);
		}
		
		public static void gameStateNewGame(){
			level = 0;
			score = 0;
			//Create background
			background = new Background (gameScene);			
			//Set the ui Scene
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene ();			
			//Score implementation goes here (Milestone 2)				
			player = new Player (gameScene);				
			enemyList = new List<Enemy>();
			proj = new List<Projectile> ();	
			switchGameState(GAME_STATE_NEW_LEVEL);
		}
		
		public static void gameStateNewLevel(){		
			level++;
			enemyList = new List<Enemy>();
			levelEnemyAmount += 4 + level; 
			if (levelEnemyAmount>=9){levelEnemyAmount=9;}
			//Include level knob lol ---> Actually means the kinda difficulty level increase shizz bruh
			spawnWave();
			switchGameState(GAME_STATE_PLAY_GAME);
		}
		
		public static void spawnWave(){
			Vector2 pos = new Vector2(0, 0);
			//NEED TO SORT OUT ROTATE VALUE
			
			Vector2 rot = new Vector2(0);
			for(int i = 0; i < levelEnemyAmount; i++){
				if(i == 0){
					pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
					rot = new Vector2(0);
				}
				else if(i == 1){
					pos = new Vector2(100.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
					rot = new Vector2(0);
				}
				//else if(i == 2){pos = new Vector2(100.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				//else if(i == 3){pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				//else if(i == 4){pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				//else if(i == 5){pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				//else if(i == 6){pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				//else if(i == 7){pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				//else if(i == 8){pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				enemyList.Add(new Enemy(pos, rot));
			}
		}
		
		public static void gameStatePlayGame(){
			//checkAmmo();
			checkKeys();
			//checkForCollisions();
			Update();	
			//checkForEndOfLevel();	
		}
		
		public static void checkKeys(){
			//Determine whether the player tapped the screen
			var touches = Touch.GetData (0);
			
			//If tapped, inform the player.
			if (touches.Count > 0) 
			{
				float newX = (touches [0].X + 0.5f) * 960 - 5;
				float newY = 544 - (touches [0].Y + 0.5f) * 544 - 10;
				
				Player.sprite.Position = new Vector2 (newX, newY);		
			}
			
			var buttons = GamePad.GetData (0);
						
			//DEBUGGING PURPOSES			
			if (buttons.Buttons != 0) {
				if ((buttons.Buttons & GamePadButtons.Cross) != 0) 
				{								
					//Vector2 testPos = new Vector2(500f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
					//testProjectile = new Projectile(testPos);
					proj.Add(new Projectile(enemyList[0].sprite.Position));
				}
			}
			//END
				
		}

		public static void Update(){	
			//Update the player.
			player.update();
			
			//Update the Enemy
			for(int i = 0; i < levelEnemyAmount; i++){
				enemyList[i].update();
			}
			
			//Update Projectiles
			for (int i =0; i<proj.Count; i++) 
			{
				proj[i].update();
				if(proj[i].getAlive() == false){
					proj.RemoveAt(i);

				}
			}
		}
////		public static void TrackPlayer()
////		{
////			float xDiff = (Player.sprite.X + (Player.sprite.Quad.S.X/2)) - (sprite().Position.X + (player().Quad.S.X/2));			
//		
////			float yDiff = (sprite.Position.Y + (sprite.Quad.S.Y/2)) - (sprite().Position.Y + (sprite().Quad.S.Y/2));
////		
////			if(!(xDiff == 0 || yDiff == 0))
////			{
////				if(yDiff > 0)
////				{					
////					float angle = FMath.PI - FMath.Atan(xDiff/yDiff);				
////				 	Move(-3.0f * FMath.Sin(angle), -3.0f * -FMath.Cos(angle));			
////				}
////				else
////				{
////					float angle = FMath.Atan(xDiff/-yDiff);				
////				 	Move(-3.0f * FMath.Sin(angle), -3.0f * -FMath.Cos(angle));	
////				}	
////				Rotate(-xDiff, -yDiff);			
////			}		
////		}
	}
}

