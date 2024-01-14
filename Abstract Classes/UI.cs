using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution;

abstract class UI : IComparable<UI>
{
    #region Constantes
    protected static readonly Font Font = new Font("Fonts\\The Wild Breath of Zelda.otf"); // https://chequered.ink/
    protected static readonly uint BaseSize = 16;
    protected static readonly Color BaseColor = Color.White;
    protected static readonly Color BaseOutlineColor = Color.Black;
    protected static readonly float BaseOutlineThickness = 1;

    protected static readonly Text BaseText = new Text()
    {
        Font = UI.Font,
        CharacterSize = BaseSize,
        FillColor = BaseColor,
        OutlineColor = BaseOutlineColor,
        OutlineThickness = BaseOutlineThickness,
    };
    #endregion

    #region Abstract
    /// <summary>
    /// Positions des composantes
    /// </summary>
    public abstract Dictionary<object, Vector2f>? UiPos { get; }

    /// <summary>
    /// Ordre d'affichage (2 affichera au-dessus de 1)
    /// </summary>
    public abstract int Layer { get; }
    #endregion

    #region Virtual
    /// <summary>
    /// Détermine si une interface doit être affichée ou non
    /// </summary>
    public virtual bool ShowCondition(RenderWindow window, Personnage player) => true;

    /// <summary>
    /// Fonction permettant d'actualiser les composantes
    /// </summary>
    public virtual void UpdateComponents(RenderWindow window, Personnage player) { }
    #endregion

    protected static Vector2f GetTLCorner(RenderWindow window)
    {
        View view = window.GetView();
        return view.Center - view.Size / 2;
    }

    public void Draw(RenderWindow window, Personnage player)
    {
        if (UiPos == null || !ShowCondition(window, player))
            return;

        Vector2f tlCorner = GetTLCorner(window);

        foreach (Transformable? item in UiPos.Keys)
        {
            if (item == null)
                continue;

            item.Position = tlCorner + UiPos[item];
        }

        UpdateComponents(window, player);

        foreach (Drawable item in UiPos.Keys)
        {
            window.Draw(item);
        }
    }

    public int CompareTo(UI? other) => other != null && other.Layer > Layer ? -1 : 1;
}
