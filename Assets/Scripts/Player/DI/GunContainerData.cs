using IndividualGames.ScriptableObjects;

namespace IndividualGames.Player
{
    /// <summary>
    /// GunController initialization data.
    /// </summary>
    public class GunContainerData
    {
        public PlayerInputs PlayerInputs;
        public GunStats GunStats;

        public GunContainerData(PlayerInputs playerInputs, GunStats gunStats)
        {
            PlayerInputs = playerInputs;
            GunStats = gunStats;
        }
    }
}