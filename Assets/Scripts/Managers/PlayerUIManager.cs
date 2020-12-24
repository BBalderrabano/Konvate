using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerHolder player;
    public TextMeshProUGUI bleedCount;
    public TextMeshProUGUI energyCount;

    public TextMeshProUGUI damageCounter;
    public TextMeshProUGUI healingCounter;

    Vector3 damageCounterPos;
    Vector3 healingCounterPos;

    void Start()
    {
        damageCounterPos = damageCounter.transform.position;
        healingCounterPos = healingCounter.transform.position;

        damageCounter.alpha = 0f;
        healingCounter.alpha = 0f;

        player.playerUI = this;
        UpdateBloodChips();
    }

    int damageTaken = 0;
    int healingReceived = 0;

    public void ShowHPChange(int amount)
    {
        float duration = 0.5f;

        if(amount > 0)  //Healing
        {
            LeanTween.cancel(healingCounter.gameObject);

            healingReceived += amount;

            healingCounter.text = "+" + healingReceived;

            healingCounter.LeanAlphaText(1f, duration).setFrom(0.7f);
            LeanTween.moveX(healingCounter.gameObject, healingCounterPos.x, duration).setFrom(bleedCount.transform.position.x).setEaseOutQuad();
            LeanTween.scale(healingCounter.gameObject, Vector2.one, duration).setFrom(new Vector2(0.7f, 0.7f)).setOnComplete(() =>
            {
                healingCounter.LeanAlphaText(0f, 1f).setDelay(4f).setOnComplete(()=> {
                    healingReceived = 0;
                });
            });
        }
        else if(amount < 0) //Damage
        {
            LeanTween.cancel(damageCounter.gameObject);

            damageTaken += amount;

            damageCounter.text = ""+damageTaken;

            damageCounter.LeanAlphaText(1f, duration).setFrom(0.7f);
            LeanTween.moveY(damageCounter.gameObject, damageCounterPos.y, duration).setFrom(bleedCount.transform.position.y).setEaseOutQuad();
            LeanTween.scale(damageCounter.gameObject, Vector2.one, duration).setFrom(new Vector2(0.7f, 0.7f)).setOnComplete(()=> 
            {
                damageCounter.LeanAlphaText(0f, 1f).setDelay(4f).setOnComplete(() => {
                    damageTaken = 0;
                });
            });
        }
    }

    public void UpdateEnergyCount()
    {
        if(player.photonId != GameManager.singleton.localPlayer.photonId && GameManager.singleton.turn.currentPhase.value is SetCardsPhase)
        {
            energyCount.text = "?";
        }
        else
        {
            energyCount.text = Mathf.Max(0, player.currentEnergy.value).ToString();
        }
    }

    public void UpdateBloodChips()
    {
        bleedCount.text = player.bleedCount.ToString();
    }

    public void UpdateAll()
    {
        UpdateBloodChips();
        UpdateEnergyCount();
    }
}
