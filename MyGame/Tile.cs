using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Tile
{
    public Vector2 pos;
    public Texture2D tex;
    public bool notWalkable;

    public Tile(Vector2 pos, Texture2D tex, bool notWalkable)
    {
        this.pos = pos;
        this.tex = tex;
        this.notWalkable = notWalkable;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(tex, pos, Color.White);
    }
}