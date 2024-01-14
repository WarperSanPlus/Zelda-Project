using System.Reflection;

namespace KB4_PFI_Zelda_solution.Interfaces;

/// <summary>
/// Interface permettant d'afficher toutes les propriétés d'un objet
/// </summary>
interface IPrintable
{
    /// <summary>
    /// Retourne un string contenant toutes les propriété de l'objet donné
    /// </summary>
    /// <param name="obj">Objet à afficher</param>
    public static string ToString<T>(T obj)
    {
        string? result = default;

        if (obj != null)
        {
            // https://stackoverflow.com/a/7596241
            foreach (var prop in typeof(Personnage).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                bool hasProperty = obj.GetType().GetProperty(prop.Name) != null;
                if (!prop.CanWrite || !hasProperty)
                    continue;

                result += $"\n{prop.Name}: {prop.GetValue(obj)}";
            }
        }
        return result == null ? "" : result;
    }
}
