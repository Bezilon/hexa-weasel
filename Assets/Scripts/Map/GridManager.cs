using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public Map LevelMap;

	public float HexRadius;

	public Vector2 TestGoalGridPosition;

	// Default Tile Object
	public GameObject DefaultTile;

	public static HexTile PlayersTile;

	public Vector2 PlayerAxialPosition {
		get {
			return PlayersTile.AxialCoordinates;
		}
	}
	public Vector3 PlayerCubePosition {
		get {
			return PlayersTile.CubeCoordinates;
		}
	}

    private Vector2 GridSize { get { return LevelMap.GridSize; } }

    // Use this for initialization
    void Start() {
		CreateBasicGrid();
		HighlightPath( SimplePath( FindHex( PlayerAxialPosition ), FindHex( TestGoalGridPosition ) ) );
		HighlightPlayerPosition();
	}
	
	// Update is called once per frame
	void Update() {

		// Clicking on the board
		if (Input.GetMouseButtonDown(0)){
			Ray ray = GameObject.Find("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			HexTile ClickedHex;
			if (Physics.Raycast(ray, out hit)){
				//ClickedHex = Grid.FindHex(hit.transform.gameObject);
				ClickedHex = hit.transform.gameObject.GetComponent<HexTile>();
				
				resetBoardHighlights();
				HighlightPath( SimplePath( PlayersTile, ClickedHex ) );
			}
		}
		//Highlight Player position
	}

    public TerrainProperty FindProperty(HexTile tile)
    {
        return Array.Find(LevelMap.TerrainProperties, property => property.Type == tile.Type);
    }

    public bool HexInBounds(HexTile a) {
		return (a.AxialCoordinates.x >= 0.0F) && (a.AxialCoordinates.y >= ( 0.0F - ( GridSize.y / 2.0F ) + ( GridSize.y % 2 ) ) );
	}
	
	public IEnumerable<HexTile> Neighbors(HexTile a) {
		foreach (Vector3 dir in HexTile.Directions) {
			Vector3 NextHexCoords = new Vector3(a.CubeCoordinates.x + dir.x, a.CubeCoordinates.y + dir.y, a.CubeCoordinates.z + dir.z);
			HexTile NextHex = FindHex(NextHexCoords);
			if ( NextHex != null && HexInBounds(NextHex) && FindProperty(NextHex).Passable) {
				yield return NextHex;
			}
		}
	}

	private void CreateBasicGrid() {
		float YStep = Mathf.Sqrt( HexRadius*HexRadius - HexRadius*HexRadius/4 );
		float XStep = HexRadius - Mathf.Sqrt( HexRadius*HexRadius - YStep*YStep )/2;
		float OddRowPush = 0.0F;
		int GridCorrector = 0;
		Vector3 GlobalCoordinates = new Vector3(0.0F, 0.0F, 0.0F);
		GameObject CurrentTile;
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
                HexTile currentHex = CurrentTile.GetComponent<HexTile>();
                currentHex.AxialCoordinates = new Vector2(GridX, GridY);
                currentHex.Type = (TerrainType)Rand.Next(0, 5);
                TerrainProperty terrainProp = FindProperty(currentHex);
                CurrentTile.GetComponent<Renderer>().material.color = terrainProp.DebugColor;

				if(GridX == 0 && GridY == 0) PlayersTile = currentHex;				
			}

			if( GridX % 2 == 1 ) GridCorrector--;
		}
	}

	private void GenerateTerrain() {
		
	}

	private void LoadMap() {
		
	}

	public HexTile FindHex(Vector3 a) {
		foreach (Transform child in transform)
		{
			if(child.GetComponent<HexTile>().CubeCoordinates == a) return child.GetComponent<HexTile>();
		}
		return null;
	}

	public HexTile FindHex(Vector2 a) {
		foreach (Transform child in transform)
		{
			if(child.GetComponent<HexTile>().AxialCoordinates == a) return child.GetComponent<HexTile>();
		}
		return null;
	}

	public HexTile FindHex(HexTile a) {
		HexTile[] allHexes = gameObject.GetComponentsInChildren<HexTile>();
		foreach(HexTile currentHex in allHexes) {
			if(a == currentHex) return currentHex;
		}
		return null;
	}

	public HexTile FindHex(GameObject a) {
		return a.GetComponent<HexTile>();
	}

	public void resetBoardHighlights() {
		foreach (Transform child in transform)
		{
			if(Array.Find(LevelMap.TerrainProperties, property => property.Type == child.GetComponent<HexTile>().Type).Passable) child.GetComponent<Renderer>().material.color = (new Color(1,1,1,1));
			else child.GetComponent<Renderer>().material.color = new Color(0,0,0,1);
		}
	}

	public int HeuristicHexDistance(HexTile a, HexTile b) {
		return (Math.Abs((int)a.CubeCoordinates.x - (int)b.CubeCoordinates.x) + Math.Abs((int)a.CubeCoordinates.y - (int)b.CubeCoordinates.y) + Math.Abs((int)a.CubeCoordinates.z - (int)b.CubeCoordinates.z)) / 2;
	}

	// A* Search based simple path
	public List<HexTile> SimplePath(HexTile start, HexTile goal) {
		List<HexTile> path = new List<HexTile>();
		Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
		Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();
		PriorityQueue<HexTile> frontier = new PriorityQueue<HexTile>();
		frontier.Enqueue(start, 0);
		
		cameFrom[start] = start;
		costSoFar[start] = 0;
		
		while ( frontier.Count > 0 ) {
			HexTile current = frontier.Dequeue();
			
			if (current.Equals(goal)) break;
			
			foreach (HexTile next in Neighbors(current)) {
				int newCost = costSoFar[current] + Array.Find(LevelMap.TerrainProperties, property => property.Type == next.Type).Cost;
				if ( !costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
					costSoFar[next] = newCost;
					int priority = newCost + HeuristicHexDistance(next, goal);
					frontier.Enqueue(next, priority);
					cameFrom[next] = current;
				}
			}
		}

		HexTile RevCurrent = goal;
		path.Add(goal);
		path.Add(cameFrom[RevCurrent]);
		while (RevCurrent != start) {
			RevCurrent = cameFrom[RevCurrent];
			path.Add(RevCurrent);
		}
		path.Remove(start);
		path.Reverse();
		return path;
	}

	public void HighlightPlayerPosition() {
		//FindHex( new Vector2(PlayerAxialPosition.x, PlayerAxialPosition.y) ).ColorHexTile(new Color(1, 0, 0, 1));
	}

	public void HighlightPath(List<HexTile> Path) {
		float blue = 0.2F;
		float green = 1.0F;
		foreach (HexTile CurrentTile in Path) {
			//CurrentTile.ColorHexTile(new Color(0,green,blue,1));
			blue *= 1.5F;
			green /= 1.5F;
		}
	}
}
