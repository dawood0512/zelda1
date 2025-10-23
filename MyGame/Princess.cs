using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Princess
{
    public Point TilePos;
    public Rectangle bounds => new Rectangle(TilePos.X * Game1.tilesize, TilePos.Y * Game1.tilesize, Game1.tilesize, Game1.tilesize);
    public Princess(Point p)
    {
        TilePos = p;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(TextureManager.princesstex, bounds, Color.White);
    }
}