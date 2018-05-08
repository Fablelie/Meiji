using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFontSize : MonoBehaviour {
	public Text ddd;
	Text text;
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		print(text.cachedTextGenerator.fontSizeUsedForBestFit);

		ddd.fontSize =text.cachedTextGenerator.fontSizeUsedForBestFit;
	}
}
