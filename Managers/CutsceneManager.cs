using KB4_PFI_Zelda;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KB4_PFI_Zelda_solution.Managers;

static class CutsceneManager
{
    /* List instructions:
     * - Set Direction (DONE)
     * - Move X tiles (DONE)
     * - Move for X time (DONE)
     * - Get Flag (DONE)
     * - Set Flag (DONE)
     * - Play Animation
     * - Play Sound
     * - Wait X time (DONE)
     * - Wait for movement (DONE)
     */

    public static async Task<bool> PlayCutsceneAsync(Func<Task>[] commands)
    {
        bool notSpecified = !Utilitaire.IsInCutscene;

        if (notSpecified)
            Utilitaire.IsInCutscene = true;
        foreach (var item in commands)
        {
            try
            {
                await item();
            }
            finally { }
        }

        if (notSpecified)
            Utilitaire.IsInCutscene = false;
        return true;
    }

    private static async Task<Task> RunAction(Action ac)
    {
        await Task.Run(ac);
        return Task.CompletedTask;
    }

    public static async Task<Task> WaitTime(int timeMS)
    {
        await Task.Delay(timeMS);
        return Task.CompletedTask;
    }

    #region Direction
    public static async Task<Task> SetDirection(Personnage chara, Animation.Direction dir)
        => await RunAction(() => { chara.DIRECTION = dir; });
    #endregion

    #region Movement
    private static async Task<Task> WaitForMovement(Vector2f desiredPosition, Personnage chara)
    {
        while (chara.Position.X < desiredPosition.X || chara.Position.Y < desiredPosition.Y)
        {
            await Task.Delay(1000 / GameManager.FPS);
        }
        chara.Position = desiredPosition;
        return Task.CompletedTask;
    }

    public static async Task<Task> MoveTiles(int count, Personnage chara, Animation.Direction dir)
        => await Move(count, chara, dir, false);

    public static async Task<Task> MoveTime(int timeMS, Personnage chara, Animation.Direction dir)
        => await Move(timeMS, chara, dir, true);

    public static async Task<Task> MoveFrames(int count, Personnage chara, Animation.Direction dir)
        => await MoveTime(GameManager.SPF * count, chara, dir);

    private static async Task<Task> Move(int amount, Personnage chara, Animation.Direction dir, bool isAmountTime)
    {
        if (dir != Animation.Direction.NEUTRAL)
        {
            await SetDirection(chara, dir);

            chara.Move(null);

            if (isAmountTime)
                await Task.Delay(amount);
            else
                await WaitForMovement(new Vector2f(chara.Position.X, chara.Position.Y + GameManager.TileSize * (amount - 0.5f)), chara);

            await SetDirection(chara, Animation.Direction.NEUTRAL);
        }
        return Task.FromResult(Task.CompletedTask);
    }
    #endregion

    #region Lock/Unlock
    /// <summary>
    /// Bloque les mouveents du personnage donné
    /// </summary>
    public static async Task<Task> LockCharacter(Personnage chara)
        => await SetLockCharacter(chara, true);

    /// <summary>
    /// Débloque les mouvements du personnage donné
    /// </summary>
    public static async Task<Task> UnlockCharacter(Personnage chara)
        => await SetLockCharacter(chara, false);

    private static async Task<Task> SetLockCharacter(Personnage chara, bool locked)
        => await RunAction(() => { chara.IsLocked = locked; });
    #endregion

    #region Flags
    /// <summary>
    /// Obtient la valeur du flag donné
    /// </summary>
    public static bool GetFlag(byte id)
    {
        bool value;
        if (GameManager.Flags.TryGetValue(id, out value))
            return value;
        throw new Exception("Flag doesn't exit");
    }
    
    /// <summary>
    /// Attribue la valeur donnée au flag donné
    /// </summary>
    public static Task SetFlag(byte id, bool value)
    {
        if (!GameManager.Flags.TryGetValue(id, out _))
            throw new Exception("Flag doesn't exit");
        GameManager.Flags[id] = value;
        return Task.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Place le centre de la vue à la position donnée
    /// </summary>
    public static Task SetViewToPos(RenderWindow window, Vector2f pos)
    {
        window.GetView().Center = pos;

        return Task.CompletedTask;
    }
}
