using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO.UI;
using UnityEngine.UI;

namespace SO.UI
{
    public class UpdateTextFromPhase : UIPropertyUpdater
    {
        public PhaseVariable currentPhase;
        public Text targetText;
        public TMPro.TextMeshProUGUI targetMeshText;

        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            if (targetText != null)
            {
                targetText.text = currentPhase.value.phaseDescription;
            }

            if (targetMeshText != null)
            {
                targetMeshText.text = currentPhase.value.phaseDescription;
            }
        }
    }
}