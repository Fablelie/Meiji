using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCountDownGauge : MonoBehaviour {
	
	public float maxTime;
	public Text display;
	public Image gauge;

	private float currentTime;
    private WaitForSeconds wait = new WaitForSeconds(1);

	void Start()
	{
		display.text = maxTime.ToString();
		gauge.fillAmount = 0;
		StartCoroutine("CountDown");
		// StartCoroutine("AnimateGauge");
	}

	IEnumerator AnimateGauge()
	{
		while (currentTime > 0)
		{
			gauge.fillAmount += Time.deltaTime * 0.15f;
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator CountDown()
	{
        var w = new WaitForEndOfFrame();
		currentTime = maxTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			display.text = currentTime.ToString("0");
			// print(((currentTime / maxTime) * 100)*0.01f);
			gauge.fillAmount = (((maxTime-currentTime)/maxTime)*100)*0.01f;
			yield return w;
		}
	}
	
}
