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
		private Vector2 direction;
		private int projectileFrameCount;	
			
		private double percentFire;	
		private double projFrameDelay;
		public static int keyDDD = 0;
		public int val;
		private static Random r = new Random();
		private Vector2 min, max;
		private Bounds2 box;
		
		public Enemy (Vector2 pos)
		{
			val = keyDDD;
			keyDDD++;
			textureInfo = new TextureInfo("/Application/textures/enemy.png");		
			sprite			= new SpriteUV(textureInfo);
			sprite.Quad.S	= textureInfo.TextureSizef;
			isAlive = true;
			sprite.Position = pos;
			projectileFrameCount = 0;
			
			percentFire = 0.001;
			projFrameDelay = AppMain.lvlProjectileFrameDelay;
			//Add to the current scene.
			AppMain.gameScene.AddChild(sprite);
		}
		
		public bool getAlive() {return isAlive;}
						
		public Bounds2 getBoundingBox(){
			min.X  = sprite.Position.X;
		    min.Y  = sprite.Position.Y;
			max.X  = sprite.Position.X + (textureInfo.TextureSizef.X);
			max.Y  = sprite.Position.Y + (textureInfo.TextureSizef.Y);
		    box.Min  = min;   
			box.Max  = max;			
			return box;
		}
		
		public void removeSprite(){
			AppMain.gameScene.RemoveChild(sprite, true);		
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
