using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class GameState : ScriptableObject
{
    public PlayerAction[] actions;

    public void Tick(float d) {

        for (int i = 0; i < actions.Length; i++) {

            actions[i].Execute(d);

        }
    }
}
