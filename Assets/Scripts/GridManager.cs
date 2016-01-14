using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	public Vector2 GridSize;
	public float HexRadius;

	public Vector2 TestGoalGridPosition;

	// Default Tile Object
	public GameObject DefaultTile;

	[HideInInspector]
	public static Vector2 PlayerGridPosition;
	public Vector3 PlayerCubePosition {
		get {
			return new Vector3(PlayerGridPosition.x, PlayerGridPosition.y, -PlayerGridPosition.x-PlayerGridPosition.y);
		}
	}

	// Use this for initialization
	void Start() {
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
		foreach (Vector3 dir in HexTile.Directions) {
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
		bool RandomPassable;
		bool PlayerPlaced = false;
		int GridCorrector = 0;
		Vector3 GlobalCoordinates = new Vector3(0.0F, 0.0F, 0.0F);
		GameObject CurrentTile;
		HexTile TempHex;
		System.Random Rand = new System.Random();

		for (int GridX = 0; GridX < (int)GridSize.x; GridX++) {
			OddRowPush = ( GridX % 2 == 1 ) ? ( YStep / 2 ) : 0;
			for (int GridY = GridCorrector; GridY < (int)GridSize.y + GridCorrector; GridY++) {

				GlobalCoordinates.x = ( ( GridY - GridCorrector ) * YStep ) + OddRowPush;
				GlobalCoordinates.y = -GridX*XStep;
				
				//Instantiating a tile 
				CurrentTile = (GameObject)Instantiate(DefaultTile, new Vector3(GlobalCoordinates.x, 0, GlobalCoordinates.y), Quaternion.identity);
				CurrentTile.name = "[" + GridY + "," + GridX + "]";
				CurrentTile.transform.SetParent(this.transform);
				CurrentTile.AddComponent<HexTile>();
				CurrentTile.GetComponent<HexTile>().GridCoordinates = new Vector2(GridX, GridY);
				CurrentTile.GetComponent<HexTile>().Passable = RandomPassable;
				CurrentTile.GetComponent<HexTile>().Cost = 1;
				
				/*RandomPassable = Rand.NextDouble() > 0.2;
				TempHex = new HexTile(CurrentTile, GlobalCoordinates.x, GlobalCoordinates.y, GridX, GridY, RandomPassable, 1);
				TempHex.ColorHexTile(new Color(Convert.ToInt32(RandomPassable), Convert.ToInt32(RandomPassable), Convert.ToInt32(RandomPassable),1));
				if (!PlayerPlaced && RandomPassable) { 
					PlayerGridPosition = new Vector2(GridX, GridY);
					PlayerPlaced = true;
				}
				HexList.Add(TempHex);*/
					
			}

			if( GridX % 2 == 1 ) GridCorrector--;
		}
	}

	private void GenerateTerrain() {
		
	}

	private void LoadMap() {
		
	}

	public HexTile FindHex(Vector3 a) {
		transform.Find("[" + a.z + "," + a.x + "]");
		//return HexList.Find( h => (h.CubeCoordinates == a) );
	}

	public HexTile FindHex(Vector2 a) {
		//return HexList.Find( h => (h.GridCoordinates == a) );
	}

	public HexTile FindHex(HexTile a) {
		return HexList.Find( h => (h == a) );
	}

	public HexTile FindHex(GameObject a) {
		return HexList.Find( h => (h.HexObject == a) );
	}

	public void resetBoardHighlights() {
		foreach (HexTile i in HexList) {
			if(i.Passable) i.ColorHexTile(new Color(1,1,1,1));
		}
	}

	public int HeuristicHexDistance(HexTile a, HexTile b) {
		return (Math.Abs((int)a.CubeCoordinates.x - (int)b.CubeCoordinates.x) + Math.Abs((int)a.CubeCoordinates.y - (int)b.CubeCoordinates.y) + Math.Abs((int)a.CubeCoordinates.z - (int)b.CubeCoordinates.z)) / 2;
	}

	// A* Search based simple path
	public List<HexTile> SimplePath(HexTile start, HexTile goal, int steps = 3) {
		List<HexTile> Path = new List<HexTile>();
		Dictionary<HexTile, HexTile> CameFrom = new Dictionary<HexTile, HexTile>();
		Dictionary<HexTile, int> CostSoFar = new Dictionary<HexTile, int>();
		PriorityQueue<HexTile> frontier = new PriorityQueue<HexTile>();
		frontier.Enqueue(start, 0);
		
		CameFrom[start] = start;
		CostSoFar[start] = 0;
		
		for (int stepsTaken = 0; frontier.Count > 0 && stepsTaken < steps; stepsTaken++) {
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
