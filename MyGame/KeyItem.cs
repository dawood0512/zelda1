using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class KeyItem
{
    public Vector2 pos;
    public bool Collected = false;

    public Rectangle bounds => new Rectangle((int)pos.X, (int)pos.Y, Game1.tilesize, Game1.tilesize);

    public KeyItem(Point tile)
    {
        pos = new Vector2(tile.X * Game1.tilesize, tile.Y * Game1.tilesize);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Collected)
        {
            spriteBatch.Draw(TextureManager.keytex, bounds, Color.White);
        }
    }
}