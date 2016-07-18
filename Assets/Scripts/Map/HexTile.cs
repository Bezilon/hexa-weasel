using UnityEngine;
using System.Collections;

public class HexTile : MonoBehaviour {
    public static readonly Vector3[] Directions = {
        new Vector3(1,-1,0),
        new Vector3(1,0,-1),
        new Vector3(0,1,-1),
        new Vector3(-1,1,0),
        new Vector3(-1,0,1),
        new Vector3(0,-1,1)
    };

    // Basic properties of this tile
    public TerrainType Type;

	// Position in the Grid
	public Vector2 AxialCoordinates;
	// Cube coordinates
	public Vector3 CubeCoordinates {
		get {
			return new Vector3(AxialCoordinates.x, AxialCoordinates.y, -AxialCoordinates.x-AxialCoordinates.y);
		}
		set {
			AxialCoordinates = new Vector2(value.x,value.z);
		}
	}
}