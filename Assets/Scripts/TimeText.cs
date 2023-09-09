using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text += IndicatorGoalConnector.elapsed.ToString() + "s";
    }
}
