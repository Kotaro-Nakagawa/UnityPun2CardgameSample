using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnShowdown();
public class ShowDown : MonoBehaviour
{

    public OnShowdown onShowdownDelegate;
    public void OnClick()
    {
        Debug.Log("SHOW DOWN!");
        onShowdownDelegate?.Invoke();
    }
}
