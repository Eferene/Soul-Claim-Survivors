using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timerText;
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (true)
        {
            float time = Time.timeSinceLevelLoad;
            int hours = Mathf.FloorToInt(time / 3600F);
            int minutes = Mathf.FloorToInt((time % 3600) / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            yield return new WaitForSeconds(1f);
        }
    }
}
