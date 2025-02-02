using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject VisualGameObject;
    
    private void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnOnSelectedCounterChange;
    }

    private void Player_OnOnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeEventArgs e)
    {
        if (e._selectedCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        VisualGameObject.SetActive(true);
    }

    private void Hide()
    {
        VisualGameObject.SetActive(false);
    }
}
