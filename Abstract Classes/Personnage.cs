using KB4_PFI_Zelda;
using KB4_PFI_Zelda_solution.Interfaces;
using KB4_PFI_Zelda_solution.Items;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda_solution;

abstract class Personnage : Animation2, IApprochable, IDrawableDebug, IAttaquable, IPrintable
{
    #region Fields
    /// <summary>
    /// Name of the Character
    /// </summary>
    public string NAME { get; }

    /// <summary>
    /// Current health of the Character
    /// </summary>
    public int HEALTH { get; protected set; }

    /// <summary>
    /// Current attack stat of the Character (Deal more damage)
    /// </summary>
    public int ATTACK { get; protected set; }

    /// <summary>
    /// Current defence stat of the Character (Take less damage)
    /// </summary>
    public int DEFENCE { get; protected set; }

    /// <summary>
    /// Current agility stat of the Character (Bigger change to evade)
    /// </summary>
    public int AGILITY { get; protected set; }

    private int speed = 2;
    public int SPEED
    {
        get => speed;
        set
        {
            if (value >= 0)
                speed = value;
        }
    }

    public Texture? TEXTURE { get; }

    public Animation.Direction DIRECTION = Animation.Direction.NEUTRAL;

    public readonly ushort DICEFACES = 20;

    public bool IsLocked = false;
    #endregion

    #region Automatic Properties
    public bool IsAlive => HEALTH > 0;
    public bool IsMoving => DIRECTION != Animation.Direction.NEUTRAL;

    public Vector2f ColliderPos => Position + ColliderOffset;
    #endregion

    public Personnage(string name, Texture? texture, Vector2f position, string path, int health, int attack, int defence, int agility)
    : base(path, texture, position)
    //: base(texture, position)
    {
        NAME = name;
        HEALTH = health;
        ATTACK = attack;
        DEFENCE = defence;
        AGILITY = agility;
        SPEED = AGILITY;
    }

    public string ChangeHealth(int amount)
    {
        int prevHealth = HEALTH;
        HEALTH += amount;

        return $"({prevHealth} => {HEALTH})";
    }

    #region Virtual
    public virtual Vector2f ColliderOffset { get; }

    /// <summary>
    /// Distance à laquelle le personnage peut attaquer
    /// </summary>
    public virtual float Range => IAttaquable.Range;

    /// <summary>
    /// Couleur de debug correspondant au personnage
    /// </summary>
    public virtual Color DebugColor => Color.Blue;

    /// <summary>
    /// Distance à laquelle le personnage peut récolter des objets
    /// </summary>
    public virtual float DistanceMax => 10;

    /// <param name="target">Target of the attack</param>
    /// <returns>Is the target alive ?</returns>
    public virtual bool Attack(Personnage target)
    {
        if (Utilitaire.GenerateNombre(1, DICEFACES) >= target.AGILITY)
        {
            int dmg = (target as IAttaquable).SubirAttaque(this);
            this.EffectIfAttacking(target, dmg);

            Console.WriteLine(
                $"{this.NAME} did {dmg} DMG ({Math.Round(dmg * 100f / target.HEALTH, 2)}%)" +
                $" to {target.NAME} " + target.ChangeHealth(-dmg));
        }
        else
            Console.WriteLine($"{this.NAME} missed !");

        Console.WriteLine();
        return target.IsAlive;
    }

    /// <summary>
    /// Fonction appelée lorsque le personnage attaque une cible
    /// </summary>
    public virtual void EffectIfAttacking(Personnage target, int dmg) { }

    /// <summary>
    /// Fonction appelée lorsqu'un personnage subit une attaque
    /// </summary>
    /// <returns>Dégâts reçus</returns>
    public virtual int SubirAttaque(Personnage source) => CalculateDamage(source);

    /// <param name="source">Source of the attack</param>
    /// <returns>Amount of damage the character should take</returns>
    public virtual int CalculateDamage(Personnage source) => Math.Clamp(source.ATTACK - DEFENCE, 0, int.MaxValue);

    /// <summary>
    /// Fonction appelée lorsque le personnage utilise un boost
    /// </summary>
    public virtual void UseBoost(Item.ItemBoost boost)
    {
        Console.WriteLine(boost);

        HEALTH += boost.HEALTHGAIN;
        ATTACK += boost.ATTACKGAIN;
        DEFENCE += boost.DEFENCEGAIN;
        AGILITY += boost.AGILITYGAIN;

        Console.WriteLine(this);
    }
    #endregion

    #region Abstract Functions
    /// <summary>
    /// Cette méthode est responsable de gérer le déplacement d’un personnage
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public abstract bool Move(Carte? map);
    #endregion

    #region Override Functions
    public override string ToString() => IPrintable.ToString(this);
    public override string CurrentAnimation => DIRECTION.ToString().ToUpper(); // Pour Animation2

    //public override Animation.Direction Dir => DIRECTION; // Pour Animation

    /// <summary>
    /// Affiche le personnage dans la fenêtre donnée
    /// </summary>
    public override void Afficher(RenderWindow fenetre)
    {
        if (!IsAlive)
            return;

        base.Afficher(fenetre);

        if (Utilitaire.IsInDebug)
        {
            (this as IDrawableDebug).Draw(fenetre, DebugColor, this.DistanceMax / 2);
        }
    }
    #endregion
}