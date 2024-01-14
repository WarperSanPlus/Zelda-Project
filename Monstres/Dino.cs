using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Monstres;

class Dino : Monstre
{
    public Dino(string name, Texture? texture, Vector2f position) 
        : this(name, texture, position, "Monstres XML/Dino.xml", health: 150, attack: 10, defence: 15, agility: 5) { }

    public Dino(string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility) 
        : base(name, texture, position, path, health, attack, defence, agility) { }

    #region Override
    public override int SubirAttaque(Personnage source)
    {
        int damage = Utilitaire.GenerateNombre(1, Math.Clamp(source.ATTACK, 1, int.MaxValue));
        DEFENCE = Math.Clamp(DEFENCE - damage, 0, DEFENCE);

        return damage;
    }
    #endregion
}
