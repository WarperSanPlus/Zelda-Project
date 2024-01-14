////////////////////////////////////////////////////////////

// Déclaration de la classe Carte qui représente le monde
// dans lequel évoluent les personnages et les objets
// 
// Joan-Sébastien Morales 
// Création: 28 avril 2010
// Version 1.0
// Version 1.1 - Bloquer la copie
// Version 2.0 - 9 mai 2014 Adaptation à SFML - Carte dérive de Sprite
// Version 3.0 - 29 avril 2020 - Traduction en C#
////////////////////////////////////////////////////////////

//--------------------------------------------------------//
// Carte
//--------------------------------------------------------//
using KB4_PFI_Zelda_solution;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda;

class Carte : Sprite
{
    private bool[,] obstruction; // Carte d'obstruction
    private List<Vector2u> availableTiles = new();
    ////////////////////////////////////////////////////////////
    // Carte
    // Constructeur paramétrique
    //
    // Intrants: 
    // Texture: Texture contenant l'image du monde 
    //                  
    // Obstruction: L'image d'obstruction est une version de
    //              l'image du monde où les pixels
    //              où les personnages peuvent passer
    //              sont de couleurs noires(RGB 0,0,0)
    ////////////////////////////////////////////////////////////
    public Carte(Texture laTexture, Image obs) 
        : base(laTexture)
    {
        obstruction = new bool[obs.Size.Y, obs.Size.X];

        for (uint y = 0; y < obstruction.GetLength(0); y++)
        {
            for (uint x = 0; x < obstruction.GetLength(1); x++)
            {
                bool isBlocked = obs.GetPixel(x, y) != Color.Black;

                if (!isBlocked)
                    availableTiles.Add(new Vector2u(x, y));

                obstruction[y, x] = isBlocked;
            }
        }
    }

    public Carte(string pathTexture, string pathObs)
        : this(new Texture(pathTexture), new Image(pathObs))
    { }

    public uint Hauteur { get => (uint)obstruction.GetLength(0); }
    public uint Largeur { get => (uint)obstruction.GetLength(1); }

    ////////////////////////////////////////////////////////////
    // Afficher
    // Affiche la carte dans une fenêtre
    //
    // Intrant: Fenetre: Fenêtre dans laquelle la carte doit
    //                    être affichée
    ////////////////////////////////////////////////////////////
    public void Afficher(RenderWindow fenetre)
    {
        fenetre.Draw(this);
    }

    public Vector2u GetRandomPos() => availableTiles[Utilitaire.GenerateNombre(0, availableTiles.Count)];

    ////////////////////////////////////////////////////////////
    // EstPositionValide
    // Permet d
    // Extrant: Vrai si le personnage peut se trouver à cette
    //          position sur la carte, faux dans le cas 
    //          contraire
    //          Vrai si la validation est optionnelle.
    ////////////////////////////////////////////////////////////
    public (bool, bool) EstPositionValide(Vector2f position)
    {
        //if (position.X < 0 || position.X >= obstruction.Size.X || position.Y < 0 || position.Y >= obstruction.Size.Y)
        if (position.X < 0 || position.X >= Largeur || position.Y < 0 || position.Y >= Hauteur)
        {
            return (false, true);
            //throw new Exception("Carte.EstPositionValide: position en dehors de la carte!!");
        }

        //return obstruction.GetPixel((uint)position.X, (uint)position.Y) == Color.Black;
        return (!obstruction[(int)position.Y, (int)position.X], false);
    }
}
