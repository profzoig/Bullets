using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Bullets
{
	public class Player
	{
		//Private variables
		private static SpriteUV		sprite;
		private static TextureInfo	textureInfo;
		private static bool 		alive;
		public Vector2 			position;
		
		public bool Alive { get{return alive;} set{alive = value;} }
		
		//Public functions
		public Player (Scene scene)
		{
			textureInfo = new TextureInfo("/Application/textures/player.png");
			
			sprite			= new SpriteUV();
			sprite			= new SpriteUV(textureInfo);
			sprite.Quad.S	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(50.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			
			//Add to the current scene.
			scene.AddChild(sprite);
		}
		
		public static void Update()
		{
			
		}
	}
}
