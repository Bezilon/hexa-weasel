using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public delegate void ClickAction();
	public static event ClickAction OnClicked;

	private GridManager Grid;

	// Use this for initialization
	void Start () {
		Grid = GetComponent<GridManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			HexTile ClickedHex;
			HexTile PlayerPos;
			if (Physics.Raycast(ray, out hit)){
				//ClickedHex = Grid.FindHex(hit.transform.gameObject);
				ClickedHex = Grid.HexList.Find( h => (h.HexObject == hit.transform.gameObject) );
				PlayerPos = Grid.FindHex(GridManager.PlayerGridPosition);
				Grid.resetBoardHighlights();
				Grid.HighlightPath( Grid.SimplePath( PlayerPos, ClickedHex ) );
			}
		}
	}


}
