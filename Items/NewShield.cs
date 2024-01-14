using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Items;

class NewShield : Item
{
    public NewShield(string NAME, Vector2f POSITION, Texture? texture)
        : this(NAME, new ItemBoost() { DEFENCEGAIN = 3, AGILITYGAIN = -1, }, POSITION, texture) { }

    public NewShield(string NAME, ItemBoost itemBoost, Vector2f POSITION, Texture? texture)
        : base(NAME, itemBoost, POSITION, texture) { }

    public override float DistanceMax => 25;
}
