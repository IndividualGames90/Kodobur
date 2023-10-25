using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// Label changeable for player kill counter.
    /// </summary>
    public class PlayerKillCounter : LabelChangeable
    {
        public static readonly int k_PlayerKillCounter = "KillCounter".GetHashCode();

        private void Awake()
        {
            Init(new Container(k_PlayerKillCounter));
        }
    }
}