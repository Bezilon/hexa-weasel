using UnityEngine;
using System.Collections;

public class HexTile {
	// Basic properties of this tile
	public int Cost;
	public bool Passable;
	public bool BlockingTheView;
	// Global Position
	public Vector3 GlobalCoordinates;
	// Position in the Grid
	public Vector2 GridCoordinates;
	// Cube coordinates
	public Vector3 CubeCoordinates {
		get {
			return new Vector3(GridCoordinates.x, GridCoordinates.y, -GridCoordinates.x-GridCoordinates.y);
		}
		set {
			GridCoordinates = new Vector2(value.x,value.z);
		}
	}
	//The GameObject of this HexTile
	public GameObject HexObject;

	// Default Constructor
	public HexTile() {
		Passable = true;
		Cost = 1;
	}
	// Consturctor that knows the global and the grid position
	public HexTile(GameObject Hex_, float x, float y, int GrX, int GrY) {
		HexObject = Hex_;
		GlobalCoordinates = new Vector3(x, 0, y);
		GridCoordinates = new Vector2(GrX, GrY);
		Passable = true;
		BlockingTheView = false;
		Cost = 1;
	}
	// Consturctor that knows the global and the grid position
	public HexTile(GameObject Hex_, float x, float y, int GrX, int GrY, bool Passable_, int Cost_) {
		HexObject = Hex_;
		GlobalCoordinates = new Vector3(x, 0, y);
		GridCoordinates = new Vector2(GrX, GrY);
		Passable = Passable_;
		BlockingTheView = false;
		Cost = Cost_;
	}

	public void ColorHexTile(Color NewColor) {
		HexObject.GetComponent<Renderer>().material.color = NewColor;
	}
}