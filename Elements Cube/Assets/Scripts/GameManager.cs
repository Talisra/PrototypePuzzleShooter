using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Board gameBoard;

    List<Block> selectedBlocks = new List<Block>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        gameBoard = FindObjectOfType<Board>();
    }

    public void PROTOTYPE_FLOODFILL(Block block)
    {
        gameBoard.FloodCheckBlocks(block);
    }

    public void TrySelectBlock(Block block)
    {
        if (selectedBlocks.Contains(block)) // Deselect already selected block
        {
            selectedBlocks.Remove(block);
            block.SetSelection(false);
        }
        else if (selectedBlocks.Count < 2) // Select block
        {
            selectedBlocks.Add(block);
            block.SetSelection(true);
            if (selectedBlocks.Count == 2) // if there are two blocks, try to switch them on board and clear their selections
            {
                gameBoard.SwitchBlocks(selectedBlocks[0], selectedBlocks[1]);
                foreach(Block actionBlock in selectedBlocks)
                {
                    actionBlock.SetSelection(false);
                }
                selectedBlocks.Clear();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
