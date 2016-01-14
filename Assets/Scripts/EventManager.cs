using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public delegate void ClickAction();
	public static event ClickAction OnClicked;

	private GridManager Grid;

	// Use this for initialization
	void Start () {
		//Grid = transform.Find("WorldManager").GetComponent<GridManager>();
	}
	
	// Update is called once per frame
	void Update () {

	}


}
