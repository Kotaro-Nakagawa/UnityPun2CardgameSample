using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public static PlayerAction instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int localPlayerNumber;
    public bool isMasterPlayer;

    public void GameSetup()
    {
        Debug.Log($"localAction isMasterPlayer is {isMasterPlayer}");
        if (isMasterPlayer)
        {
            GameMaster.instance.GameSetup();
        }
    }
    public void Draw(int targetActor, int cardId)
    {
        if (targetActor == localPlayerNumber)
        {
            GameManager.instance.CreateMyCard(cardId);
        }
        else
        {
            GameManager.instance.CreateEnemyCard(cardId);
        }
    }

    public void UseHand(int targetActor, int cardId)
    {
        Debug.Log($"use {targetActor}P hand {cardId}");
        if (targetActor == localPlayerNumber)
        {
            Debug.Log($"use my hand with cardId{cardId}");
            GameManager.instance.MoveMyHandCardToPod(cardId);
        }
        else
        {
            GameManager.instance.MoveEnemyHandCardToPod(cardId);
        }
    }
}
