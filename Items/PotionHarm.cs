using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Items;

class PotionHarm : Item
{
    public PotionHarm(string NAME, Vector2f POSITION, Texture? texture)
        : this(NAME, new ItemBoost() { HEALTHGAIN = -5, }, POSITION, texture) { }

    public PotionHarm(string NAME, ItemBoost itemBoost, Vector2f POSITION, Texture? texture)
        : base(NAME, itemBoost, POSITION, texture) { }

    public override float DistanceMax => 20;
}
