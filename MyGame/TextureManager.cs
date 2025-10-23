using Microsoft .Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class TextureManager
{
    //Tiles
    public static Texture2D grasstex;
    public static Texture2D watertex;
    public static Texture2D soiltex;
    public static Texture2D stonefloortex;
    public static Texture2D bridgetex;
    public static Texture2D bushgrasstex;
    public static Texture2D bushtex;
    public static Texture2D walltex;

    // //Objekt
    public static Texture2D doorclosedtex;
    public static Texture2D dooropentex;
    public static Texture2D keytex;
    public static Texture2D princesstex;

    // //spelare
    // public static Texture2D linkalltex;
    public static Texture2D frontanimtex;
    public static Texture2D backanimtex;
    public static Texture2D runanimtex;
    public static Texture2D swordtex;

    // //Fiender
    // public static Texture2D skeletonstriptex;
    public static Texture2D skeletontex;

    public static void LoadTextures(ContentManager Content)
    {
        //Tiles
        grasstex = Content.Load<Texture2D>("grass");
        watertex = Content.Load<Texture2D>("water");
        soiltex = Content.Load<Texture2D>("soil");
        stonefloortex = Content.Load<Texture2D>("stonefloor");
        bridgetex = Content.Load<Texture2D>("bridge");
        bushgrasstex = Content.Load<Texture2D>("bush");
        bushtex = Content.Load<Texture2D>("bushS");
        walltex = Content.Load<Texture2D>("wall");

        // //Objekt
        doorclosedtex = Content.Load<Texture2D>("door");
        dooropentex = Content.Load<Texture2D>("opendoor");
        keytex = Content.Load<Texture2D>("key");
        princesstex = Content.Load<Texture2D>("Zelda");

        // //spelare
        // linkalltex = Content.Load<Texture2D>("link_all");
        frontanimtex = Content.Load<Texture2D>("front_anim");
        backanimtex = Content.Load<Texture2D>("back_anim");
        runanimtex = Content.Load<Texture2D>("running_anim");
        swordtex = Content.Load<Texture2D>("sword_anim");

        // //Fiender
        // skeletonstriptex = Content.Load<Texture2D>("skelet_anim");
        skeletontex = Content.Load<Texture2D>("skelett");
    }
}