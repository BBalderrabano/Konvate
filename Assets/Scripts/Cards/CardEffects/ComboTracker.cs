using UnityEngine;
using System.Collections;

public class ComboTracker
{
    public int photonId;
    public string card_name;

    public ComboTracker(int photonId, string card_name)
    {
        this.photonId = photonId;
        this.card_name = card_name;
    }
}
