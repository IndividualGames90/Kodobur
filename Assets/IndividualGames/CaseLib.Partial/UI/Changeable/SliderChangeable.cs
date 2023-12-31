﻿using UnityEngine;
using UnityEngine.UI;

namespace IndividualGames.CaseLib.UI
{
    /// <summary>
    /// Image specific unity hook for UIChangeable, slider version
    /// </summary>
    public class SliderChangeable : UIChangeable<float>
    {
        [SerializeField] protected Image image;

        private void Awake()
        {
            Init();
        }

        public override void OnChange(float changedValue)
        {
            try
            {
                image.fillAmount = changedValue;
            }
            catch (MissingReferenceException e)
            { }
        }
    }
}