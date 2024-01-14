using KB4_PFI_Zelda_solution.Managers;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.UIs;

internal class ClassicUI : UI
{
    #region UI Components
    private Text StatsLabel = new Text(BaseText)
    {
        DisplayedString = "STATS",
    };

    private Sprite LifeIcon = new Sprite()
    {
        Texture = new Texture("Images/Icons-Pack.png"),
        TextureRect = new IntRect(32 * 15, 32 * 5, 32, 32),
        Scale = new Vector2f(0.75f, 0.75f),
    };
    private Text HealthLabel = new Text(BaseText)
    {
        FillColor = Color.Red,
    };

    private Text AttackLabel = new Text(BaseText)
    {
        FillColor = Color.Red,
    };
    private Sprite SwordIcon = new Sprite()
    {
        Texture = new Texture("Images/Icons-Pack.png"),
        TextureRect = new IntRect(32 * 6, 32 * 5, 32, 32),
        Scale = new Vector2f(0.75f, 0.75f),
    };

    private Text DefenceLabel = new Text(BaseText)
    {
        FillColor = Color.Red,
    };
    private Sprite ShieldIcon = new Sprite()
    {
        Texture = new Texture("Images/Icons-Pack.png"),
        TextureRect = new IntRect(32 * 2, 32 * 5, 32, 32),
        Scale = new Vector2f(0.75f, 0.75f),
    };
    #endregion

    public override bool ShowCondition(RenderWindow window, Personnage player) => player.IsAlive;

    public override Dictionary<object, Vector2f>? UiPos => new Dictionary<object, Vector2f>()
    {
        [StatsLabel] = new Vector2f(0, GameManager.UIBottom - BaseSize * 4 - 5),

        [HealthLabel] = new Vector2f(22, GameManager.UIBottom - BaseSize * 3 - 5),
        [LifeIcon] = new Vector2f(0, GameManager.UIBottom - BaseSize * 3 - 7),

        [AttackLabel] = new Vector2f(22, GameManager.UIBottom - BaseSize * 2 - 5),
        [SwordIcon] = new Vector2f(0, GameManager.UIBottom - BaseSize * 2 - 7),

        [DefenceLabel] = new Vector2f(22, GameManager.UIBottom - BaseSize * 1 - 5),
        [ShieldIcon] = new Vector2f(0, GameManager.UIBottom - BaseSize * 1 - 7)
    };

    public override int Layer => 1;

    public override void UpdateComponents(RenderWindow window, Personnage player)
    {
        HealthLabel.DisplayedString = player.HEALTH.ToString();
        AttackLabel.DisplayedString = player.ATTACK.ToString();
        DefenceLabel.DisplayedString = player.DEFENCE.ToString();
    }
}
