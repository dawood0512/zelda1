using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

namespace MyGame;

public enum GameState
{
    Start, Playing, Win, GameOver
}

public enum Direction
{
    Up, Down, Left, Right
}

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    public static Tile[,] tileArray;
    public static int tilesize = 40;

    private GameState state = GameState.Start;

    public Player player;
    private readonly List<Enemy> enemies = new();
    private KeyItem keyItem;
    private Door door;
    private Princess princess;

    private static Door doorStaticref;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureManager.LoadTextures(Content);

        // TODO: use this.Content to load your game content here

        graphics.PreferredBackBufferWidth = 1510;
        graphics.PreferredBackBufferHeight = 920;
        graphics.ApplyChanges();

        CreateLevel("maze.txt");
        state = GameState.Start;
        RefreshTitle();
    }

    public List<string> ReadFromFile(string Filename)
    {
        StreamReader sr = new StreamReader(Filename);
        List<string> result = new List<string>();

        while (!sr.EndOfStream)
        {
            result.Add(sr.ReadLine());
        }
        sr.Close();
        return result;
    }

    public void CreateLevel(string Filename)
    {
        List<string> level = ReadFromFile(Filename);

        enemies.Clear();
        tileArray = new Tile[level[0].Length, level.Count];

        Point? playerStart = null;
        List<Point> enemySpawns = new();
        Point? keyTile = null;
        Point? doorTile = null;
        Point? princessTile = null;

        for (int i = 0; i < level.Count; i++)
        {
            for (int j = 0; j < level[0].Length; j++)
            {
                if (level[i][j] == 'G')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.grasstex, false);
                }
                else if (level[i][j] == 'W')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.watertex, true);
                }
                else if (level[i][j] == 'S')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.soiltex, false);
                }
                else if (level[i][j] == 'F')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.stonefloortex, false);
                }
                else if (level[i][j] == 'A')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.bridgetex, false);
                }
                else if (level[i][j] == 'B')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.bushgrasstex, true);
                }
                else if (level[i][j] == 'C')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.bushtex, true);
                }
                else if (level[i][j] == 'V')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.walltex, true);
                }
                else if (level[i][j] == 'L')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.grasstex, false); playerStart = new Point(j, i);
                }
                else if (level[i][j] == 'E')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.grasstex, false); enemySpawns.Add(new Point(j, i));
                }
                else if (level[i][j] == 'K')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.grasstex, false); keyTile = new Point(j, i);
                }
                else if (level[i][j] == 'D')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.stonefloortex, false); doorTile = new Point(j, i);
                }
                else if (level[i][j] == 'P')
                {
                    tileArray[j, i] = new Tile(new Vector2(j * tilesize, i * tilesize), TextureManager.stonefloortex, false); princessTile = new Point(j, i);
                }
            }
        }
        player = new Player(playerStart ?? new Point(2, 2));
        keyItem = keyTile.HasValue ? new KeyItem(keyTile.Value) : null;
        door = doorTile.HasValue ? new Door(doorTile.Value) : null;
        princess = princessTile.HasValue ? new Princess(princessTile.Value) : null;

        for (int i = 0; i < enemySpawns.Count; i++)
        {
            bool horizontal = (i % 2 == 0);
            int patrolTiles = 4 + (i % 3);
            float speed = (i % 2 == 0) ? 1.0f : 0.5f;
            enemies.Add(new Enemy(enemySpawns[i], patrolTiles, horizontal, speed));
        }
    }
    
    public static bool IsWalkable(Point p)
    {
        if (p.X < 0 || p.Y < 0 || p.X >= tileArray.GetLength(0) || p.Y >= tileArray.GetLength(1))
        {
            return false;
        }

        bool blockedTile = tileArray[p.X, p.Y].notWalkable;
        bool blockedDoor = doorStaticref != null && !doorStaticref.Open && doorStaticref.TilePos == p;
        return !(blockedTile || blockedDoor);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyMouseReader.Update();
        doorStaticref = door;

        switch (state)
        {
            case GameState.Start:
                if (KeyMouseReader.KeyPressed(Keys.Enter))
                {
                    state = GameState.Playing;
                }
                break;

            case GameState.Playing:
                player.Update(gameTime);
                foreach (var e in enemies)
                {
                    
                    e.Update(gameTime);
                
                }

                if (KeyMouseReader.KeyPressed(Keys.Space))
                {
                    player.StartAttack();
                }

                if (player.isAttacking)
                {
                    Rectangle swordHit = player.GetSwordHitbox();
                    foreach (var e in enemies)
                    {
                        if (e.Alive && e.Bounds.Intersects(swordHit))
                        {
                            e.Alive = false;
                            break;
                        }
                    }
                }

                if (keyItem != null && !keyItem.Collected && player.Bounds.Intersects(keyItem.bounds))
                {
                    keyItem.Collected = true;
                    player.HasKey = true;
                }

                if (door != null && !door.Open && player.HasKey && Math.Abs(player.TilePos.X - door.TilePos.X) + Math.Abs(player.TilePos.Y - door.TilePos.Y) == 1)
                {
                    door.Open = true;
                }

                foreach (var e in enemies)
                {
                    if (e.Alive && e.Bounds.Intersects(player.Bounds) && player.canBeHurt)
                    {
                        var pc = player.Bounds.Center;
                        var ec = e.Bounds.Center;
                        Point dir = (Math.Abs(pc.X - ec.X) > Math.Abs(pc.Y - ec.Y))
                            ? (pc.X > ec.X ? new Point(1, 0) : new Point(-1, 0))
                            : (pc.Y > ec.Y ? new Point(0, 1) : new Point(0, -1));

                        player.ApplyDamage(1, dir);

                        if (player.HP <= 0)
                        {
                            state = GameState.GameOver;
                        }
                        break;
                    }
                }

                if (door != null && door.Open && princess != null && player.TilePos == princess.TilePos)
                {
                    state = GameState.Win;
                }
                break;

            case GameState.Win:
            case GameState.GameOver:
                if (KeyMouseReader.KeyPressed(Keys.R))
                {
                    LoadContent();
                    state = GameState.Start;
                }
                break;
        }

        RefreshTitle();

        base.Update(gameTime);
    }

    private void RefreshTitle()
    {
        string hp = (player != null) ? player.HP.ToString() : "-";
        string haskey = (player != null && player.HasKey) ? "Yes" : "No";

        string stateText = state switch
        {
            GameState.Start => "Press ENTER to start",
            GameState.Playing => "Find the key, open the castle, save the princess",
            GameState.Win => "You saved the princess! Press R to play again",
            GameState.GameOver => "Game Over - Press R to Restart",
            _ => ""
        };

        Window.Title = $"HP: {hp} | Key: {haskey} - {stateText}";

    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();

        foreach (Tile tile in tileArray)
        {
            tile.Draw(spriteBatch);
        }
        keyItem?.Draw(spriteBatch);
        door?.Draw(spriteBatch);
        princess?.Draw(spriteBatch);
        foreach (var e in enemies)
        {
            e.Draw(spriteBatch);
        }
        player.Draw(spriteBatch);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
