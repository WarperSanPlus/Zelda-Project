using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Items;

class Epee : Item
{
    public Epee(string NAME, Vector2f POSITION, Texture? texture)
        : this(NAME, new ItemBoost() { ATTACKGAIN = 5, }, POSITION, texture) { }

    public Epee(string NAME, ItemBoost itemBoost, Vector2f POSITION, Texture? texture)
        : base(NAME, itemBoost, POSITION, texture) { }
}
