using KB4_PFI_Zelda_solution.Managers;
using KB4_PFI_Zelda_solution.UIs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KB4_PFI_Zelda_solution;

class LoadingZone
{
    IntRect Collider;
    bool hasBeenFound = false;
    Vector2f TpPos;
    Heros heros;

    Func<Task>[]? fadeInFuncs = null;
    Func<Task>[]? fadeOutFuncs = null;

    public LoadingZone(
        Vector2i Size,
        Vector2i position,
        Vector2f tpPos,
        FadingUI ui,
        Func<Task>[]? fadeInFuncs = null,
        Func<Task>[]? fadeOutFuncs = null)
    {
        this.heros = GameManager.Link;
        Collider = new IntRect(position, Size);
        TpPos = tpPos;
        this.fadeInFuncs = fadeInFuncs;
        this.fadeOutFuncs = fadeOutFuncs;

        this.heros.LoadingZoneCheck += Check;
        ui.EndFadeIn += EndFadeInEvent;
        ui.EndFadeOut += EndFadeOutEvent;
    }

    private void Check(object? sender, EventArgs e)
    {
        if (GameManager.Flags[GameManager.LoadingZoneFound])
            return;

        if (sender is not Heros h)
            return;

        IntRect rectLink = new IntRect((Vector2i)h.Position, new Vector2i(1, 1));

        if (this.Collider.Intersects(rectLink))
        {
            GameManager.Flags[GameManager.LoadingZoneFound] = true;
            hasBeenFound = true;

            h.IsLocked = true;
            Utilitaire.IsInCutscene = true;
            GameManager.Flags[GameManager.IsFadingIn] = true;
        }
    }

    private void EndFadeInEvent(object? sender, EventArgs e)
    {
        if (!hasBeenFound)
            return;

        heros.Position = TpPos;
        _ = CutsceneManager.LockCharacter(heros);

        if (fadeInFuncs != null)
            _ = CutsceneManager.PlayCutsceneAsync(fadeInFuncs);
    }

    private void EndFadeOutEvent(object? sender, EventArgs e)
    {
        if (!hasBeenFound)
            return;

        if (fadeOutFuncs != null)
            _ = CutsceneManager.PlayCutsceneAsync(fadeOutFuncs);

        _ = CutsceneManager.UnlockCharacter(heros);
        Utilitaire.IsInCutscene = false;
        hasBeenFound = false;
        GameManager.Flags[GameManager.LoadingZoneFound] = false;
    }
}