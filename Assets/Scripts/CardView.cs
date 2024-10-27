using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    public void Show(CardModel cardModel) // cardModelのデータ取得と反映
    {
        if (cardModel.isFaceUp)
        {
            nameText.text = cardModel.name;
        }
        else
        {
            nameText.text = "?";
        }
    }
}
