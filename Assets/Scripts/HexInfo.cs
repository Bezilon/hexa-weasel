using UnityEngine;
using System.Collections;

public class HexInfo {
	//The GameObject of this HexTile
	private GameObject Hex;
	// Global Position
	public Vector3 GlobalCoordinates;
	// Position in the Grid
	public Vector2 GridCoordinates;
	// Cube coordinates
	public Vector3 CubeCoordinates {
		get {
			return new Vector3(GridCoordinates.x, GridCoordinates.y, -GridCoordinates.x-GridCoordinates.y);
		}
	}
	// Hex Properties
	public Vector2[] GridNeighbours;
	public Vector3[] CubeNeighbours;
	public bool Passable;
	public bool BlockingTheView;
	public int Cost;

	// Default Constructor
	public HexInfo() {
		Passable = true;
		Cost = 1;
	}
	// Consturctor that knows the global and the grid position
	public HexInfo(GameObject Hex_, float x, float y, int GrX, int GrY) {
		Hex = Hex_;
		GlobalCoordinates = new Vector3(x, 0, y);
		GridCoordinates = new Vector2(GrX, GrY);
		GridNeighbours = new Vector2[] {
			new Vector2(GrX+1, GrY), new Vector2(GrX+1, GrY-1), new Vector2(GrX, GrY-1),
			new Vector2(GrX-1, GrY), new Vector2(GrX-1, GrY+1), new Vector2(GrX, GrY+1)
		};
		CubeNeighbours = new Vector3[] {
			new Vector3(CubeCoordinates.x+1, CubeCoordinates.y-1,  CubeCoordinates.z), new Vector3(CubeCoordinates.x+1,  CubeCoordinates.y, CubeCoordinates.z-1), new Vector3(CubeCoordinates.x, CubeCoordinates.y+1, CubeCoordinates.z-1),
			new Vector3(CubeCoordinates.x-1, CubeCoordinates.y+1,  CubeCoordinates.z), new Vector3(CubeCoordinates.x-1,  CubeCoordinates.y, CubeCoordinates.z+1), new Vector3(CubeCoordinates.x, CubeCoordinates.y-1, CubeCoordinates.z+1)
		};
		Passable = true;
		BlockingTheView = false;
		Cost = 1;
	}
	// Consturctor that knows the global and the grid position
	public HexInfo(GameObject Hex_, float x, float y, int GrX, int GrY, bool Passable_, int Cost_) {
		Hex = Hex_;
		GlobalCoordinates = new Vector3(x, 0, y);
		GridCoordinates = new Vector2(GrX, GrY);
		GridNeighbours = new Vector2[] {
			new Vector2(GrX+1, GrY), new Vector2(GrX+1, GrY-1), new Vector2(GrX, GrY-1),
			new Vector2(GrX-1, GrY), new Vector2(GrX-1, GrY+1), new Vector2(GrX, GrY+1)
		};
		CubeNeighbours = new Vector3[] {
			new Vector3(CubeCoordinates.x+1, CubeCoordinates.y-1,  CubeCoordinates.z), new Vector3(CubeCoordinates.x+1,  CubeCoordinates.y, CubeCoordinates.z-1), new Vector3(CubeCoordinates.x, CubeCoordinates.y+1, CubeCoordinates.z-1),
			new Vector3(CubeCoordinates.x-1, CubeCoordinates.y+1,  CubeCoordinates.z), new Vector3(CubeCoordinates.x-1,  CubeCoordinates.y, CubeCoordinates.z+1), new Vector3(CubeCoordinates.x, CubeCoordinates.y-1, CubeCoordinates.z+1)
		};
		Passable = Passable_;
		BlockingTheView = false;
		Cost = Cost_;
	}

	public void ColorHexTile(Color NewColor) {
		Hex.GetComponent<Renderer>().material.color = NewColor;
	}
}