using SFML.Graphics;
using SFML.System;
using static KB4_PFI_Zelda.Animation;

namespace KB4_PFI_Zelda_solution;

static class Utilitaire
{
    #region Constantes
    public const string BaseTitle = "PFI Zelda";
    #endregion

    public static bool IsInDebug = false;
    public static bool IsInCutscene = false;

    private static Random rng = new Random();
    public static int GenerateNombre(int min, int max) => rng.Next(min, max);

    public static Direction GetRandomDirection(Direction[]? forbiddenDirections = null)
    {
        List<Direction> allDirections = new List<Direction>();
        foreach (Direction item in Enum.GetValues(typeof(Direction)))
        {
            if (item == Direction.NEUTRAL)
                continue;

            allDirections.Add(item);
        }

        if (forbiddenDirections != null)
        {
            for (int i = allDirections.Count - 1; i >= 0; i--)
            {
                int index = Array.IndexOf(forbiddenDirections, allDirections[i]);
                if (index != -1)
                {
                    allDirections.RemoveAt(i);
                }
            }
        }
        return allDirections.Count == 0 ? Direction.Bas : allDirections[GenerateNombre(0, allDirections.Count)];
    }
    public static T? CreateObject<T>(Type type, object?[]? args = null)
    {
        object? obj = Activator.CreateInstance(type, args);
        return obj == null ? default(T) : (T)obj;
    }

    public static Type GetRandomType<T>(Dictionary<Type, T?> dic)
        => dic.Keys.ToArray()[GenerateNombre(0, dic.Keys.Count)];

    public static int SortByY(Transformable obj1, Transformable? obj2)
        => obj2 == null || obj2.Position.Y > obj1.Position.Y ? -1 : 1;

    #region Vector Related
    public static double Distance(Vector2f a, Vector2f b) => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    public static Vector2f AddVector2f(Vector2f a, Vector2f b) => new Vector2f(a.X + b.X, a.Y + b.Y);
    public static Vector2i DirectionToVector2i(Direction dir)
    {
        Vector2i result = new Vector2i(0, 0);
        switch (dir)
        {

            case Direction.Gauche:
            case Direction.Droite:
                result.X = dir == Direction.Droite ? 1 : -1;
                break;
            case Direction.Bas:
            case Direction.Haut:
                result.Y = dir == Direction.Bas ? 1 : -1;
                break;
            default:
                result.Y = 1;
                break;
        }
        return result;
    }
    public static bool IsBetweenPoints(Vector2f pointA, Vector2f pointB, Vector2f pos)
    {
        return pos.X >= pointA.X && pos.X <= pointB.X && pos.Y >= pointA.Y && pos.Y <= pointB.Y;
    }

    public static string Vector2fToString(Vector2f a) => $"(X) {a.X} (Y) {a.Y}";
    #endregion

    public static View GetViewWithBounds(FloatRect[] bounds, RenderWindow window, Personnage center)
    {
        View Vue = window.GetView();
        Vector2f charVelocity = Vue.Center - center.Position;
        Vector2f topLeftCorner = Vue.Center - Vue.Size / 2;

        FloatRect rectX = new FloatRect(topLeftCorner - new Vector2f(charVelocity.X, 0), Vue.Size);
        FloatRect rectY = new FloatRect(topLeftCorner - new Vector2f(0, charVelocity.Y), Vue.Size);

        bool blockX = false;
        bool blockY = false;

        foreach (var item in bounds)
        {
            if (!blockX && item.Intersects(rectX))
                blockX = true;

            if (!blockY && item.Intersects(rectY))
                blockY = true;
        }

        Vue.Center = new Vector2f(
            !blockX ? center.Position.X : Vue.Center.X,
            !blockY ? center.Position.Y : Vue.Center.Y
            );
        return Vue;
    }
}
