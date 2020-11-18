using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Orco/Fuerza en Numeros")]
public class CE_FuerzaEnNumeros : CardEffect
{
    public override void Execute()
    {
        base.Execute();

        Debug.Log("PLAY CARD FROM HAND");

    }
}

