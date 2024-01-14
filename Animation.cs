////////////////////////////////////////////////////////////
// Animation.h
// 
// Déclaration de la classe Animation qui représente
// et permet de manipuler une animation
//
// XXX 
// Création: 28 avril 2010
// Version 1.0
// Version 1.1 - Bloquer la copie
// Version 2.0 - CAnimation - Dérive de Sprite - 5 mai 2014
// Version 3.0 - Traduction en C# - 29 avril 2020
////////////////////////////////////////////////////////////

//--------------------------------------------------------//
// Animation
//--------------------------------------------------------//
using SFML.Graphics;
using SFML.System;
using System.Xml;

namespace KB4_PFI_Zelda;

// [CLASSE FOURNIE]
// ~2708,363392 ticks
// ~4270,47564 ticks irl
// ~3522,1178 ticks (100 zombies)
// ~587,97576 ticks (Only Afficher())
// ~2883,151786 ticks (Final test)
class Animation : Sprite, IComparable<Animation>
{
    // Nombre d'images dans l'animation
    private int nbImages;
    // Vitesse de l'animation 
    private int vitesse;
    // Numéro de l'image
    private int compteur = 0;
    // Hauteur en pixels d'une image
    private int imageHauteur;
    // Largeur en pixels d'une image
    private int imageLargeur;
    // Position précédente
    private Vector2f anciennePosition;

    ////////////////////////////////////////////////////////////
    // Animation
    // Constructeur
    //
    // Intrants : Texture, position, NbImages par direction et vitesse
    // La texture doit contenir autant de directions que d'images 
    // Exemple: 4x4
    //
    ////////////////////////////////////////////////////////////
    public Animation(Texture? texture, Vector2f pos, int images = 4, int vit = 3) : base(texture)
    {
        nbImages = images;
        vitesse = vit;

        Position = pos; // Position héritée de Sprite
        if (texture is null)
        {
            imageLargeur = 0;
            imageHauteur = 0;
        }
        else
        {
            imageLargeur = (int)texture.Size.X / nbImages;
            imageHauteur = (int)texture.Size.Y / nbImages;
        }

        Origin = new Vector2f(imageLargeur / 2, imageHauteur); // Les pieds
    }
    ////////////////////////////////////////////////////////////
    // Afficher
    // Méthode qui affiche l'animation dans
    // la fenêtre. 
    // Intrant: Fenetre dans laquelle il faut afficher  
    //          l'animation
    //
    ////////////////////////////////////////////////////////////
    public virtual void Afficher(RenderWindow fenetre)
    {
        // Si la position est différente, changer d'image
        if (!anciennePosition.Equals(Position))
        {
            compteur++;
            anciennePosition = Position; // référence? - non struct 
        }

        // Calcul du rectangle source
        if (Dir != Direction.NEUTRAL)
        {
            LastDir = Dir;
        }

        TextureRect = new IntRect(imageLargeur * (compteur / vitesse % nbImages),
            imageHauteur * (int)LastDir,
            imageLargeur,
            imageHauteur);

        // Affichage
        fenetre.Draw(this);
    }

    public int CompareTo(Animation? other)
        => other == null || other.Position.Y > this.Position.Y ? -1 : 1;

    ////////////////////////////////////////////////////////////
    // Direction courante de l'amimation
    ////////////////////////////////////////////////////////////
    public enum Direction // Pour clarifier le code
    {
        Bas, Gauche, Droite, Haut, // cet ordre est important car il est identique à l'ordre des images dans le spritesetà
        NEUTRAL,
    };
    private Direction LastDir = Direction.Bas;
    public virtual Direction Dir { get; protected set; } = Direction.NEUTRAL;
}

// [CLASSE CRÉÉE PAR WarperSan]
// Plus lent que Animation, mais offre une plus grande liberté
// 
// ~2691,092998 ticks
// ~4850,06866 ticks irl
// ~3961,59524 ticks (100 zombies)
// ~965,41648 ticks (Only Afficher())
// ~3530,949638 ticks (Final test)
class Animation2 : Sprite, IComparable<Animation2>
{
    public int CompareTo(Animation2? other)
        => other == null || other.Position.Y > this.Position.Y ? -1 : 1;

