using UnityEngine;
using System.Collections;

public class HexTileHelper : MonoBehaviour {

	public static HexTile GetHexOfGmObj(GameObject a) {
		return (HexTile) a.GetComponent(typeof(HexTile));
	}
}
