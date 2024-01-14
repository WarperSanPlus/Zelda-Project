using KB4_PFI_Zelda;
using KB4_PFI_Zelda_solution.Interfaces;
using KB4_PFI_Zelda_solution.Managers;
using KB4_PFI_Zelda_solution.Monstres;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KB4_PFI_Zelda_solution;

internal class Heros : Personnage, IDrawableDebug
{
    public Heros(Window win, string name, Texture? texture, Vector2f position)
        : this(win, name, texture, position, path: "Link.xml", health: 1000, attack: 200, defence: 10, agility: 4)
    { }

    public Heros(Window win, string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility)
        : base(name, texture, position, path, health, attack, defence, agility)
    {
        win.KeyPressed += ProcessPressedKeys;
        win.KeyReleased += ProcessReleasedKeys;
        moveSpeedDebug = SPEED;
    }

    public event EventHandler? IApprochableCheck;
    public event EventHandler? IAttaquableCheck;
    public event EventHandler? LoadingZoneCheck;

    #region Fields
    public bool LeaveGame { get; private set; }

    public int moveSpeedDebug = default;
    #endregion

    #region Controls Section
    List<Keyboard.Key> keysHeld = new List<Keyboard.Key>();
    private void ProcessPressedKeys(object? sender, KeyEventArgs e)
    {
        if (sender == null || keysHeld.Contains(e.Code) || Utilitaire.IsInCutscene)
            return;

        keysHeld.Add(e.Code);

        // MOVEMENT ACTIONS
        if (PlayerControls.IsMovementInput(e.Code))
        {
            DIRECTION = PlayerControls.GetDirectionFromInput(e.Code);
            return;
        }

        // DEBUG ACTIONS
        if (PlayerControls.IsDebugInput(e.Code))
        {
            PlayerControls.ControlsEnum debugAction = PlayerControls.GetDebugCategory(e.Code);

            if (!Utilitaire.IsInDebug && debugAction != PlayerControls.ControlsEnum.TOGGLE_DEBUG)
                return;

            switch (debugAction)
            {
                case PlayerControls.ControlsEnum.TOGGLE_DEBUG:
                    Utilitaire.IsInDebug = !Utilitaire.IsInDebug;
                    ((Window)sender).SetTitle(Utilitaire.BaseTitle + (Utilitaire.IsInDebug ? $" - (Debug)" : ""));
                    break;
                case PlayerControls.ControlsEnum.SPEED_UP_DEBUG:
                    moveSpeedDebug += 1;
                    break;
                case PlayerControls.ControlsEnum.SPEED_DOWN_DEBUG:
                    moveSpeedDebug = Math.Clamp(moveSpeedDebug - 1, 0, moveSpeedDebug);
                    break;
                case PlayerControls.ControlsEnum.RESET_SPEED_DEBUG:
                    moveSpeedDebug = SPEED;
                    break;
                default:
                    break;
            }
            return;
        }

        switch (e.Code)
        {
            case Keyboard.Key.Escape:
                LeaveGame = true;
                break;
            default:
                break;
        }
    }
    private void ProcessReleasedKeys(object? sender, KeyEventArgs e)
    {
        keysHeld.Remove(e.Code);

        if (Utilitaire.IsInCutscene)
            return;

        if (PlayerControls.IsMovementInput(e.Code))
        {
            (bool result, Keyboard.Key key) = PlayerControls.IsMovementInputHeld(keysHeld);
            DIRECTION = result ? PlayerControls.GetDirectionFromInput(key) : Animation.Direction.NEUTRAL;
            return;
        }
    }
    #endregion

    #region Override
    public override Color DebugColor => Color.Red;
    public override Vector2f ColliderOffset => new Vector2f(0, -8);

    public override int SubirAttaque(Personnage source)
    {
        return Utilitaire.GenerateNombre(1, Math.Clamp(source.ATTACK, 1, int.MaxValue));
    }

    public override void EffectIfAttacking(Personnage target, int dmg)
    {
        if (target is Squelette)
        {
            DEFENCE = Math.Clamp(DEFENCE - target.CalculateDamage(this) / 2, 0, DEFENCE);
        }
    }

    public override bool Move(Carte? map)
    {
        if (IsMoving && !IsLocked)
        {
            Vector2i velocity = Utilitaire.DirectionToVector2i(DIRECTION) * (Utilitaire.IsInDebug ? moveSpeedDebug : SPEED);
            if (velocity.X != 0 || velocity.Y != 0)
            {
                Vector2f tempPos = Utilitaire.AddVector2f(Position, (Vector2f)velocity);
                (bool result, bool forcedBlock) =
                    map != null && !Utilitaire.IsInCutscene
                    ? map.EstPositionValide(tempPos)
                    : (true, false);

                if (!forcedBlock && (result || Utilitaire.IsInDebug))
                {
                    Position = tempPos;
                }
            }
        }

        if (IApprochableCheck != null)
        {
            IApprochableCheck.Invoke(this, EventArgs.Empty);
        }

        if (IAttaquableCheck != null)
        {
            IAttaquableCheck.Invoke(this, EventArgs.Empty);
        }

        if (LoadingZoneCheck != null)
        {
            LoadingZoneCheck.Invoke(this, EventArgs.Empty);
        }
        return LeaveGame;
    }
    #endregion
}
