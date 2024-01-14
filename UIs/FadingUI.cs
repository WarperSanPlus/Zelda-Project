using KB4_PFI_Zelda_solution.Managers;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.UIs;

internal class FadingUI : UI
{
    #region UI Components
    private RectangleShape BlackBG = new RectangleShape()
    {
        FillColor = new Color(0, 0, 0, 0),
    };
    #endregion

    public override Dictionary<object, Vector2f>? UiPos => new Dictionary<object, Vector2f>()
    {
        [BlackBG] = new Vector2f(0, 0),
    };

    public override int Layer => 2147483645;
    public byte Speed = 10;

    public event EventHandler? EndFadeIn;
    public event EventHandler? EndFadeOut;

    public override bool ShowCondition(RenderWindow window, Personnage player)
        => GameManager.Flags[0x0000] || GameManager.Flags[0x0001];

    public override void UpdateComponents(RenderWindow window, Personnage player)
    {
        if (BlackBG.FillColor.A != (GameManager.Flags[GameManager.IsFadingIn] ? 0xFF : 0x00))
        {
            BlackBG.Size = (Vector2f)window.Size;

            if (GameManager.Flags[GameManager.IsFadingIn])
                BlackBG.FillColor += new Color(0, 0, 0, Speed);
            else
                BlackBG.FillColor -= new Color(0, 0, 0, Speed);
        }
        else
        {
            if (GameManager.Flags[GameManager.IsFadingIn] && EndFadeIn != null)
            {
                EndFadeIn.Invoke(this, EventArgs.Empty);
                GameManager.Flags[GameManager.IsFadingIn] = false;
                GameManager.Flags[GameManager.IsFadingOut] = true;
            }
            else if (GameManager.Flags[GameManager.IsFadingOut] && EndFadeOut != null)
            {
                EndFadeOut.Invoke(this, EventArgs.Empty);
                GameManager.Flags[GameManager.IsFadingOut] = false;
            }
        }
    }
}
