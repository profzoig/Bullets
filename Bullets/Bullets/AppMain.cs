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
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
				
		public static Player player;
		public static Enemy enemy;
		private static Background background;
		public static List<Projectile> proj;
		public static List<Enemy> enemyList;
		public static List<Coin> coinCol;
		
		public static int score;
		public static int level; 
		public static int levelEnemyAmount;
		public static bool gamePause;

		//Game states which make up the game engine
		private const int GAME_STATE_TITLE = 10;
		private const int  GAME_STATE_MENU = 20;
		private const int  GAME_STATE_HTP = 30;
		private const int  GAME_STATE_NEW_GAME = 40;
		private const int  GAME_STATE_NEW_LEVEL = 50;
		private const int  GAME_STATE_PLAY_GAME = 60;
		private const int  GAME_STATE_PLAYER_DIE = 70;
		private const int  GAME_STATE_GAME_OVER = 80;
		
		//Start/Gameover Sprites
		public static SpriteUV		startSprite;
		private static TextureInfo	starttextureInfo;
		
		public static SpriteUV		gameOverSprite;
		private static TextureInfo	gameOvertextureInfo;
		
		public static float lvlProjSpeed = 0.02f;
		public static double lvlProjectileFrameDelay = 25;
	 	public static int lvlFireChance = 301;
		
		public static int currentGameState = 0;
		
		

			
		public static void Main (string[] args)
		{
			Initialize ();
			//Switch the game state to title
			switchGameState(GAME_STATE_TITLE);
			gamePause = false;
			//Game Loop
			bool quitGame = false;
			while (!quitGame) {		
				if(gamePause == false){
					Director.Instance.Update ();
					runGame();
					var buttons = GamePad.GetData (0);
						
					//DEBUGGING PURPOSES			

						if ((buttons.Buttons & GamePadButtons.Start) != 0) 
						{
							gamePause = true;
						//	Scheduler.Instance.ScheduleUpdateForTarget(appMain,0,false);
						}
				}
				else{
					var buttons = GamePad.GetData (0);
						
								

						if ((buttons.Buttons & GamePadButtons.Start) != 0) 
						{
							gamePause = false;
							//Scheduler.Instance.ScheduleUpdateForTarget(gameScene,0,true);
						}
					
			}


				Director.Instance.Render ();
				UISystem.Render ();			
				Director.Instance.GL.Context.SwapBuffers ();
				Director.Instance.PostSwap ();

			}
			Director.Terminate();
		}
		
		public static void gameStateTitle(){

			starttextureInfo = new TextureInfo("/Application/textures/startsprite.png");
			startSprite			= new SpriteUV(starttextureInfo);
			startSprite.Quad.S	= starttextureInfo.TextureSizef;
			startSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			gameScene.AddChild(startSprite);
			//gameScene.RemoveChild(startSprite, true);
			
			switchGameState(GAME_STATE_MENU);
			
		}


		public static void gameStateMenu(){
		
			var buttons = GamePad.GetData (0);
				
			if (buttons.Buttons != 0) {
				if ((buttons.Buttons & GamePadButtons.Cross) != 0) 
				{
			Console.WriteLine ("DONE");
			switchGameState(GAME_STATE_NEW_GAME);
				}
			
		}
			
		}
		public static void gameStateHTP(){}
		public static void gameStateGameOver(){
			
			gameOvertextureInfo = new TextureInfo("/Application/textures/gameoversprite.png");
			gameOverSprite			= new SpriteUV(gameOvertextureInfo);
			gameOverSprite.Quad.S	= gameOvertextureInfo.TextureSizef;
			gameOverSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			gameScene.AddChild(gameOverSprite);
			
			
			switchGameState(GAME_STATE_MENU);
			
			
			
		}
		
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
			
						//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Height*0.05f - scoreLabel.Height/2);
			scoreLabel.Text = "Score: " + score;
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);

