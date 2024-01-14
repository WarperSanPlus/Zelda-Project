using KB4_PFI_Zelda;
using KB4_PFI_Zelda_solution.Items;
using KB4_PFI_Zelda_solution.Monstres;
using KB4_PFI_Zelda_solution.UIs;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using static KB4_PFI_Zelda_solution.CutscenesHolder;

namespace KB4_PFI_Zelda_solution.Managers;

static class GameManager
{
    #region Constantes
    public const int InitLargeur = 640;
    public const int InitHauteur = 640; // 480;
    public const int UIRight = InitLargeur / 2;
    public const int UIBottom = InitHauteur / 2;
    const int PosiInitHérosX = 848;
    const int PosiInitHérosY = 1989;
    const int NbMonstres = 100;
    const int NbItems = 100;

    public const int FPS = 60;
    public const int SPF = 1000 / FPS;

    public const int TileSize = 8;
    const int MapSize = TileSize * 512;
    #endregion

    public const byte IsFadingIn = 0x0000;
    public const byte IsFadingOut = 0x0001;
    public const byte RequestLeave = 0x0002;
    public const byte LoadingZoneFound = 0x0003;

    public static readonly Dictionary<byte, bool> Flags = new Dictionary<byte, bool>()
    {
        [IsFadingIn] = false,
        [IsFadingOut] = false,
        [RequestLeave] = false,
        [LoadingZoneFound] = false,
    };

    public static RenderWindow fenetre = new RenderWindow(
        new VideoMode(InitLargeur, InitHauteur),
        Utilitaire.BaseTitle
        );

    public static Heros Link = new Heros(
        fenetre,
        "LINK",
        new Texture("images/link.png"),
        new Vector2f(PosiInitHérosX, PosiInitHérosY)
        );

    public static void Init()
    {
        PlayerControls.LoadControls("controls.ctrl");

        // Création de la fenêtre
        fenetre.Closed += (s, a) => fenetre.Close(); // Ferme la fenêtre sur l'évènement Closed

        fenetre.Resized += WindowResized;
        fenetre.GetView().Zoom(0.5f);

        fenetre.GetView().Center = Link.Position;

        #region Création de la carte du monde
        Carte monde = new Carte(
            pathTexture: "images/lemonde.bmp",
            pathObs: "images/MasqueDuMonde (version complète).bmp");
        #endregion

        #region Monstres
        List<Monstre> monstres = InitMonsters(monde, NbMonstres, Link);
        #endregion

        #region Items
        List<Item> items = InitItems(monde, NbItems, Link);
        #endregion

        // VSYNC
        Stopwatch stopwatch = new Stopwatch();

        #region UIs
        DebugUI debugUI = new DebugUI();
        FadingUI fadingUI = new FadingUI();

        List<UI> uis = new List<UI>()
        {
            debugUI,
            new ClassicUI(),
            new DiedUI(),
            //new SpeechUI(),
            fadingUI,
        };
        uis.Sort();
        #endregion

        LoadingZone[] loadingZones = new LoadingZone[]
        {
            new LoadingZone(new Vector2i(60, 50), new Vector2i(60, 1040), new Vector2f(100, 960), fadingUI, MainLW1),
            new LoadingZone(new Vector2i(80, 25), new Vector2i(60, 968), new Vector2f(95, 1095), fadingUI, LWMain1),

        };

        FloatRect[] rects =
        {
            new FloatRect(0, 0, 1, MapSize), // Left
            new FloatRect(0, MapSize, MapSize, 1), // Bottom
            new FloatRect(0, 0, MapSize, 1), // Top
            new FloatRect(MapSize, 0, 1, MapSize), // Right
            new FloatRect(0, TileSize * 128, TileSize * 128, 1), // Lost Woods Bottom
            new FloatRect(TileSize * 128, 0, 1, TileSize * 128), // Lost Woods Right
            new FloatRect(TileSize * 192, 0, 1, TileSize * 128), // Mountains Left
            new FloatRect(TileSize * 192, TileSize * 128, TileSize * 256, 1), // Mountains Bottom
            new FloatRect(TileSize * 214, TileSize * 192, TileSize * 86, 1), // Castle Top
        };

        // Boucle principale du jeu
        do
        {
            fenetre.Clear();
            stopwatch.Restart();

            #region Déplacement des entités
            // Déplacer les personnages (héros, puis monstres)
            if (Link.IsAlive)
                Link.Move(monde);

            foreach (var item in monstres)
            {
                item.Move(monde);
            }
            #endregion

            // Le ramassage d'items sont gérés par la classe Heros
            // Les attaques sont gérées par la classe Heros

            #region Affichage du monde
            fenetre.SetView(Utilitaire.GetViewWithBounds(rects, fenetre, Link));

            monde.Afficher(fenetre);
            #endregion

            #region Affichage des entités
            Link.Afficher(fenetre);

            List<Animation2> layerables = new List<Animation2>()
            {
                Link
            };

            monstres.ForEach((item) =>
            {
                layerables.Add(item);
            });

            items.ForEach((item) =>
            {
                //item.Afficher(fenetre);
                layerables.Add(item);
            });

            // Triage par Y
            layerables.Sort();

            // Monstres et Héros
            foreach (var item in layerables)
            {
                item.Afficher(fenetre);
            }
            #endregion

            #region Afficher les UIs
            foreach (var item in uis)
            {
                item.Draw(fenetre, Link);
            }
            #endregion

            // Rafraichir la fenêtre
            fenetre.Display();
            stopwatch.Stop();

            #region VSYNC
            float currentFPS = SPF - stopwatch.ElapsedMilliseconds;

            if (Utilitaire.IsInDebug)
                debugUI.FPSLabel.DisplayedString = $"{stopwatch.ElapsedMilliseconds} ms";

            Thread.Sleep((int)(currentFPS < 0 ? 0 : currentFPS));
            #endregion

            // Traiter les évènements de la fenêtre
            fenetre.DispatchEvents();
        } while (!Link.LeaveGame && fenetre.IsOpen && !Flags[RequestLeave]);

        if (!Link.LeaveGame)
        {
            Console.WriteLine("Appuyer sur n\'importe quelle touche pour quitter");
            Console.ReadKey();
        }
    }

