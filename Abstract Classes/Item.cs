using KB4_PFI_Zelda;
using KB4_PFI_Zelda_solution.Interfaces;
using SFML.Graphics;
using SFML.System;
using System.Reflection;

namespace KB4_PFI_Zelda_solution.Items;

/// <summary>
/// Classe abstraite déterminant toutes les propriétés d'un item
/// </summary>
abstract class Item : Animation2, IApprochable, IDrawableDebug
{
    #region Fields
    /// <summary>
    /// Nom de l'item
    /// </summary>
    public string NAME;

    /// <summary>
    /// Effets qu'attribut l'item
    /// </summary>
    public ItemBoost Boosts;

    /// <summary>
    /// Détermine si l'objet est encore utilisable
    /// </summary>
    private bool isUsable = true;

    /// <summary>
    /// Détermine si l'objet est encore utilisable
    /// </summary>
    public bool IsUsable
    {
        get => isUsable;
        private set
        {
            isUsable = value;
        }
    }

    /// <summary>
    /// Position de l'item
    /// </summary>
    public Vector2f ColliderPos => Position - new Vector2f(0, Texture.Size.Y / 2);

    #endregion

    /// <summary>
    /// Effets qu'attribut l'item
    /// </summary>
    public struct ItemBoost
    {
        public int HEALTHGAIN;
        public int ATTACKGAIN;
        public int DEFENCEGAIN;
        public int AGILITYGAIN;

        public override string ToString()
        {
            string result = "";

            FieldInfo[] fields = typeof(ItemBoost).GetFields();

            foreach (var item in fields)
            {
                object? val = item.GetValue(this);

                if (val == null || (int)val == 0)
                    continue;

                result += $"{item.Name}: {val};";
            }
            return result;
        }
    }

    public Item(string NAME, ItemBoost itemBoost, Vector2f POSITION, Texture? texture)
        : base(null, texture, POSITION)
        //: base(texture, POSITION, 1)
    {
        this.NAME = NAME;
        Boosts = itemBoost;
    }


    /// <summary>
    /// Méthode qui gère l'utilisation de l'item
    /// </summary>
    public void TakeEvent(object? sender, EventArgs e)
    {
        if (sender == null || !isUsable)
            return;

        Heros s = (Heros)sender;

        if ((this as IApprochable).EstProche(s).estProche)
        {
            isUsable = false;
            Use(s);
        }
    }

    #region Override
    public override string ToString() => $"Item {NAME}: {Boosts};{Position}";

    /// <summary>
    /// Afficher l'item dans la fenêtre donnée
    /// </summary>
    /// <param name="window">Fenêtrer à afficher l'objet à l'intérieur</param>
    public override void Afficher(RenderWindow window)
    {
        if (!isUsable)
            return;

        window.Draw(this);

        if (Utilitaire.IsInDebug)
        {
            (this as IDrawableDebug).Draw(window, Color.Cyan, this.DistanceMax / 2);
        }
    }
    #endregion

    #region Virtual
    /// <summary>
    /// Distance à laquelle l'objet peut être collecté.
    /// </summary>
    public virtual float DistanceMax => 10;

    /// <summary>
    /// Fonction qui s'exécute quand un personnage récolte un objet
    /// </summary>
    public virtual void Use(Personnage source) => source.UseBoost(Boosts);
    #endregion
}
