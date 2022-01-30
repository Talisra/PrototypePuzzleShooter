using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int cols = 4;
    public int rows = 8;
    private float margin = 0.15f;

    const float MARGIN_FROM_BLOCK = 0.075f; // 0.075

    Vector3 startPosition = Vector3.zero;
    public Block blockPrefab;

    private Block[,] blockMatrix;

    // Start is called before the first frame update

    void Awake()
    {
        blockMatrix = new Block[cols, rows];
        InitiateBoard();
    }

    private void Start()
    {

    }

    public void InitiateBoard()
    {
        margin = blockPrefab.size * MARGIN_FROM_BLOCK;
        float step = blockPrefab.size + margin;
        float offset_x = -(step * cols / 2) + ((cols % 2 == 0) ? step / 2 : 0);
        float offset_y = -(step * rows / 2) + ((rows % 2 == 0) ? step / 2 : 0);
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Block block = Instantiate(blockPrefab, startPosition + new Vector3(offset_x + step * i, offset_y + step * j, 0), Quaternion.identity) as Block;
                block.SetElement((Element)Random.Range(0, 8));
                block.SetMatrixLocation(i, j);
                block.transform.SetParent(transform);
                block.gameObject.name = "Block [" + i + "," + j + "]";
                blockMatrix[i, j] = block;
            }
        }
    }

    public Block GetBlock(int row, int col)
    {
        return blockMatrix[col,row];
    }

    public bool SwitchBlocks(Block block1, Block block2) // try to switch between blocks, the switch will not be done if not possible
    // returns the result of the switch for outside use
    {
        float distance = Vector3.Distance(block1.transform.position, block2.transform.position);
        if (distance > blockPrefab.size*1.1f + margin)
        {
            return false;
        }
        // Switch blocks: transform, logical location at matrix.
        Vector3 tempLocation = block2.transform.position;
        // switch block 1
        blockMatrix[block1.GetMatrixCol(), block1.GetMatrixRow()] = block2;
        block2.SetMatrixLocation(block1.GetMatrixCol(), block1.GetMatrixRow());
        block2.transform.position = block1.transform.position;
        // switch block2
        blockMatrix[block2.GetMatrixCol(), block2.GetMatrixRow()] = block1;
        block1.SetMatrixLocation(block2.GetMatrixCol(), block2.GetMatrixRow());
        block1.transform.position = tempLocation;
        return true;
    }

    public List<Block> FloodCheckBlocks(Block block) // returns a list of all the blocks with the same color, using Flood Fill
    {
        List<Block> sameColorBlocks = new List<Block>();
        Debug.Log("start for block " + block.GetMatrixCol() + " , " + block.GetMatrixRow() + " element: " + block.type);
        sameColorBlocks.AddRange(FloodCheckUtil(block, block.type));
        Debug.Log("final count: " + sameColorBlocks.Count);
        return sameColorBlocks;
    }

    private List<Block> FloodCheckUtil(Block block, Element type)
    {
        Debug.Log("check for block " + block.GetMatrixCol() + ", " + block.GetMatrixRow() + "| "+ type.ToString() + " / " + block.type.ToString());
        List<Block> tempList = new List<Block>();
        int blockCol = block.GetMatrixCol();
        int blockRow = block.GetMatrixRow();

        if (blockCol < 0 || blockCol >=cols || blockRow < 0 || blockRow >= rows)
        {
            // do nothing
            Debug.Log("out from recur: already checked");
        }
        if (block.checkedForFloodFill)
        {
            block.checkedForFloodFill = true;
            Debug.Log("out from recur: already checked");

            // do nothing
        }
        if (block.type == type && !block.checkedForFloodFill)
        {
            // add block to list
            tempList.Add(block);
            block.checkedForFloodFill = true;
            Debug.Log("Found Color, keep searching...");
            // recur for right left top bot
            tempList.AddRange(FloodCheckUtil(blockMatrix[blockCol + 1, blockRow], block.type));
            tempList.AddRange(FloodCheckUtil(blockMatrix[blockCol - 1, blockRow], block.type));
            tempList.AddRange(FloodCheckUtil(blockMatrix[blockCol, blockRow + 1], block.type));
            tempList.AddRange(FloodCheckUtil(blockMatrix[blockCol, blockRow - 1], block.type));
        }
        else if (!block.checkedForFloodFill)
        {
            block.checkedForFloodFill = true;
        }
        Debug.Log(tempList.Count);
        return tempList;
    }

    private void PrintMatrixDebug()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Debug.Log(i + ", " + j + ": " + blockMatrix[i, j].type + " |location: " + blockMatrix[i, j].GetMatrixCol() + ", " + blockMatrix[i, j].GetMatrixRow());
            }
        }
    }

    void Update()
    {
        
    }
}
