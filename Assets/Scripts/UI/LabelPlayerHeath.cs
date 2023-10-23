using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// Label changeable for player health.
    /// </summary>
    public class LabelPlayerHealth : LabelChangeable
    {
        public static readonly int k_LabelPlayerHealth = "PlayerHealth".GetHashCode();

        private void Awake()
        {
            Init(new Container(k_LabelPlayerHealth));
        }
    }
}