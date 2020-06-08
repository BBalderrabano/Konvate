using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO.UI;
using UnityEngine.UI;

namespace SO.UI
{
    public class UpdateTextFromPhaseController : UIPropertyUpdater
    {
        public PhaseVariable currentPhase;
        public TMPro.TextMeshProUGUI targetMeshText;

        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            if(GameManager.singleton.currentPlayer == null)
            {
                targetMeshText.text = "";
            }
            else
            {
                if (GameManager.singleton.currentPlayer.isLocal)
                {
                    targetMeshText.text = "Tu turno";
                }
                else
                {
                    targetMeshText.text = "Esperando al oponente";
                }
            }
        }
    }
}