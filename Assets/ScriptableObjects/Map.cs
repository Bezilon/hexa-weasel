using UnityEngine;

[CreateAssetMenu]
public class Map : ScriptableObject {
    public Vector2 GridSize;
    public TerrainProperty[] TerrainProperties;
}
