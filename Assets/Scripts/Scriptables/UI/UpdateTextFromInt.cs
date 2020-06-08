using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;

namespace SO.UI
{
    public class UpdateTextFromInt : UIPropertyUpdater
    {
        public IntVariable targetInt;
        public TMPro.TextMeshProUGUI targetMeshText;
        
        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            targetMeshText.text = targetInt.value.ToString();
        }
        
        public void Raise(int target)
        {
            targetMeshText.text = target.ToString();
        }
    }
}
