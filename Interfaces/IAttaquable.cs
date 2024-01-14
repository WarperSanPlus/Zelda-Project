namespace KB4_PFI_Zelda_solution.Interfaces;

/// <summary>
/// Interface permettant d'être attaqué
/// </summary>
interface IAttaquable
{
    /// <summary>
    /// Distance à laquelle les enemis peuvent se toucher
    /// </summary>
    const float Range = 24;

    /// <summary>
    /// Méthode responsable de gérer les dommages engendrés par 
    /// une attaque perpétrée par le personnage passé en paramètre 
    /// </summary>
    /// <param name="source">Source de l'attaque</param>
    /// <returns>Nombre de dégats (Positif)</returns>
    int SubirAttaque(Personnage source);
}