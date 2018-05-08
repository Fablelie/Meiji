using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayClicker : MonoBehaviour {

	public GameObject obj;
	void OnEnable()
	{
		obj.SetActive(true);
		StartCoroutine(Wait());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(3);
		obj.SetActive(false);
	}
}
