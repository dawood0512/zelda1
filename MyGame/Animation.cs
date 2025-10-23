using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Animation
{
    private readonly Texture2D tex;
    private readonly int frameWidth;
    private readonly int frameHeight;
    private readonly int frameCount;
    private float fps;
    private float timer;

    public Animation(Texture2D tex, int frameWidth, int frameHeight, int frames, float fps = 10f)
    {
        this.tex = tex;
        this.frameWidth = frameWidth;
        this.frameHeight = frameHeight;
        this.frameCount = frames;
        this.fps = fps;
        timer = 0f;
    }

    public void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds * fps;
    }

    public void ResetToFrame(int index)
    {
        if (frameCount <= 0)
        {
            timer = 0f;
            return;
        }
        index = ((index % frameCount) + frameCount) % frameCount;
        timer = index + 0.0001f;
    }

        //utan flipp
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        int frameindex = (int)timer % frameCount;
        var src = new Rectangle(frameindex * frameWidth, 0, frameWidth, frameHeight);
        var dst = new Rectangle((int)position.X, (int)position.Y, Game1.tilesize, Game1.tilesize);
        spriteBatch.Draw(tex, dst, src, Color.White);
    }

        //med flipp
    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects)
    {
        int frameindex = (int)timer % frameCount;
        var src = new Rectangle(frameindex * frameWidth, 0, frameWidth, frameHeight);
        var dst = new Rectangle((int)position.X, (int)position.Y, Game1.tilesize, Game1.tilesize);
        spriteBatch.Draw(tex, dst, src, Color.White, 0f, Vector2.Zero, effects, 0f);
    }
}