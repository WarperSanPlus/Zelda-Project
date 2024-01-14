using KB4_PFI_Zelda_solution.Managers;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.UIs;

/// <summary>
/// Classe statique qui gère le menu de Debug
/// </summary>
internal class DebugUI : UI
{
    #region UI Components
    private Text PlayerPosLabel = new Text(BaseText);
    private Text SpeedLabel = new Text(BaseText);
    public Text FPSLabel = new Text(BaseText);
    public Text ViewPosLabel = new Text(BaseText);
    private Text LockedLabel = new Text(BaseText);
    #endregion

    public override bool ShowCondition(RenderWindow window, Personnage player) => Utilitaire.IsInDebug;

    public override Dictionary<object, Vector2f>? UiPos => new Dictionary<object, Vector2f>()
    {
        [PlayerPosLabel] = new Vector2f(0, 0),
        [ViewPosLabel] = new Vector2f(0, BaseSize * 1),
        [SpeedLabel] = new Vector2f(0, BaseSize * 2),
        [FPSLabel] = new Vector2f(GameManager.UIRight, 0),
        [LockedLabel] = new Vector2f(0, BaseSize * 3),
    };

    public override int Layer => 2147483647;

    public override void UpdateComponents(RenderWindow window, Personnage player)
    {
        PlayerPosLabel.DisplayedString = "PLAYER: " + Utilitaire.Vector2fToString(player.Position);
        ViewPosLabel.DisplayedString = "VIEW: " + Utilitaire.Vector2fToString(window.GetView().Center);
        SpeedLabel.DisplayedString = $"Speed: {((Heros)player).moveSpeedDebug}";
        LockedLabel.DisplayedString = player.IsLocked.ToString();
        LockedLabel.FillColor = player.IsLocked ? Color.Red : Color.Green;

        FPSLabel.Position -= new Vector2f(FPSLabel.GetLocalBounds().Width, 0);
    }
}