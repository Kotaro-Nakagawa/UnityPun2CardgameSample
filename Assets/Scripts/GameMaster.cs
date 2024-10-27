using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    List<int> deck = new() { 1, 2, 3, 4, 5, 6, 7 };
    int turnPlayer = 1;
    public void GameSetup()
    {
        // デッキのシャッフル
        for (int i = 0; i < deck.Count; i++)
        {
            int temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Deal(1);
        Deal(2);
        _ = TurnStart();
    }

    void Deal(int target)
    {
        int cardId = deck[0];
        deck.RemoveAt(0);
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Draw", RpcTarget.All, target, cardId);
    }

    [PunRPC]
    void Draw(int target, int cardId)
    {
        PlayerAction.instance.Draw(target, cardId);
    }

    async ValueTask TurnStart()
    {
        PhotonView photonView = PhotonView.Get(this);
        if (deck.Count == 0)
        {
            photonView.RPC("ResultDraw", RpcTarget.All);
            return;
        }
        Debug.Log("TurnStart");
        photonView.RPC("SetUIDelegates", RpcTarget.All, turnPlayer);
        photonView.RPC("NotifyTurn", RpcTarget.All, turnPlayer);
        await Awaitable.WaitForSecondsAsync(1);
        Deal(turnPlayer);
    }

    [PunRPC]
    void ResultDraw()
    {
        GameManager.instance.ShowDraw();
    }

    [PunRPC]
    void NotifyTurn(int turnActorNumber)
    {
        Debug.Log("NotifiTurn");
        _ = GameManager.instance.NotifyTurn(PlayerAction.instance.localPlayerNumber == turnActorNumber);
    }

    [PunRPC]
    void SetUIDelegates(int turnActorNumber)
    {
        if (PlayerAction.instance.localPlayerNumber == turnActorNumber)
        {
            Debug.Log("set delegate as usable");
            GameManager.instance.SetOnDropCardToPod((int cardId) =>
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("MoveTurnPlayersHandCardToPod", RpcTarget.All, cardId, turnActorNumber);
            });
            GameManager.instance.SetShowDownDelegate(() =>
            {
                PhotonView photonView = PhotonView.Get(this);
                int[] myCards = GameManager.instance.GetMyHandCards();
                int[] enemyCards = GameManager.instance.GetEnemyHandCards();
                // ターンプレイヤーなので、myCards の count は 2 で enemyCards の count は 1 の筈
                int winner = 3 - turnActorNumber; // turnActor が 1 なら 2、turnActor が 2 なら 1
                if (myCards.Count() != 2 || enemyCards.Count() != 1)
                {
                    // 何等かの不具合
                    Debug.Log("何等かの不具合");
                    Debug.Log($"myCardsCount = {myCards.Count()} / enemyCardsCount = {enemyCards.Count()}");
                }
                int difference = System.Math.Abs(myCards[0] - myCards[1]);
                int sum = myCards[0] + myCards[1];
                if (difference > enemyCards[0] || sum < enemyCards[0])
                {
                    winner = turnActorNumber;
                }
                photonView.RPC("Showdown", RpcTarget.All, winner);
            });
        }
        else
        {
            GameManager.instance.SetOnDropCardToPod((int cardId) => { });
        }
    }

    [PunRPC]
    void MoveTurnPlayersHandCardToPod(int cardId, int turnActorNumber)
    {
        PlayerAction.instance.UseHand(turnActorNumber, cardId);
        if (PlayerAction.instance.isMasterPlayer)
        {
            TurnSwitch();
            _ = TurnStart();
        }
    }

    [PunRPC]
    void Showdown(int winner)
    {
        GameManager.instance.ShowAllCards();
        GameManager.instance.ShowResult(PlayerAction.instance.localPlayerNumber == winner);
    }

    void TurnSwitch()
    {
        if (turnPlayer == 1) { turnPlayer = 2; } else { turnPlayer = 1; }
    }
}
