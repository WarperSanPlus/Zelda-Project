using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Monstres;

class Squelette : Monstre
{
    public Squelette(string name, Texture? texture, Vector2f position) 
        : this(name, texture, position, "Monstres XML/Squelette.xml", health: 20, attack: 8, defence: 15, agility: 4) { }

    public Squelette(string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility) 
        : base(name, texture, position, path, health, attack, defence, agility) { }


    #region Override
    public override int SubirAttaque(Personnage source)
    {
        return HEALTH;
    }
    #endregion
}
