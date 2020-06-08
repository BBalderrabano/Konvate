using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;

namespace SO.UI
{
    public class UpdateText : UIPropertyUpdater
    {
        public StringVariable targetString;
        public Text targetText;
        public TMPro.TextMeshProUGUI targetMeshText;
        
        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            if (targetText != null)
            {
                targetText.text = targetString.value;
            }
            
            if (targetMeshText != null)
            {
                targetMeshText.text = targetString.value;
            }
        }
        
        public void Raise(string target)
        {
            if (targetText != null)
            {
                targetText.text = target;
            }

            if (targetMeshText != null)
            {
                targetMeshText.text = target;
            }
        }
    }
}
