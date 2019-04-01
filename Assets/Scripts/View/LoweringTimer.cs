using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LoweringTimer : MonoBehaviour
{
    private Text _timer;

    private void Start()
    {
        _timer = GetComponent<Text>();
        Events.TimerTic += UpdateTimer;
    }

    private void OnDestroy()
    {
        Events.TimerTic -= UpdateTimer;
    }

    private void UpdateTimer(int seconds)
    {
        _timer.text = seconds.ToString();
    }
}
