
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

[RequireComponent(typeof(RawImage))]
public class QRCodeReader : MonoBehaviour {
	[SerializeField] private RawImage rawTexture;
	[SerializeField] private Button backBtn;
	private WebCamTexture camTexture;
	private Rect screenRect;

	private WaitForEndOfFrame endFrame = new WaitForEndOfFrame();

	private IBarcodeReader barcodeReader = new BarcodeReader();

	void OnEnable()
	{
		// Debug.Log("OnEnable");
		camTexture = new WebCamTexture();
		rawTexture.material.mainTexture = camTexture;
		camTexture.Play();
        StartCoroutine("Decoder");
	}

	void OnDisable()
	{
		StopCoroutine("Decoder");
		camTexture.Stop();
	}

	public void WebCamStop()
	{
		camTexture.Stop();
	}

	IEnumerator Decoder()
	{
		yield return endFrame;
		Result result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
        if (result != null) {
			WebCamStop();
			gameObject.SetActive(false);
			backBtn.gameObject.SetActive(false);
			GameManager.Instance.SetGameStation(result.Text);
            // print(result.Text);
		}
		else
			StartCoroutine(Decoder());
	}

}



