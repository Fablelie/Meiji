using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Game/Station")]
public class Station : ScriptableObject {
	public string stationName;
	public string[] stationTitle;
	public int stationIndex;
	public bool isSpecialStation = false;
	public Question[] questions;
	[HideInInspector] public ReactiveProperty<int> score = new ReactiveProperty<int>();
	public bool isClear = false;
}
