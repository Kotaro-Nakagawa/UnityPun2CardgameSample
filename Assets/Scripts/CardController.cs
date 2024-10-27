using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; // カードの見た目の処理
    public CardModel model; // カードのデータを処理
    public CardMovement movement; // カードの動きを処理

    public int ID { get { return model.cardId; } }

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID, bool isFaceUp) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, isFaceUp); // カードデータを生成
        view.Show(model); // 表示
    }

    public void Open()
    {
        model.isFaceUp = true;
        view.Show(model);
    }
}
