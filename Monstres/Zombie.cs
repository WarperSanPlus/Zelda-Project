using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Monstres;

class Zombie : Monstre
{
    public Zombie(string name, Texture? texture, Vector2f position) 
        : this(name, texture, position, path: "Monstres XML\\Zombie.xml", health: 80, attack: 12, defence: 5, agility: 2) { }

    public Zombie(string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility)
        : base(name, texture, position, path, health, attack, defence, agility) { }

    #region Override
    public override int SubirAttaque(Personnage source)
    {
        this.DEFENCE += Utilitaire.GenerateNombre(1, Math.Clamp(source.ATTACK, 1, int.MaxValue));
        return source.ATTACK;
    }
    #endregion
}
