using UnityEngine;
using UnityEngine.UI;

public class TestGround2Info : MonoBehaviour
{
    private Text _text;
    private DayAndNight _day_and_night;
	private void Awake()
    {
        _text = GameObject.Find("Output").transform.FindChild("Text").GetComponent<Text>();

        _day_and_night = GameObject.Find("DayAndNight").GetComponent<DayAndNight>();
	}
	
	private void FixedUpdate ()
    {
        _text.text = _day_and_night.Info();
	}
}
