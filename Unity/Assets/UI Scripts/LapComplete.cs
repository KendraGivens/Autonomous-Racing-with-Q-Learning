using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapComplete : MonoBehaviour
{
    public GameObject LapCompleteTrigger;
    //public GameObject HalfLapTrigger; 

    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MilliDisplay;


    void OnTriggerEnter()
    {
        // make sure second count is static 
        if (LapTimeManager.SecondCount <= 9)
        {
            SecondDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "0" + LapTimeManager.SecondCount + ".";
        }
        else
        {
            SecondDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + LapTimeManager.SecondCount + ".";
        }

        if (LapTimeManager.SecondCount <= 9)
        {
            MinuteDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "0" + LapTimeManager.MinuteCount + ".";
        }
        else
        {
            MinuteDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + LapTimeManager.MinuteCount + ".";
        }

        MilliDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + LapTimeManager.MilliCount.ToString("F0");
        

        LapTimeManager.MinuteCount = 0;
        LapTimeManager.SecondCount = 0;
        LapTimeManager.MilliCount = 0;

        //HalfLapTrigger.SetActive(true);
        LapCompleteTrigger.SetActive(false);


    }
}
