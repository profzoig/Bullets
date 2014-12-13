using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
namespace Bullets
{
	public class Coin
	{
		public static SpriteUV		sprite;
		private static TextureInfo	textureInfo;
		public bool 		alive;
		public Bounds2 bounds;
		public Rectangle boundsRect;
		public static Random random = new Random();
		//Public functions
		public Coin ()
		{
			textureInfo = new TextureInfo("/Application/textures/blip.png");
			sprite			= new SpriteUV(textureInfo);
			sprite.Quad.S	= textureInfo.TextureSizef;
			bounds = sprite.Quad.Bounds2();
			int randomNumberX = random.Next(35, 910);
			int randomNumberY = random.Next(43, 498);
			Vector2 randomLocation = new Vector2(randomNumberX,randomNumberY);
			sprite.Position = randomLocation;
			boundsRect = new Rectangle(sprite.Position.X, sprite.Position.Y, textureInfo.Texture.Width, textureInfo.Texture.Height);
			//Add to the current scene.
			AppMain.gameScene.AddChild(sprite);
		}
		public bool getAlive(){
			return this.alive;
		}
		
		public void removeSprite(){
			AppMain.gameScene.RemoveChild(sprite, true);		
		}
		
		public void Update()
		{

			
		}
	}
}

