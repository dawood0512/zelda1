using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Door
{
    public Point TilePos;
    public bool Open;

    public Rectangle bounds => new Rectangle(TilePos.X * Game1.tilesize, TilePos.Y * Game1.tilesize, Game1.tilesize, Game1.tilesize);

    public Door(Point p)
    {
        TilePos = p;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Open ? TextureManager.dooropentex : TextureManager.doorclosedtex, bounds, Color.White);
    }
}