using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cardId;
    public string name;
    public bool isFaceUp;
    public CardModel(int cardID, bool isFaceUp) // データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);

        cardId = cardEntity.cardId;
        name = cardEntity.name;
        this.isFaceUp = isFaceUp;
    }
}
