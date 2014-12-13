using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Bullets
{
	public class Enemy
	{
		//Private variables
		public SpriteUV	sprite;
		private static TextureInfo textureInfo;
		private bool isAlive = false;
		private int rotation;
		private Vector2 direction;
		private int projectileFrameCount;	
		private double chance;	
		private double percentFire;	
		private double projFrameDelay;
		public bool Alive {get{return isAlive;} set{isAlive = value;}}
		public static int keyDDD = 0;
		public int val;
		private static Random r = new Random();
		
		public Enemy (Vector2 pos, Vector2 rot)
		{
			val = keyDDD;
			keyDDD++;
			textureInfo = new TextureInfo("/Application/textures/enemy.png");		
			sprite			= new SpriteUV(textureInfo);
			sprite.Quad.S	= textureInfo.TextureSizef;
			isAlive = true;
			sprite.Position = pos;//new Vector2(900.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			//sprite.Rotate(rot);//sprite.Rotate = new Vector2(180.0f);
			projectileFrameCount = 0;
			chance = 0;
			percentFire = 0.001;
			projFrameDelay = AppMain.lvlProjectileFrameDelay;
			//Add to the current scene.
			AppMain.gameScene.AddChild(sprite);
		}

		public void update()
		{
			projectileFrameCount++;
			direction= Player.sprite.Position- sprite.Position;
			direction.Normalize();
			
			sprite.Angle=FMath.Atan2(direction.X,-direction.Y) -90;
			//Enemy has a chance to shoot every movement
			if(isAlive == true){
				int c = r.Next(0, AppMain.lvlFireChance);
				//chance = c*AppMain.lvlFireChance;
				Console.WriteLine(chance);
				if (c/100 < percentFire) {
					fire();			
				}		
			}
		}
		
		public void fire(){
			if (projectileFrameCount > projFrameDelay){
				AppMain.proj.Add(new Projectile(this.sprite.Position));				
				projectileFrameCount = 0;	
			}			
		}
	}
}
