using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Items;

class Pain : Item
{
    public Pain(string NAME, Vector2f POSITION, Texture? texture)
        : this(NAME, new ItemBoost() { HEALTHGAIN = 50, }, POSITION, texture) { }

    public Pain(string NAME, ItemBoost itemBoost, Vector2f POSITION, Texture? texture)
        : base(NAME, itemBoost, POSITION, texture) { }
}
