using KB4_PFI_Zelda;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution.Monstres;

/// <summary>
/// Classe abstraite qui définit ce que les monstres possèdent
/// </summary>
/// <remarks>
/// Il est préférable que les monstres possèdent 2 constructeurs: un constructeur avec tous les paramètres 
/// et un constructeur avec les stats définies. Ceci permet d'uniformiser la création de monstres.
/// </remarks>
abstract class Monstre : Personnage
{
    protected Monstre(string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility)
        : base(name, texture, position, path, health, attack, defence, agility) { }

    #region IsStupid
    /// <summary>
    /// Définit si le monstre bouge inconsciemment ou non
    /// </summary>
    public virtual bool IsStupid { get; } = true;
    List<Animation.Direction> UnseenDirections = new List<Animation.Direction>();
    #endregion

    /// <summary>
    /// Méthode qui gère les attaques
    /// </summary>
    public void DamageEvent(object? sender, EventArgs e)
    {
        if (sender == null || !IsAlive)
            return;

        Personnage personnage = (Personnage)sender;
        double distance = Utilitaire.Distance(personnage.ColliderPos, this.ColliderPos);

        if (distance <= personnage.Range)
        {
            personnage.Attack(this);

            if (!this.IsAlive)
                return;
        }

        // Permet à un enemi d'attaquer le héro sans ce faire attaquer
        if (distance <= this.Range)
            this.Attack(personnage);
    }

    #region Override Functions
    public override Color DebugColor => Color.Yellow;

    public override bool Move(Carte? map)
    {
        if (!IsAlive || map == null)
            return false;

        Vector2i velocity = Utilitaire.DirectionToVector2i(DIRECTION) * SPEED;
        bool result = false;
        bool forcedBlock = true;
        Vector2f tempPos = new Vector2f();

        if (DIRECTION != Animation.Direction.NEUTRAL && (velocity.X != 0 || velocity.Y != 0))
        {
            tempPos = Utilitaire.AddVector2f(Position, (Vector2f)velocity);
            (result, forcedBlock) = map.EstPositionValide(tempPos);
        }

        if (!forcedBlock && result)
        {
            Position = tempPos;

            if (!IsStupid && UnseenDirections.Count != 0)
                UnseenDirections.Clear();
        }
        else
        {
            if (!IsStupid)
                UnseenDirections.Add(DIRECTION);

            DIRECTION = Utilitaire.GetRandomDirection(IsStupid ? null : UnseenDirections.ToArray());
        }
        return false;
    }
    #endregion
}
