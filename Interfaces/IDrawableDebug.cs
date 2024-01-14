using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Interfaces;

/// <summary>
/// Interface permettant d'afficher la collision circulaire de l'objet
/// </summary>
interface IDrawableDebug : IApprochable
{
    /// <summary>
    /// Affiche une collision circulaire selon le rayon 
    /// à la position du collider de l'objet
    /// </summary>
    /// <param name="window">Fenêtre dans laquelle afficher le cercle</param>
    /// <param name="color">Couleur du cercle</param>
    /// <param name="radius">Rayon du cercle</param>
    void Draw(RenderWindow window, Color color, float radius)
    {
        CircleShape collider = new CircleShape(radius);
        collider.FillColor = color;
        collider.Origin = new Vector2f(collider.Radius, collider.Radius);
        collider.Position = this.ColliderPos;
        window.Draw(collider);
    }
}
