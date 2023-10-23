﻿using IndividualGames.CaseLib.DI;
using IndividualGames.CaseLib.UI;

namespace IndividualGames.UI
{
    /// <summary>
    /// Label changeable for player ammo counter.
    /// </summary>
    public class PlayerAmmoCounter : LabelChangeable
    {
        public static readonly int k_PlayerAmmoCounter = "Label.AmmoCounter".GetHashCode();

        private void Awake()
        {
            Init(new Container(k_PlayerAmmoCounter));
        }
    }
}