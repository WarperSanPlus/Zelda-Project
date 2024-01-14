using KB4_PFI_Zelda_solution.Managers;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.UIs;

internal class SpeechUI : UI
{
    #region UI Components
    private Text DiedLabel = new Text(BaseText)
    {
        FillColor = Color.White,
    };
    #endregion

    public override Dictionary<object, Vector2f>? UiPos => new Dictionary<object, Vector2f>()
    {
        [DiedLabel] = new Vector2f(GameManager.UIRight - DiedLabel.GetLocalBounds().Width, GameManager.UIBottom - DiedLabel.GetLocalBounds().Height) / 2,
    };

    public override int Layer => 10;

    string Text = "This is such a beautiful text!\nEveryone should enjoy it now!!";

    public override void UpdateComponents(RenderWindow window, Personnage player)
    {
        if (DiedLabel.DisplayedString != Text)
            DiedLabel.DisplayedString = Text.Substring(0, DiedLabel.DisplayedString.Length + 1);
    }
}
