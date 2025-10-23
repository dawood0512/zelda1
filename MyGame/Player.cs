
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame;

public class Player
{
    public Point TilePos { get; private set; }
    public Direction Facing = Direction.Down;
    public int HP = 3;
    public bool HasKey = false;

    private bool issteping = false;
    private Vector2 pixelpos;
    private Vector2 startpixel, targetpixel;
    private float stepT = 0f;
    private float stepduration = 0.12f;

    private bool attacking = false;
    private float attackT = 0f;
    private readonly float attackDuration = 0.28f;
    private const int AttackFrameSize = 40;
    private const int AttackFrames = 8;

    private float hurtCooldown = 0f;
    private const float hurtIframes = 0.6f;
    public bool canBeHurt => hurtCooldown <= 0f;
    public bool isAttacking => attacking;

    public Rectangle Bounds =>
        new Rectangle((int)pixelpos.X, (int)pixelpos.Y, Game1.tilesize, Game1.tilesize);

    private readonly Animation animDown = new Animation(TextureManager.frontanimtex, 40, 40, 8, 10f);
    private readonly Animation animup = new Animation(TextureManager.backanimtex, 40, 40, 8, 10f);
    private readonly Animation animside = new Animation(TextureManager.runanimtex, 40, 40, 8, 12f);

    public Player(Point startTile)
    {
        TilePos = startTile;
        pixelpos = new Vector2(TilePos.X * Game1.tilesize, TilePos.Y * Game1.tilesize);
    }

    private void UpdateFacingFromKeys()
    {
        var ks = Keyboard.GetState();
        if (ks.IsKeyDown(Keys.Up))
        {
            Facing = Direction.Up;
        }
        else if (ks.IsKeyDown(Keys.Down))
        {
            Facing = Direction.Down;
        }
        else if (ks.IsKeyDown(Keys.Left))
        {
            Facing = Direction.Left;
        }
        else if (ks.IsKeyDown(Keys.Right))
        {
            Facing = Direction.Right;
        }
    }

    public void StartAttack()
    {
        if (attacking) return;

        UpdateFacingFromKeys();

        attacking = true;
        attackT = 0f;

        issteping = false;
        stepT = 0f;
        startpixel = targetpixel = pixelpos;
    }

    public void Update(GameTime gameTime)
    {

        if (!issteping && !attacking)
        {
            var ks = Keyboard.GetState();
            Point dir = Point.Zero;

            if (ks.IsKeyDown(Keys.Up))
            {
                dir = new Point(0, -1);
                Facing = Direction.Up;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                dir = new Point(0, 1);
                Facing = Direction.Down;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                dir = new Point(-1, 0);
                Facing = Direction.Left;
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                dir = new Point(1, 0);
                Facing = Direction.Right;
            }

            if (dir != Point.Zero)
            {
                TryStartStep(dir);
            }
            else
            {
                animDown.ResetToFrame(0);
                animup.ResetToFrame(0);
                animside.ResetToFrame(0);
            }
        }

        if (issteping)
        {
            stepT += (float)gameTime.ElapsedGameTime.TotalSeconds / stepduration;
            pixelpos = Vector2.Lerp(startpixel, targetpixel, MathHelper.SmoothStep(0, 1, stepT));

            if (stepT >= 1f)
            {
                issteping = false;
                stepT = 0f;
                pixelpos = targetpixel;
                TilePos = new Point((int)(pixelpos.X / Game1.tilesize), (int)(pixelpos.Y / Game1.tilesize));
            }
        }

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (attacking)
        {
            attackT += dt;
            if (attackT >= attackDuration)
            {
                attacking = false;
                attackT = 0f;
            }
        }

        if (hurtCooldown > 0f)
        {
            hurtCooldown -= dt;
        }

        if (issteping && !attacking)
        {
            switch(Facing)
            {
                case Direction.Down: animDown.Update(gameTime); break;
                case Direction.Up: animup.Update(gameTime); break;
                case Direction.Left: 
                case Direction.Right: animside.Update(gameTime); break;
            }
        }
    }

