using KB4_PFI_Zelda;
using KB4_PFI_Zelda_solution.Managers;
using SFML.System;

namespace KB4_PFI_Zelda_solution;

/// <summary>
/// Il serait possible de stocker ces cutscenes dans des fichiers
/// </summary>
static class CutscenesHolder
{
    public static readonly Func<Task>[] MainLW1 = new Func<Task>[]
    {
        async () => await CutsceneManager.LockCharacter(GameManager.Link),
        async () => await CutsceneManager.MoveFrames(1, GameManager.Link, Animation.Direction.Haut),
        async () => await CutsceneManager.SetDirection(GameManager.Link, Animation.Direction.NEUTRAL),
        async () => await CutsceneManager.SetViewToPos(GameManager.fenetre, new Vector2f(161, 864)),
        async () => await CutsceneManager.WaitTime(GameManager.SPF),
    };

    public static readonly Func<Task>[] LWMain1 = new Func<Task>[]
    {
        async () => await CutsceneManager.LockCharacter(GameManager.Link),
        async () => await CutsceneManager.MoveFrames(1, GameManager.Link, Animation.Direction.Bas),
        async () => await CutsceneManager.SetDirection(GameManager.Link, Animation.Direction.NEUTRAL),
        async () => await CutsceneManager.SetViewToPos(GameManager.fenetre, new Vector2f(161, 1186)),
        async () => await CutsceneManager.WaitTime(GameManager.SPF),
    };
}
