using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Monstres;

class Bee : Monstre
{
    public Bee(string name, Texture? texture, Vector2f position) 
        : this(name, texture, position, "Monstres XML/Bee.xml", health: 10, attack: 5, defence: 2, agility: 4) { }

    public Bee(string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility) 
        : base(name, texture, position, path, health, attack, defence, agility) { }

    public override Vector2f ColliderOffset => new Vector2f(0, -36);
    public override bool IsStupid => false;

}