    class Frame : IComparable<Frame>
    {
        public string? Name { get; private set; }
        public IntRect Rect { get; private set; }

        public Frame(int width, int height, XmlNode FrameNode)
        {
            if (FrameNode.Attributes == null || FrameNode.Attributes.Count != 3)
                return;

            IntRect rect = new IntRect(0, 0, width, height);

            XmlAttribute? x = FrameNode.Attributes["x"];
            if (x != null)
                rect.Left = int.Parse(x.Value);

            XmlAttribute? y = FrameNode.Attributes["y"];
            if (y != null)
                rect.Top = int.Parse(y.Value);

            XmlAttribute? name = FrameNode.Attributes["name"];
            if (name != null)
                Name = name.Value.ToUpper();

            Rect = rect;
        }

        public int CompareTo(Frame? other)
            => other != null ? string.Compare(this.Name, other.Name) : -1;
    }

    Dictionary<string, List<Frame>> animations = new Dictionary<string, List<Frame>>();

    private int SpriteWidth = 0;
    private int SpriteHeight = 0;
    private bool IsAnimated = true;

    public Animation2(string? path, Texture? texture, Vector2f position)
        : base(texture)
    {
        Position = position;
        IsAnimated = File.Exists(path);

        if (IsAnimated && Path.GetExtension(path) == ".xml" && !string.IsNullOrEmpty(path))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList nodes = doc.GetElementsByTagName("sprites");
            if (nodes == null || nodes.Count == 0)
                return;

            XmlNode? spritesNode = nodes[0];

            if (spritesNode == null || spritesNode.Attributes == null)
                return;

            XmlAttribute? xAttribute = spritesNode.Attributes["width"];
            if (xAttribute != null)
                SpriteWidth = int.Parse(xAttribute.Value);

            XmlAttribute? yAttribute = spritesNode.Attributes["height"];
            if (yAttribute != null)
                SpriteHeight = int.Parse(yAttribute.Value);

            List<Frame> Frames = new List<Frame>();

            foreach (XmlNode node in spritesNode.ChildNodes)
            {
                Frame frame = new Frame(SpriteWidth, SpriteHeight, node);
                Frames.Add(frame);
            }

            Frames.Sort();
            foreach (var item in Frames)
            {
                if (item.Name == null)
                    continue;

                string name = GetAnimationName(item.Name);

                if (item.Name.Contains(name))
                {
                    if (!animations.ContainsKey(name))
                    {
                        animations.Add(name, new List<Frame>());
                    }
                    animations[name].Add(item);
                }
            }
        }
        else
        {
            if(texture != null)
            {
                SpriteWidth = (int)texture.Size.X;
                SpriteHeight = (int)texture.Size.Y;
            }
        }

        Origin = GetOrigin();
    }
    
    private string LastAnimation = "";
    public int Count { get; private set; }
    public int Speed { get; private set; } = 3;
    private Vector2f OldPosition = new Vector2f();

    #region Virtual
    public virtual string? CurrentAnimation { get; private set; }

    public virtual void Afficher(RenderWindow window)
    {
        if (IsAnimated)
        {
            if (ChangeFrame())
            {
                Count++;
                OldPosition = Position;
            }

            string animation = CurrentAnimation == null || !animations.ContainsKey(CurrentAnimation) ? LastAnimation : CurrentAnimation;

            if (!string.IsNullOrEmpty(animation) && animations.ContainsKey(animation))
            {
                LastAnimation = animation;
                Count = Count >= animations[animation].Count * Speed ? 0 : Count;

                TextureRect = animations[animation][Count / Speed].Rect;
            }
            else
            {
                TextureRect = animations.First().Value.First().Rect;
            }
        }

        window.Draw(this);
    }
    public virtual Vector2f GetOrigin() => new Vector2f(SpriteWidth / 2, SpriteHeight);
    public virtual bool ChangeFrame() => !OldPosition.Equals(Position);
    #endregion
    private string GetAnimationName(string frameName)
    {
        string name = frameName;
        int count = 1;

        while (uint.TryParse(name.Substring(name.Length - count), out _))
        {
            count++;
        }
        name = name.Substring(0, name.Length - count + 1).ToUpper();

        return name;
    }
}