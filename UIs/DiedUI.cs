using KB4_PFI_Zelda_solution.Managers;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.UIs;

internal class DiedUI : UI
{
    #region UI Components
    private Text DiedLabel = new Text(BaseText)
    {
        FillColor = Color.Red,
        DisplayedString = "YOU DIED",
    };
    #endregion

    public override Dictionary<object, Vector2f>? UiPos => new Dictionary<object, Vector2f>()
    {
        [DiedLabel] = new Vector2f(GameManager.UIRight - DiedLabel.GetLocalBounds().Width, GameManager.UIBottom - DiedLabel.GetLocalBounds().Height) / 2,
    };

    public override int Layer => 2147483646;
    private bool WaitForFadingIn = false;
    
    public override void UpdateComponents(RenderWindow window, Personnage player)
    {
        if (WaitForFadingIn && !GameManager.Flags[GameManager.IsFadingIn])
        {
            GameManager.Flags[GameManager.RequestLeave] = true;
            WaitForFadingIn = false;
        }
        else if (!WaitForFadingIn && !GameManager.Flags[GameManager.IsFadingIn])
        {
            GameManager.Flags[GameManager.IsFadingIn] = true;
            WaitForFadingIn = true;
        }
    }

    public override bool ShowCondition(RenderWindow window, Personnage player) => !player.IsAlive;
}
