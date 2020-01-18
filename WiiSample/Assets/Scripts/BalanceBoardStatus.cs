using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceBoardStatus : MonoBehaviour
{
    [SerializeField] Slider remainBatteryBar;

    void Update()
    {
        remainBatteryBar.value = BalanceBoardController.Instance.GetBalanceBoardBattery();
    }
}
