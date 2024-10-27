using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // カードの親要素
    public Transform cardParent;

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        // 元のカードの親要素を取得
        cardParent = transform.parent;

        Transform canvas = GameObject.Find("Canvas").GetComponent<Transform>();
        transform.SetParent(canvas, false);

        // blocksRaycastsをオフにする
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        // 親要素を変更
        transform.SetParent(cardParent, false);

        // blocksRaycastsをオンにする
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
