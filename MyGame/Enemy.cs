using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Enemy
{
    public static float GlobalspeedScale = 6f;
    public bool Alive { get; set; } = true;

    public Vector2 pos;

    private readonly float speedTilesPerSec;
    private int direction = 1;

    private readonly bool horizontal;
    private readonly int minpix, maxpix;

    public Rectangle Bounds =>
        new Rectangle((int)pos.X, (int)pos.Y, Game1.tilesize, Game1.tilesize);

    public Enemy(Point startTile, int patrolTiles, bool horizontal, float speed)
    {
        int sx = startTile.X * Game1.tilesize;
        int sy = startTile.Y * Game1.tilesize;
        pos = new Vector2(sx, sy);

        this.horizontal = horizontal;
        this.speedTilesPerSec = speed;

        if (horizontal)
        {
            minpix = sx;
            maxpix = sx + patrolTiles * Game1.tilesize;
        }
        else
        {
            minpix = sy;
            maxpix = sy + patrolTiles * Game1.tilesize;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (!Alive) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        float pixelPerSecond = speedTilesPerSec * Game1.tilesize * GlobalspeedScale;
        float deltaPix = pixelPerSecond * dt;

        float maxStep = Game1.tilesize * 0.25f;
        if (deltaPix > maxStep) deltaPix = maxStep;

        if (horizontal)
        {
            //X-Led
            pos.X += direction * deltaPix;
            if (pos.X > maxpix)
            {
                pos.X = maxpix;
                direction = -1;
            }

            if (pos.X < minpix)
            {
                pos.X = minpix;
                direction = 1;
            }
        }
        //Y-led
        else
        {
            pos.Y += direction * deltaPix;
            if (pos.Y > maxpix)
            {
                pos.Y = maxpix;
                direction = -1;
            }

            if (pos.Y < minpix)
            {
                pos.Y = minpix;
                direction = 1;
            }
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Alive) return;
        spriteBatch.Draw(TextureManager.skeletontex, Bounds, Color.White);
    }
}