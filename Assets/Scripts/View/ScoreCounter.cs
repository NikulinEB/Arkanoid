using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreCounter : MonoBehaviour
{
    private Text _counter;

    private void Start()
    {
        _counter = GetComponent<Text>();
        UpdateCounter(0);
        Events.ScoreChanged += UpdateCounter;
    }

    private void OnDestroy()
    {
        Events.ScoreChanged -= UpdateCounter;
    }

    private void UpdateCounter(int score)
    {
        _counter.text = "Score: " + score;
    }

}
