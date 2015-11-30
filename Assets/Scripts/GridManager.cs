using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	public Vector2 GridSize;
	public float HexRadius;

	public Vector2 PlayerGridPosition;
	public Vector2 TestGoalGridPosition;
	public Vector3 PlayerCubePosition {
		get {
			return new Vector3(PlayerGridPosition.x, PlayerGridPosition.y, -PlayerGridPosition.x-PlayerGridPosition.y);
		}
	}
	// Default Tile Object
	public GameObject DefaultTile;

	[HideInInspector]
	public static readonly Vector3[] Directions = new [] {
		new Vector3(1,-1,0),
		new Vector3(1,0,-1),
		new Vector3(0,1,-1),
		new Vector3(-1,1,0),
		new Vector3(-1,0,1),
		new Vector3(0,-1,1)
	};
	[HideInInspector]
	private List<HexTile> HexList = new List<HexTile>();

	// Use this for initialization
	void Start() {
		PlayerGridPosition = new Vector2(0.0F,0.0F);
		CreateBasicGrid();
		HighlightPath( SimplePath( FindHex( PlayerGridPosition ), FindHex( TestGoalGridPosition ) ) );
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

		for (int GridX = 0; GridX < (int)GridSize.x; GridX++) {
			OddRowPush = ( GridX % 2 == 1 ) ? ( YStep / 2 ) : 0;
			for (int GridY = 0; GridY < (int)GridSize.y; GridY++) {
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

	public int HeuristicHexDistance(HexTile a, HexTile b) {
		return (Math.Abs((int)a.CubeCoordinates.x - (int)b.CubeCoordinates.x) + Math.Abs((int)a.CubeCoordinates.y - (int)b.CubeCoordinates.y) + Math.Abs((int)a.CubeCoordinates.z - (int)b.CubeCoordinates.z)) / 2;
	}

	// A* Search based simple path
	public List<HexTile> SimplePath(HexTile start, HexTile goal) {
		List<HexTile> Path = new List<HexTile>();
		Dictionary<HexTile, HexTile> CameFrom = new Dictionary<HexTile, HexTile>();
		Dictionary<HexTile, int> CostSoFar = new Dictionary<HexTile, int>();
		PriorityQueue<HexTile> frontier = new PriorityQueue<HexTile>();
		frontier.Enqueue(start, 0);
		
		CameFrom[start] = start;
		CostSoFar[start] = 0;
		
		while (frontier.Count > 0) {
			HexTile current = frontier.Dequeue();
			
			if (current.Equals(goal)) break;
			
			foreach (HexTile next in Neighbors(current)) {
				int newCost = CostSoFar[current] + next.Cost;
				if ( !CostSoFar.ContainsKey(next) || newCost < CostSoFar[next]) {
					CostSoFar[next] = newCost;
					int priority = newCost + HeuristicHexDistance(next, goal);
					frontier.Enqueue(next, priority);
					CameFrom[next] = current;
				}
			}
		}

		HexTile RevCurrent = goal;
		Path.Add(goal);
		Path.Add(CameFrom[RevCurrent]);
		while (RevCurrent != start) {
			RevCurrent = CameFrom[RevCurrent];
			Path.Add(RevCurrent);
		}
		Path.Remove(start);
		Path.Reverse();
		return Path;
	}

	public void HighlightPlayerPosition() {
		FindHex( new Vector2(PlayerGridPosition.x, PlayerGridPosition.y) ).ColorHexTile(new Color(1, 0, 0, 1));
	}

	public void HighlightPath(List<HexTile> Path) {
		float blue = 0.2F;
		float green = 1.0F;
		foreach (HexTile CurrentTile in Path) {
			CurrentTile.ColorHexTile(new Color(0,green,blue,1));
			blue *= 1.5F;
			green /= 1.5F;
		}
	}
}
