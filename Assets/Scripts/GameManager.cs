using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] GameObject turnNotificationPrefab;
    [SerializeField] GameObject enemyTurnNotificationPrefab;
    [SerializeField] GameObject resultWinPrefab;
    [SerializeField] GameObject resultLosePrefab;
    [SerializeField] GameObject resultDrawPrefab;

    [SerializeField] Transform playerHand;
    [SerializeField] Transform enemyHand;
    [SerializeField] Transform canvas;
    [SerializeField] Transform pod;
    [SerializeField] Transform callButton;

    // シングルトン化
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // StartGame();
    }

    public void StartGame()
    {
        // CreateCard(1, playerHand);
    }

    public void CreateCard(int cardId, bool isFaceUp, Transform trans)
    {
        // cardPrefabをtransに生成する
        CardController card = Instantiate(cardPrefab, trans);
        card.Init(cardId, isFaceUp);
    }

    public void CreateMyCard(int cardId)
    {
        CreateCard(cardId, true, playerHand);
    }

    public void CreateEnemyCard(int cardId)
    {
        CreateCard(cardId, false, enemyHand);
    }

    public async ValueTask NotifyTurn(bool isMyTurn)
    {
        GameObject turnNotifier = Instantiate(isMyTurn ? turnNotificationPrefab : enemyTurnNotificationPrefab, canvas);
        await Awaitable.WaitForSecondsAsync(1);
        Destroy(turnNotifier);
    }

    public void MoveMyHandCardToPod(int cardId)
    {
        var myHandCards = canvas.GetComponentsInChildren<CardController>();
        foreach (CardController card in myHandCards)
        {
            if (card.ID == cardId)
            {
                CreateCard(card.model.cardId, true, pod);
                Destroy(card.gameObject);
            }
        }
    }

    public void MoveEnemyHandCardToPod(int cardId)
    {
        var enemyHandCards = enemyHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in enemyHandCards)
        {
            if (card.ID == cardId)
            {
                CreateCard(card.model.cardId, true, pod);
                Destroy(card.gameObject);
            }
        }
    }

    public void SetOnDropCardToPod(OnCardDrop onCardDrop)
    {
        var dropField = pod.GetComponent<DropField>();
        dropField.onCardDropDelegate = onCardDrop;
    }

    public void SetShowDownDelegate(OnShowdown onShowdown)
    {
        var showdown = callButton.GetComponent<ShowDown>();
        showdown.onShowdownDelegate = onShowdown;
    }

    public int[] GetMyHandCards()
    {
        // Debug.Log(playerHand.GetComponentsInChildren<CardController>().Select(h => h.ID));
        return playerHand.GetComponentsInChildren<CardController>().Select(h => h.ID).ToArray();
    }

    public int[] GetEnemyHandCards()
    {
        return enemyHand.GetComponentsInChildren<CardController>().Select(h => h.ID).ToArray();
    }

    public void ShowAllCards()
    {
        var enemyHandCards = enemyHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in enemyHandCards)
        {
            card.Open();
        }
    }

    public void ShowResult(bool isWin)
    {
        Instantiate(isWin ? resultWinPrefab : resultLosePrefab, canvas);
    }

    public void ShowDraw()
    {
        Instantiate(resultDrawPrefab, canvas);
    }
}
