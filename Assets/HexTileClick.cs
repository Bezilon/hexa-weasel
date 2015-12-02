using UnityEngine;
using System.Collections;

public class HexTileClick : MonoBehaviour {

	void OnEnable()
	{
		EventManager.OnClicked += ColorHexTile;
	}
	
	
	void OnDisable()
	{
		EventManager.OnClicked -= ColorHexTile;
	}
	
	
	void ColorHexTile()
	{
		this.GetComponent<Renderer>().material.color = new Color(1,1,0,1);
	}
}