    private void TryStartStep(Point dir)
    {
        Point next = new Point(TilePos.X + dir.X, TilePos.Y + dir.Y);
        if (Game1.IsWalkable(next))
        {
            issteping = true;
            startpixel = pixelpos;
            targetpixel = new Vector2(next.X * Game1.tilesize, next.Y * Game1.tilesize);
        }
    }

        //knockback
    public void ForceToTile(Point p)
    {
        TilePos = p;
        pixelpos = new Vector2(p.X * Game1.tilesize, p.Y * Game1.tilesize);
        issteping = false;
        stepT = 0f;
    }

    public Rectangle GetSwordHitbox()
    {
        int r = Game1.tilesize;
        var b = Bounds;
        return Facing switch
        {
            Direction.Up => new Rectangle(b.X, b.Y - r / 2, b.Width, r / 2),
            Direction.Down => new Rectangle(b.X, b.Bottom, b.Width, r / 2),
            Direction.Left => new Rectangle(b.X - r / 2, b.Y, r / 2, b.Height),
            Direction.Right => new Rectangle(b.Right, b.Y, r / 2, b.Height),
            _ => b
        };
    }

        //player träffar enemy 
    public void ApplyDamage(int amount, Point knockbackDir)
    {
        if (hurtCooldown > 0f) return;
        HP -= amount;
        hurtCooldown = hurtIframes;

        Point kb = new Point(TilePos.X + knockbackDir.X, TilePos.Y + knockbackDir.Y);
        if (Game1.IsWalkable(kb)) ForceToTile(kb);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        //attackanimation
        if (attacking)
        {
            if (Facing == Direction.Left || Facing == Direction.Right)
            {
                int frame = (int)(attackT / attackDuration * AttackFrames);
                if (frame < 0) frame = 0;
                if (frame >= AttackFrames) frame = AttackFrames - 1;

                Rectangle src = new Rectangle(frame * AttackFrameSize, 0, AttackFrameSize, AttackFrameSize);
                float rotation = 0f;
                SpriteEffects flip = (Facing == Direction.Right)
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                Vector2 center = pixelpos + new Vector2(Game1.tilesize * 0.5f, Game1.tilesize * 0.5f);
                Vector2 origin = new Vector2(AttackFrameSize * 0.5f, AttackFrameSize * 0.5f);

                spriteBatch.Draw(TextureManager.swordtex, position: center, sourceRectangle: src, color: Color.White, rotation: rotation, origin: origin, scale: 1f, effects: flip, layerDepth: 0f);

                return;
            }
        }

        if (issteping)
        {
            //rörelseanimation
            switch (Facing)
            {
                case Direction.Down:
                    animDown.Draw(spriteBatch, pixelpos);
                    break;
                case Direction.Up:
                    animup.Draw(spriteBatch, pixelpos);
                    break;
                case Direction.Left:
                    animside.Draw(spriteBatch, pixelpos);
                    break;
                case Direction.Right:
                    animside.Draw(spriteBatch, pixelpos, SpriteEffects.FlipHorizontally);
                    break;
            }
        }
        else
        {
            var idleSrc = new Rectangle(0, 0, 40, 40);
            switch (Facing)
            {
                case Direction.Down:
                    spriteBatch.Draw(TextureManager.frontanimtex, Bounds, idleSrc, Color.White);
                    break;
                case Direction.Up:
                    spriteBatch.Draw(TextureManager.backanimtex, Bounds, idleSrc, Color.White);
                    break;
                case Direction.Left:
                    spriteBatch.Draw(TextureManager.runanimtex, Bounds, idleSrc, Color.White);
                    break;
                case Direction.Right:
                    spriteBatch.Draw(TextureManager.runanimtex, Bounds, idleSrc, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    break;
            }
        }
    }
}