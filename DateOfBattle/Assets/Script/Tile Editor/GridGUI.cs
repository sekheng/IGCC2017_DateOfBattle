using UnityEngine;

/// <summary>
/// Used for visualizing how the grid will look like in the editor
/// </summary>
public class GridGUI : MonoBehaviour {
    [Header("The Grid values!")]
    [Tooltip("The individual grid width")]
    public float m_gridWidth = 64.0f;
    [Tooltip("The individual grid height")]
    public float m_gridHeight = 64.0f;

    [Tooltip("The map width in pixel")]
    public float m_mapWidth = 5000f;
    [Tooltip("The map height in pixel")]
    public float m_mapHeight = 5000f;

    [Tooltip("The tile prefab")]
    public Transform m_tilePrefab;

    [Tooltip("Pick a color for the grid outline")]
    public Color m_gridOutlineColor = Color.white;
	
	void OnDrawGizmos()
    {
        // We will need to make sure that the Gizmos can retain it's original color
        Color originalGizmosColor = Gizmos.color;
        Gizmos.color = m_gridOutlineColor;

        // The midpoint is (0,0,0)
        // We will loop from the bottom to top of the determined height and draw a line according to the grid height!
        for (float yValue = transform.position.y - (m_mapHeight * 0.5f); yValue <= transform.position.y + (m_mapHeight * 0.5f); yValue += m_gridHeight)
        {
            float lineGridYPos = Mathf.Floor(yValue / m_gridHeight) * m_gridHeight;
            Gizmos.DrawLine(
                new Vector3(-1000000f, lineGridYPos)
                , new Vector3(1000000f, lineGridYPos)
                );
        }
        // Loop from left to right of the determined width and draw a line according to the grid width
        for (float xValue = transform.position.x - (m_mapWidth * 0.5f); xValue <= transform.position.x + (m_mapWidth * 0.5f); xValue += m_gridWidth)
        {
            float lineGridXPos = Mathf.Floor(xValue / m_gridWidth) * m_gridWidth;
            Gizmos.DrawLine(
                new Vector3(lineGridXPos, -1000000)
                , new Vector3(lineGridXPos, 1000000)
                );
        }
        // Return the gizmos color
        Gizmos.color = originalGizmosColor;
    }
}
