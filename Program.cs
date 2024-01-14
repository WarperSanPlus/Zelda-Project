// NOM: WarperSan
// DATE (DD/MM/YYYY): 02/05/2023 - 12/05/2023

// TOUCHES:
//  Haut: W/Up Arrow
//  Bas: S/Down Arrow
//  Gauche: A/Left Arrow
//  Droite: D/Right Arrow
//  Toggle Debug Menu: F10
//  Debug Speed up: NumPad +
//  Debug Speed down: NumPad -
//  Debug Speed reset: NumPad /

#region Carnet de modification
/* DATE: 2 MAI 2023
 * CHANGEMENTS:
 *  - Début de développement d'un système de contrôles
 *  - Début de développement d'un debug mode
 *  
 *  - Implémentation d'un système de mouvement "intelligent" (À TESTER)
 *  - Implémentation d'un système de VSYNC (Kinda)
 *  
 *  - Modification de stockage de la carte (Image => Bool[,])
 *  - Modification des extrants de Carte.EstPositionValide() (bool => bool, bool)
 *  - Modification de l'enum Animation.Direction pour inclure un état neutre
 *  - Modification de la fonction Animation.Afficher() pour gérer l'état neutre
 *  
 *  - Amélioration du système de mouvement
 *  
 *  - Complétion de l'interface IApprochable
 *  - Complétion de 'interface IAttaquable
 *  - Complétion de la classe Personnage
 *  - Complétion partielle de la classe Héros
 *  - Complétion de la classe Monstre
 * ---
 * DATE: 3 MAI 2023
 * CHANGEMENTS:
 *  - Début du développement d'un menu de Debug
 * 
 *  - Implémentation partielle du spawn des monstres
 *  - Implémentation d'un système de sauvegarde et de chargement de contrôles
 *  
 *  - Modification de l'ordre d'affichage (Celui avec le plus grand Y s'affiche en dernier)
 *  
 *  - Complétion des classes Zombie, Squelette, Dino
 *  ---
 *  DATE: 4 MAI 2023
 *  CHANGEMENTS:
 *   - Début de documentation
 *   
 *   - Implémentation de l'affichage pour les combats
 *   - Implémentation d'un système de UI avec layers
 *   - Implémentation de la méthode Personnage.EffectIfAttacking() (Utilisée pour le Squelette)
 *   
 *   - Modification du nom de la constante Program.Largeur => Program.InitLargeur
 *   - Modification du nom de la constante Program.Hauteur => Program.InitHauteur
 *   - Modification de la visibilité des constantes Program.InitLargeur & Program.InitHauteur (private => public)
 *   
 *   - BUG FIX: Moonwalking enemies
 *   
 *   - Complétion du système de récolte d'items
 *  ---
 *  DATE: 5 MAI 2023
 *  CHANGEMENTS:
 *   - Implémentation de l'interface IComparable dans la classe Animation
 *  
 *   - Modification de la classe parent de la classe Item (Sprite => Animation)
 *   
 *   - Complétion d'un nouveau système d'Animation (Plus contrôlable, mais un peu plus lent)
 *  ---
 *  DATE 6 MAI 2023
 *  CHANGEMENTS:
 *   - Implémentation d'un système de caméra clamp
 *   - Implémentation d'un système de fenêtre de jeu stable (aspect ratio)
 *   - Implémentation des constantes UIRight et UIBottom pour placer des éléments à partir de ces directions
 *   - Implémentation d'un constructeur Carte(string, string)
 *   
 *   - Modification du ratio (640;480 => 640;640)
 *   - Modification du zoom (1 => 0.5)
 *   - Modification de la variable pour quitter le jeu (bQuitter => Heros.LeaveGame)
 *   - Modification de la variable pour si le joueur est mort (bLink => Personnage.IsAlive)
 *   - Modification de l'enum SorteDeMonstres par un array de Type Monstre
 *   - Modification de la variable pour savoir la taille du monde (cs => Carte.Hauteur;Carte.Largeur)
 *   - Modificateur du construteur utilisé pour la Carte (Carte(Texture, Image) => Carte(string, string))
 *   
 *   - BUG FIX: Enemies stayed on NEUTRAL until they first hit a wall
 *  ---
 *  DATE: 7 MAI 2023
 *  CHANGEMENTS:
 *   - Implémentation d'un nouvel item: Potion de dégât (-5 HP)
 *  ---
 *  DATE: 8 MAI 2023
 *  CHANGEMENTS:
 *   - Début d'un système de cutscene
 *   
 *   - Implémentation d'un nouvel item: Bouclier nouveau (+7 DF; -1 AGL)
 *   - Implémentation d'un système de Flag (Principalement pour les cutscenes)
 *   - Implémentation d'un nouvel ennemi: Bee (https://admurin.itch.io/top-down-mobs-bee)
 *   
 *   - Modification de la classe qui gère le jeu (Program => GameManager)
 *   - Modification de la variable pour si le joueur est mort (Personnage.IsAlive => GameManager.RequestLeave)
 *   - Modification de la variable qui détermine la porté de IApprochable (IApproche.DistanceMax => /.DistanceMax)
 *  ---
 *  DATE: 9 MAI 2023
 *  CHANGEMENTS:
 *   - Implémentation d'un système de Loading Zones
 *   - Implémentation de nouveaux flags (RequestLeave, LoadingZoneFound)
 *   
 *   - Modification du boost de défense par NewShield (7 => 3)
 *   - Modification de la variable qui contient Link (Link (local) => static Link)
 *   - Modification de la variable qui contient la fenêtre (fenetre (local) => static fenetre)
 *  ---
 *  
 *  KNOWN BUGS:
 *  - Items visually appear in walls
 *  - Camera can be stuck on walls sometimes
 *  - Bees collider is higher than it's position
 *  - Camera can flicker when going through a loading zone
 *  
 * IDÉES:
 *  1. UIs (DONE)
 *  2. Musics
 *  3. SFXs
 *  4. More Monsters (DONE)
 *  5. Cutscene manager (DONE)
 *  6. Loading zone (DONE)
 *  7. Better Camera (DONE)
 *  
 *  Better animation système (DONE)
 */
#endregion

using KB4_PFI_Zelda_solution;
using KB4_PFI_Zelda_solution.Managers;
using SFML.Graphics;
using SFML.System;

namespace KB4_PFI_Zelda;

class Program
{
    static void Main(string[] args)
    {
        GameManager.Init();
    }
}