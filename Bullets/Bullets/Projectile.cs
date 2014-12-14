using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Bullets
{
	public class Projectile
	{
		public SpriteUV sprite;
		TextureInfo	textureInfo;
		public Vector2 pPosition;
		public Vector2 direction;
		private bool isAlive = false;
		public static float pVelocity = AppMain.lvlProjSpeed;
		private Vector2 min, max;
		private Bounds2 box;
		private bool isColliding = false;
		
		public Projectile (Vector2 _pPosition)
		{
			this.isAlive = true;
			sprite = new SpriteUV();
			textureInfo = new TextureInfo("/Application/textures/bullet.png");
			sprite			= new SpriteUV(textureInfo);
			this.isColliding = false;
			sprite.Quad.S	= textureInfo.TextureSizef;
			sprite.Position =_pPosition;
		
			//travel in the direction of the player WHEN THE BULLET WAS FIRST FIRED
			direction= Player.sprite.Position- _pPosition;
			sprite.Rotate(FMath.Atan2(direction.X,direction.Y));

			AppMain.gameScene.AddChild(sprite);
		}
			
		public bool getAlive(){
			return this.isAlive;
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
		
		public void collide(){
			if(isColliding == false){
				this.isColliding = true;
				this.isAlive = false;
				removeSprite();
			}
		}
		
		public void removeSprite(){
			AppMain.gameScene.RemoveChild(sprite, true);		
		}
		
		public void update(){
			if(this.isAlive == true){		
				this.sprite.Position += this.direction * Projectile.pVelocity;
		
				// if(projectile is off screen, delete
				if ((this.sprite.Position.X > Director.Instance.GL.Context.GetViewport().Width - 35) || 
					(this.sprite.Position.X < 25) ||
					(this.sprite.Position.Y > Director.Instance.GL.Context.GetViewport().Height - 55) ||
					(this.sprite.Position.Y < 55))			
				{
						this.isAlive = false;
						AppMain.gameScene.RemoveChild(sprite, true);
				}	
			}
		}
	}
}
