using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Bullets
{
	public class Player
	{
		//Private variables
		public static SpriteUV		sprite;
		private static TextureInfo	textureInfo;
		private Vector2 min, max;
		private Bounds2 box;
		
		public Bounds2 getBoundingBox(){
			min.X  = sprite.Position.X;
		    min.Y  = sprite.Position.Y;
			max.X  = sprite.Position.X + (textureInfo.TextureSizef.X);
			max.Y  = sprite.Position.Y + (textureInfo.TextureSizef.Y);
		    box.Min  = min;   
			box.Max  = max;
			return box;
		}
		
		//Public functions
		public Player (Scene scene)
		{
			textureInfo = new TextureInfo("/Application/textures/player.png");
			sprite			= new SpriteUV(textureInfo);
			sprite.Quad.S	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
//			sprite.Scale = new Vector2(Director.Instance.GL.Context.Screen.Width,
//                                       Director.Instance.GL.Context.Screen.Height);			
			//Add to the current scene.
			scene.AddChild(sprite);
		}

		public void collide(){
			AppMain.switchGameState(80);	
		}

		public SpriteUV getSprite(){
			return sprite;	
		}
		
		public void update()
		{
			
			
		}
	}
}