    private static void WindowResized(object? sender, EventArgs e)
    {
        RenderWindow? fenetre = (sender as RenderWindow);

        if (fenetre == null)
            return;

        FloatRect r = fenetre.GetView().Viewport;
        r.Width = (float)fenetre.Size.Y / fenetre.Size.X;
        r.Left = (1 - r.Width) / 2;
        fenetre.GetView().Viewport = r;
    }

    private static List<Monstre> InitMonsters(Carte monde, uint count, Heros mainChar)
    {
        List<Monstre> monstres = new List<Monstre>();

        Dictionary<Type, Texture?> typesMonstre = new Dictionary<Type, Texture?>()
        {
            [typeof(Zombie)] = new Texture("images/zombie.png"),
            [typeof(Squelette)] = new Texture("images/skel.png"),
            [typeof(Dino)] = new Texture("images/dino.png"),
            [typeof(Bee)] = new Texture("images/bee.png"),
        };

        for (int i = 0; i < count; i++)
        {
            // Générer au hasard soit des Dino, des Zombie ou des Squelette
            Type type = Utilitaire.GetRandomType(typesMonstre);

            object?[] arguments = new object?[] {
                type.Name,
                typesMonstre[type],

                // Positionner les monstres au hasard sur la carte dont les dimensions sont monde.Hauteur par monde.Largeur.
                (Vector2f)monde.GetRandomPos()
            };

            Monstre? monstre = Utilitaire.CreateObject<Monstre>(type, arguments);

            if (monstre != null)
                monstres.Add(monstre);
        }

        foreach (var item in monstres)
        {
            mainChar.IAttaquableCheck += item.DamageEvent;
        }

        return monstres;
    }
    private static List<Item> InitItems(Carte monde, uint count, Heros mainChar)
    {
        List<Item> items = new List<Item>();

        Dictionary<Type, Texture?> typesItems = new Dictionary<Type, Texture?>()
        {
            [typeof(Pain)] = new Texture("images/pain.png"),
            [typeof(Epee)] = new Texture("images/epee.png"),
            [typeof(PotionHarm)] = new Texture("images/Icons-Pack.png", new IntRect(0, 128, 32, 32)),
            [typeof(NewShield)] = new Texture("images/Icons-Pack.png", new IntRect(160, 160, 32, 32))
        };

        for (int i = 0; i < count; i++)
        {
            // Générer au hasard soit des « pain » ou des « épée »
            Type type = Utilitaire.GetRandomType(typesItems);

            object?[] arguments = new object?[] {
                type.Name,
                (Vector2f)monde.GetRandomPos(),
                typesItems[type],
            };

            Item? item = Utilitaire.CreateObject<Item>(type, arguments);

            if (item != null)
                items.Add(item);
        }

        foreach (var item in items)
        {
            mainChar.IApprochableCheck += item.TakeEvent;
        }

        return items;
    }
}

