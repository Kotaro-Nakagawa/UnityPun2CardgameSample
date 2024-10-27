using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnCardDrop(int cardId);

public class DropField : MonoBehaviour, IDropHandler
{
    [SerializeField] GameManager gameManager;

    public OnCardDrop onCardDropDelegate;

    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); // ドラッグしてきた情報からCardControllerを取得

        if (card.movement != null) // もしカードがあれば、
        {
            if (onCardDropDelegate != null)
            {
                onCardDropDelegate(card.ID);
            }
            else
            {
                Debug.Log("on card drop delegate is null");
            }
        }
    }
}
