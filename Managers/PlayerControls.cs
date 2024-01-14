using KB4_PFI_Zelda;
using SFML.Window;

namespace KB4_PFI_Zelda_solution.Managers;

internal static class PlayerControls
{
    /// <summary>
    /// Actions possibles
    /// </summary>
    public enum ControlsEnum
    {
        INVALID,
        UP,
        DOWN,
        LEFT,
        RIGHT,
        TOGGLE_DEBUG,
        SPEED_UP_DEBUG,
        SPEED_DOWN_DEBUG,
        RESET_SPEED_DEBUG,
    }

    #region Constantes
    private static readonly Dictionary<ControlsEnum, Keyboard.Key[]> DefaultControls = new Dictionary<ControlsEnum, Keyboard.Key[]>()
    {
        [ControlsEnum.UP] = new Keyboard.Key[] { Keyboard.Key.W, Keyboard.Key.Up },
        [ControlsEnum.DOWN] = new Keyboard.Key[] { Keyboard.Key.S, Keyboard.Key.Down },
        [ControlsEnum.LEFT] = new Keyboard.Key[] { Keyboard.Key.A, Keyboard.Key.Left },
        [ControlsEnum.RIGHT] = new Keyboard.Key[] { Keyboard.Key.D, Keyboard.Key.Right },
        [ControlsEnum.TOGGLE_DEBUG] = new Keyboard.Key[] { Keyboard.Key.F10 },
        [ControlsEnum.SPEED_UP_DEBUG] = new Keyboard.Key[] { Keyboard.Key.Add },
        [ControlsEnum.SPEED_DOWN_DEBUG] = new Keyboard.Key[] { Keyboard.Key.Subtract },
        [ControlsEnum.RESET_SPEED_DEBUG] = new Keyboard.Key[] { Keyboard.Key.Divide },
    };
    const char ControlsEnumSeparator = ':';
    const char KeysSeparator = '/';
    #endregion

    /// <summary>
    /// Contrôles chargés
    /// </summary>
    public static Dictionary<ControlsEnum, Keyboard.Key[]> Controls = new Dictionary<ControlsEnum, Keyboard.Key[]>();

    /// <summary>
    /// Sauvegarder les contrôles chargés
    /// </summary>
    public static void SaveControls(string path)
    {
        string text = "";

        foreach (var item in Controls)
        {
            text += item.Key.ToString() + ControlsEnumSeparator;
            foreach (var key in item.Value)
            {
                text += (int)key + "/";
            }
            text = text.Substring(0, text.Length - 1);

            text += "\n";
        }
        File.WriteAllText(path, text);
    }

    /// <summary>
    /// Charger les contrôles stockés dans le fichier donné
    /// </summary>
    public static void LoadControls(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine(path + " doesn't exist. Default Controls will be used");
            Controls = DefaultControls;
            return;
        }

        List<ControlsEnum> defaultKeys = DefaultControls.Keys.ToList();
        Controls.Clear();
        string[] lines = File.ReadAllLines(path);

        // Load keys
        foreach (var line in lines)
        {
            if (!line.Contains(ControlsEnumSeparator))
                continue;

            string[] strings = line.Split(ControlsEnumSeparator);

            if (strings.Length != 2)
                continue;

            if (Enum.TryParse(typeof(ControlsEnum), strings[0], out object? option))
            {
                if (option == null)
                    continue;

                string[] keysString = strings[1].Split(KeysSeparator);

                List<Keyboard.Key> keys = new List<Keyboard.Key>();

                foreach (var key in keysString)
                {
                    if (Enum.TryParse(typeof(Keyboard.Key), key, out object? keyCode))
                    {
                        if (keyCode == null)
                            continue;

                        keys.Add((Keyboard.Key)keyCode);
                    }
                }

                if (keys.Count != 0)
                {
                    if (defaultKeys.Contains((ControlsEnum)option))
                        defaultKeys.Remove((ControlsEnum)option);

                    Controls.Add((ControlsEnum)option, keys.ToArray());
                }
            }
        }

        // Check for missing keys
        if (defaultKeys.Count == 0)
            return;

        foreach (var item in defaultKeys)
        {
            Console.WriteLine($"Control '{item}' wasn't found. Those keys will be assigned for it:");

            foreach (var key in DefaultControls[item])
            {
                Console.WriteLine($" - {key}");
            }

            Controls.Add(item, DefaultControls[item]);
        }

        SaveControls(path);
    }

    private static bool IsKeyInControls(ControlsEnum[] controls, Keyboard.Key key)
    {
        bool result = false;

        foreach (var item in controls)
        {
            if (!Controls.ContainsKey(item))
                continue;

            result |= Controls[item].Contains(key);

            if (result)
                break;
        }
        return result;
    }

    #region Debug Controls
    static readonly ControlsEnum[] debugKeys =
    {
        ControlsEnum.TOGGLE_DEBUG,
        ControlsEnum.SPEED_UP_DEBUG,
        ControlsEnum.SPEED_DOWN_DEBUG,
        ControlsEnum.RESET_SPEED_DEBUG,
    };

    /// <returns>Est-ce que la touche donnée est une touche de Debug</returns>
    public static bool IsDebugInput(Keyboard.Key e) => IsKeyInControls(debugKeys, e);

    /// <returns>Obtenir la catégorie de debug de la touche donnée</returns>
    public static ControlsEnum GetDebugCategory(Keyboard.Key e)
    {
        foreach (var item in debugKeys)
        {
            if (Controls[item].Contains(e))
                return item;
        }
        return ControlsEnum.INVALID;
    }
    #endregion

    #region Movement Controls
    static readonly ControlsEnum[] movementKeys =
    {
        ControlsEnum.UP,
        ControlsEnum.DOWN,
        ControlsEnum.LEFT,
        ControlsEnum.RIGHT,
    };

    /// <returns>Is the given key a movement key ?</returns>
    public static bool IsMovementInput(Keyboard.Key e) => IsKeyInControls(movementKeys, e);

    /// <returns>Direction corresponding to the given key</returns>
    public static Animation.Direction GetDirectionFromInput(Keyboard.Key e)
    {
        Animation.Direction direction = Animation.Direction.Bas;

        if (Controls[ControlsEnum.UP].Contains(e))
            direction = Animation.Direction.Haut;
        else if (Controls[ControlsEnum.RIGHT].Contains(e))
            direction = Animation.Direction.Droite;
        else if (Controls[ControlsEnum.LEFT].Contains(e))
            direction = Animation.Direction.Gauche;
        return direction;
    }

    /// <summary>
    /// Checks if a movement key is held
    /// </summary>
    /// <param name="keysHeld">List of keys held</param>
    /// <returns>(A movement key is held; Key held)</returns>
    public static (bool, Keyboard.Key) IsMovementInputHeld(List<Keyboard.Key> keysHeld)
    {
        foreach (var movement in movementKeys)
        {
            foreach (var control in Controls[movement])
            {
                if (keysHeld.Contains(control))
                    return (true, control);
            }
        }
        return (false, Keyboard.Key.Unknown);
    }
    #endregion
}