//			Vector2 testPos = new Vector2(500f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
//			testProjectile = new Projectile(gameScene, testPos);			
			//Run the scene.
			Director.Instance.RunWithScene (gameScene, true);
		}
		
		public static void gameStateNewGame(){
			level = 0;
			score = 0;
			levelEnemyAmount = 0;
			//Create background
			background = new Background (gameScene);			
			//Set the ui Scene
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene ();			
			//Score implementation goes here (Milestone 2)				
			player = new Player (gameScene);				
			enemyList = new List<Enemy>();
			proj = new List<Projectile> ();	
			coinCol = new List<Coin>();
			switchGameState(GAME_STATE_NEW_LEVEL);
		}
		
		public static void gameStateNewLevel(){		
			clearLevel();
			level++;
			enemyList = new List<Enemy>();
			coinCol = new List<Coin>();
			levelEnemyAmount +=2; 
			if (levelEnemyAmount>=8){levelEnemyAmount=8;}
			//lvlFireChance = 301 - 1*level;
			//if(lvlFireChance >= 151){lvlFireChance = 151;}

			//lvlProjectileFrameDelay = 25 - 1*level;
			//if (lvlProjectileFrameDelay<=15) {lvlProjectileFrameDelay=15;}
	
		//	lvlProjSpeed=3+.5*level;
			//if(lvlProjSpeed > 8){lvlProjSpeed = 8;}	
			
			//Include Level Changer
			spawnWave();
			spawnCoins();
			switchGameState(GAME_STATE_PLAY_GAME);
		}
		
		public static void clearLevel(){
			for(int i = 0; i < enemyList.Count; i++){enemyList[i].removeSprite();}
			for(int i = 0; i < coinCol.Count; i++){coinCol[i].removeSprite();}
		}
		
		public static void spawnCoins(){
			Random r = new Random();
			int coinAmount = r.Next(5, 30);
			Console.WriteLine(coinAmount);
			
			for (int i = 0; i< coinAmount; i++){coinCol.Add(new Coin());}			
		}
		
		public static void spawnWave(){
			Vector2 pos = new Vector2(0, 0);
			//NEED TO SORT OUT ROTATE VALUE
			
			Vector2 rot = new Vector2(0);
			for(int i = 0; i < levelEnemyAmount; i++){
				if(i == 0){
					pos = new Vector2(900.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
				}
				else if(i == 1){
					pos = new Vector2(100.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
				}
				else if(i == 2){pos = new Vector2(800.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				else if(i == 3){pos = new Vector2(200.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				else if(i == 4){pos = new Vector2(600.0f, Director.Instance.GL.Context.GetViewport().Height*0.4f);}
				else if(i == 5){pos = new Vector2(400.0f, Director.Instance.GL.Context.GetViewport().Height*0.4f);}
				else if(i == 6){pos = new Vector2(600.0f, Director.Instance.GL.Context.GetViewport().Height*0.6f);}
				else if(i == 7){pos = new Vector2(400.0f, Director.Instance.GL.Context.GetViewport().Height*0.6f);}
				else if(i == 8){pos = new Vector2(500.0f, Director.Instance.GL.Context.GetViewport().Height*0.5f);}
				enemyList.Add(new Enemy(pos));
			}
		}
		
		public static void gameStatePlayGame(){
			//checkAmmo();
			checkKeys();
			checkForCollisions();
			Update();	
			checkForEndOfLevel();	
		}
		
		public static void checkForEndOfLevel(){
			int allDead = 0;
			for(var i = 0; i < coinCol.Count; i++){
				if (coinCol[i].isAlive == false){allDead++;}
			}
			if(allDead == coinCol.Count){switchGameState(GAME_STATE_NEW_LEVEL);}
		}
		
		public static void checkForCollisions(){
			coinHitDetection();
			projectileHitDetection();
			enemyHitDetection();
		}
		
		public static void projectileHitDetection(){
			//enemyProjectile Check
			
			Bounds2 b1;
			Bounds2 b2;
			for(var i = 0; i < proj.Count; i++){
				if(proj[i].getAlive() == true){
					b1 = player.getBoundingBox();
					b2 = proj[i].getBoundingBox();
					bool b = b1.Overlaps(b2);
					if(b == true){						
						proj[i].collide();
						player.collide ();
					}
				}
			}
		}
		
		public static void coinHitDetection(){
			//coin Check
			
			Bounds2 b1;
			Bounds2 b2;
			for(var i = 0; i < coinCol.Count; i++){
				if(coinCol[i].getAlive() == true){
					b1 = player.getBoundingBox();
					b2 = coinCol[i].getBoundingBox();
					bool b = b1.Overlaps(b2);
					if(b == true){						
						coinCol[i].collide();
					}
				}
			}
		}
		
		public static void enemyHitDetection(){
			//enemyProjectile Check			
			Bounds2 b1;
			Bounds2 b2;
			for(var i = 0; i < enemyList.Count; i++){
				if(enemyList[i].getAlive() == true){
					b1 = player.getBoundingBox();
					b2 = enemyList[i].getBoundingBox();
					bool b = b1.Overlaps(b2);
					if(b == true){						
						player.collide();
					}
				}
			}
		}
		
		


		public static void checkKeys(){
			//Determine whether the player tapped the screen
			var touches = Touch.GetData (0);
			scoreLabel.Text = "Score: " + score;scoreLabel.Text = "Score: " + score;
			//If tapped, inform the player.
			if (touches.Count > 0) 
			{
				float newX = (touches [0].X + 0.5f) * 960 - 5;
				float newY = 544 - (touches [0].Y + 0.5f) * 544 - 10;
				
				Player.sprite.Position = new Vector2 (newX, newY);		
			}
			
			var buttons = GamePad.GetData (0);
						
//			//DEBUGGING PURPOSES TO MAKE THE ENEMY FIRE ON COMMAND		
//			if (buttons.Buttons != 0) {
//				if ((buttons.Buttons & GamePadButtons.Cross) != 0) 
//				{								
//					//Vector2 testPos = new Vector2(500f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
//					//testProjectile = new Projectile(testPos);
//					//proj.Add(new Projectile(enemyList[0].sprite.Position));
//				}
//			}
//			//END
				
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

	}
}