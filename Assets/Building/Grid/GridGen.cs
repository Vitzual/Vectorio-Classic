using UnityEditor.UI;
using UnityEngine;

public class GridGen : MonoBehaviour
{
    private int Rows = 100;
    private int Cols = 100;
    private float GridSize = 5;
    public GameObject GridTile;


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        GameObject ReferenceTile = Instantiate(GridTile);

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                GameObject tile = (GameObject)Instantiate(ReferenceTile, transform);

                float posX = col * GridSize;
                float posY = row * -GridSize;

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(ReferenceTile);

        float GridW = Cols * GridSize;
        float GridH = Rows * GridSize;
        transform.position = new Vector2((-GridW / 2 + GridSize / 2) + 2.5f, (GridH / 2 - GridSize / 2) + 2.5f);
    }
}
