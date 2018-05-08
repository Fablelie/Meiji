using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerPanelController : MonoBehaviour {

	[SerializeField] private GameObject renderTexture;
    [SerializeField] private GameObject backBtn;

    public static ScannerPanelController m_instance;

    public static ScannerPanelController Instance
    {
        get
        {
            return m_instance;
        }
        set
        {
            m_instance = value;
        }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

	public void SetEnableScanner(bool isEnable)
	{
        GameManager.Instance.SetEnableVuforia(isEnable);
        renderTexture.SetActive(isEnable);
        backBtn.SetActive(isEnable);
	}

	public void OnDetection(string stationCode)
	{
		GameManager.Instance.SetGameStation(stationCode);
	}
}
