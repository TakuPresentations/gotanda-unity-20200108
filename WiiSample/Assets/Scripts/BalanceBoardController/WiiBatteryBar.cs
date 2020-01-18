using UnityEngine;
using UnityEngine.UI;

public class WiiBatteryBar : MonoBehaviour
{
    [SerializeField] Slider remainBatteryBar;
    // GetExpType == 3 はBaranceBoard
    [SerializeField] int wiiExpType = 3;

    void Update()
    {
        for (int i = 0; i < Wii.GetRemoteCount(); ++i)
        {
            if (Wii.IsActive(i) && Wii.GetExpType(i) == wiiExpType)
            {
                remainBatteryBar.value = Wii.GetBattery(i);
                break;
            }
        }
    }
}
