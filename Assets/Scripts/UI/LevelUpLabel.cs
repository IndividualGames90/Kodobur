using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// Label changeable for player level up.
    /// </summary>
    public class LevelUpLabel : LabelChangeable
    {
        public static readonly int k_LevelUpLabel = "Level".GetHashCode();

        private void Awake()
        {
            Init(new Container(k_LevelUpLabel));
        }
    }
}