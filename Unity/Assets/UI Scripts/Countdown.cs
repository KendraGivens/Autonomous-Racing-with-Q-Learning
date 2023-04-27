using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public GameObject CountDown;
    public GameObject LapTimer;
    public GameObject CarControls;
    public AudioSource CountdownDing;
    public AudioSource Go;
    public AudioSource carEngine;
    public AudioSource carStart;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountStart());
        // disable car controls until after countdown 
        CarControls.GetComponent<CarController2>().enabled = false;
        carEngine.Pause(); 
        carStart.Pause();
    }
    IEnumerator CountStart()
    {
        // disable timer until after countdown 
        LapTimer.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        CountDown.GetComponent<TMPro.TextMeshProUGUI>().text = "3";
        CountdownDing.Play();  // play countdown sound 
        CountDown.SetActive(true);

        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TMPro.TextMeshProUGUI>().text = "2";
        CountdownDing.Play(); // play countdown sound 
        CountDown.SetActive(true);

        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TMPro.TextMeshProUGUI>().text = "1";
        CountdownDing.Play(); // play countdown sound 
        CountDown.SetActive(true);  

        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        Go.Play();     // play go sound

        // enabling lap timer 
        LapTimer.SetActive(true);

        // enabling car controls 
        CarControls.SetActive(true); 
        CarControls.GetComponent<CarController2>().enabled = true;
        carStart.Play();
        yield return new WaitForSeconds(.7f);
        carEngine.Play();
    }
  
}
