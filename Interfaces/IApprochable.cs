using SFML.System;

namespace KB4_PFI_Zelda_solution.Interfaces;

/// <summary>
/// Interface permettant d'être approché
/// </summary>
/// Changements:
/// - La propriété Pos s'appele ColliderPos
/// - La méthode EstProche retourne aussi la distance
interface IApprochable
{
    /// <summary>
    /// Distance maximale à laquelle deux objets sont considérés proches
    /// </summary>
    public float DistanceMax { get; }

    /// <summary>
    /// Position de l'objet
    /// </summary>
    Vector2f ColliderPos { get; }

    /// <summary>
    /// Détermine si les deux objets sont proches l'un de l'autre
    /// </summary>
    /// <returns>
    ///     <list type="number">
    ///     <item>Est-ce que les deux objets sont proches l'un de l'autre</item>
    ///     <item>Distance qui sépare les deux objets</item>
    ///     </list>
    /// </returns>
    (bool estProche, double distance) EstProche(IApprochable other)
    {
        double distance = Utilitaire.Distance(ColliderPos, other.ColliderPos);
        return (distance <= DistanceMax, distance);
    }
}