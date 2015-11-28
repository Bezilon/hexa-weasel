using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	public int SizeX;
	public int SizeY;
	public float HexRadius;
	[HideInInspector]
	public HexInfo[,] HexArray;
	public GameObject DefaultTile;

	public Vector2 PlayerGridPosition;
	public Vector3 PlayerCubePosition {
		get {
			return new Vector3(PlayerGridPosition.x, PlayerGridPosition.y, -PlayerGridPosition.x-PlayerGridPosition.y);
		}
	}

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

	private void CreateBasicGrid() {
		float YStep = Mathf.Sqrt( HexRadius*HexRadius - HexRadius*HexRadius/4 );
		float XStep = HexRadius - Mathf.Sqrt( HexRadius*HexRadius - YStep*YStep )/2;
		float OddRowPush = 0.0F;
		float GlobalX = 0.0F;
		float GlobalY = 0.0F;
		GameObject CurrentTile;

		HexArray = new HexInfo[SizeX, SizeY];

		for(int GridX = 0; GridX < SizeX; GridX++) {
			OddRowPush = ( GridX % 2 == 1 ) ? ( YStep / 2 ) : 0;
			for(int GridY = 0; GridY < SizeY; GridY++) {
				try {
					GlobalX = (GridY*YStep)+OddRowPush;
					GlobalY = -GridX*XStep;
					CurrentTile = (GameObject)Instantiate(DefaultTile, new Vector3(GlobalX, 0, GlobalY), Quaternion.identity);
					CurrentTile.name = "[" + GridY + "," + GridX + "]";
					CurrentTile.transform.SetParent(this.transform);
					HexArray[GridY,GridX] = new HexInfo(CurrentTile, GlobalX, GlobalY, GridX, GridY, true, 1);
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

	public Vector3 GetGlobalsOfTile(int x, int y) {
		return HexArray[x,y].GlobalCoordinates;
	}

	public void HighlightPlayerPosition() {
		HexArray[(int)PlayerGridPosition.x,(int)PlayerGridPosition.y].ColorHexTile(new Color(1, 0, 0, 1));
	}
	
}
