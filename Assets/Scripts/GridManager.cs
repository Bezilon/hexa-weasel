using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	public Vector2 GridSize;
	public float HexRadius;
	[HideInInspector]

	public static readonly Vector3[] Directions = new [] {
		new Vector3(1,-1,0),
		new Vector3(1,0,-1),
		new Vector3(0,1,-1),
		new Vector3(-1,1,0),
		new Vector3(-1,0,1),
		new Vector3(0,-1,1)
	};

	public Vector2 PlayerGridPosition;
	public Vector3 PlayerCubePosition {
		get {
			return new Vector3(PlayerGridPosition.x, PlayerGridPosition.y, -PlayerGridPosition.x-PlayerGridPosition.y);
		}
	}

	// Default Tile Object
	public GameObject DefaultTile;

	private List<HexTile> HexList = new List<HexTile>();

	// Use this for initialization
	void Start() {
		PlayerGridPosition = new Vector2(0.0F,0.0F);
		CreateBasicGrid();
	}
	
	// Update is called once per frame
	void Update() {
		//Highlight Player position
		HighlightPlayerPosition();
	}

	public bool HexInBounds(HexTile a) {
		return (a.GridCoordinates.x >= 0.0F) && (a.GridCoordinates.y >= 0.0F);
	}
	
	public IEnumerable<HexTile> Neighbors(HexTile a) {
		foreach (Vector3 dir in Directions) {
			Vector3 NextHexCoords = new Vector3(a.CubeCoordinates.x + dir.x, a.CubeCoordinates.y + dir.y, a.CubeCoordinates.z + dir.z);
			HexTile NextHex = FindHex(NextHexCoords);
			if ( NextHex != null && HexInBounds(NextHex) && NextHex.Passable) {
				yield return NextHex;
			}
		}
	}

	private void CreateBasicGrid() {
		float YStep = Mathf.Sqrt( HexRadius*HexRadius - HexRadius*HexRadius/4 );
		float XStep = HexRadius - Mathf.Sqrt( HexRadius*HexRadius - YStep*YStep )/2;
		float OddRowPush = 0.0F;
		Vector3 GlobalCoordinates = new Vector3(0.0F, 0.0F, 0.0F);
		GameObject CurrentTile;

		for(int GridX = 0; GridX < (int)GridSize.x; GridX++) {
			OddRowPush = ( GridX % 2 == 1 ) ? ( YStep / 2 ) : 0;
			for(int GridY = 0; GridY < (int)GridSize.y; GridY++) {
				try {
					GlobalCoordinates.x = (GridY*YStep)+OddRowPush;
					GlobalCoordinates.y = -GridX*XStep;
					CurrentTile = (GameObject)Instantiate(DefaultTile, new Vector3(GlobalCoordinates.x, 0, GlobalCoordinates.y), Quaternion.identity);
					CurrentTile.name = "[" + GridY + "," + GridX + "]";
					CurrentTile.transform.SetParent(this.transform);
					HexList.Add(new HexTile(CurrentTile, GlobalCoordinates.x, GlobalCoordinates.y, GridX, GridY, true, 1));
				}
				catch( UnityException e ) {
					Debug.Log( "Caught an error: " + e );
				}
			}
		}
	}

	private void GenerateTerrain() {
		
	}

	private void LoadTerrain() {
		
	}

	public HexTile FindHex(Vector3 a) {
		return HexList.Find( h => (h.CubeCoordinates == a) );
	}

	public HexTile FindHex(Vector2 a) {
		return HexList.Find( h => (h.GridCoordinates == a) );
	}

	public HexTile FindHex(HexTile a) {
		return HexList.Find( h => (h == a) );
	}

	public int HexDistance(HexTile a, HexTile b) {
		return (Math.Abs((int)a.CubeCoordinates.x - (int)b.CubeCoordinates.x) + Math.Abs((int)a.CubeCoordinates.y - (int)b.CubeCoordinates.y) + Math.Abs((int)a.CubeCoordinates.z - (int)b.CubeCoordinates.z)) / 2;
	}

	// Using A* path finding with optional count for steps
	/*public List<HexTile> GetQuickestPath(HexTile a, HexTile b, int AllowedSteps = 5) {


		int CurrentDistance = HexDistance(a,b);
		int HCost = 0;
		int TempHCost = 0;
		HexTile CurrentTile = a;
		HexTile CheapestNeighbour = a;

		List<HexTile> Path = new List<HexTile>();
		List<HexTile> Frontier = new List<HexTile>();
		List<HexTile> Visited = new List<HexTile>();

		for( int StepsTaken = 0; CurrentDistance != 0 || StepsTaken < AllowedSteps; StepsTaken++ ) {
			HCost = 0;

			// Looping through each neighbour of the current tile to find the cheapest path
			foreach(Vector3 CurrentNeighbourCube in CurrentTile.CubeNeighbours) {
				HexTile CurrentNeighbour = FindHexByCube(CurrentNeighbourCube);
				if(CurrentNeighbour.Passable) {
					TempHCost = HexDistance(CurrentNeighbour, a) + HexDistance(CurrentNeighbour, b);
					if( HCost == 0 || TempHCost < HCost ) {
						HCost = TempHCost;
						CheapestNeighbour = CurrentNeighbour;
					}
				}
			}

			// If we found a neighbour to step to and it's not already in the list
			if( CheapestNeighbour != CurrentTile && !Path.Contains(CurrentTile) ) {
				CurrentTile = CheapestNeighbour;
				CurrentDistance = HexDistance(CurrentTile, b);
				// Don't add the tile to the path if it already exists
				Path.Add(CurrentTile);
			// Otherwise we found an incomplete path
			} else {
				// Making sure the destination is not in the list (the path is really broken)
				// and because of the nature of the algorithm we need to remove the
				// very last step so we exactly hit the obstacle instead of "recoiling"
				if( !Path.Contains(b) ) Path.Remove(CurrentTile);
				CurrentDistance = 0;
			}
		}

		return Path;
	}*/

	public void HighlightPlayerPosition() {
		FindHex( new Vector2(PlayerGridPosition.x, PlayerGridPosition.y) ).ColorHexTile(new Color(1, 0, 0, 1));
	}
	
}
