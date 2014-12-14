using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
namespace Bullets
{
	public class Coin
	{
		public SpriteUV	sprite;
		private static TextureInfo	textureInfo;
		public bool isAlive = false;
		public Bounds2 bounds;
		public Rectangle boundsRect;
		public bool isColliding = false;
		public static Random random = new Random();
		private Vector2 min, max;
		private Bounds2 box;
		
		
		//Public functions
		public Coin ()
		{
			this.isAlive = true;
			textureInfo = new TextureInfo("/Application/textures/blip.png");
			sprite			= new SpriteUV(textureInfo);
			sprite.Quad.S	= textureInfo.TextureSizef;
			bounds = sprite.Quad.Bounds2();
			int randomNumberX = random.Next(35, 910);
			int randomNumberY = random.Next(43, 498);
			Vector2 randomLocation = new Vector2(randomNumberX,randomNumberY);
			sprite.Position = randomLocation;
			//Add to the current scene.
			AppMain.gameScene.AddChild(sprite);
		}
		
		public Bounds2 getBoundingBox(){
			min.X  = sprite.Position.X;
		    min.Y  = sprite.Position.Y;
			max.X  = sprite.Position.X + (textureInfo.TextureSizef.X);
			max.Y  = sprite.Position.Y + (textureInfo.TextureSizef.Y);
		    box.Min  = min;   
			box.Max  = max;
			
			return box;
		}
		
		public bool getAlive(){
			return this.isAlive;
		}
		
		public void removeSprite(){
			AppMain.gameScene.RemoveChild(sprite, true);		
		}
		
		public void collide(){
			if(isColliding == false){
				AppMain.score++;
				this.isColliding = true;
				this.isAlive = false;
				removeSprite();
			}
		}

	}
}

