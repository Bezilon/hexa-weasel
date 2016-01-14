using UnityEngine;
using System.Collections;

public class HexTile : MonoBehaviour {
	// Basic properties of this tile
	public int Cost;
	public bool Passable;
	public bool BlockingView;

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
	
	public static readonly Vector3[] Directions = new [] {
		new Vector3(1,-1,0),
		new Vector3(1,0,-1),
		new Vector3(0,1,-1),
		new Vector3(-1,1,0),
		new Vector3(-1,0,1),
		new Vector3(0,-1,1)
	};
	
	public void ColorHexTile(Color TileColor) {
		gameObject.GetComponent<Renderer>().material.color = TileColor;
	}
	
	/*
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
	} */
	
}