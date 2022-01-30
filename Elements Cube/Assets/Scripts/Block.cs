using UnityEngine;
public enum Element
{
    Earth,
    Water,
    Air,
    Fire,
    Plants,
    Ice,
    Thunder,
    Magma
}

public class Block : MonoBehaviour
{
    public float size = 2;
    private MeshRenderer mRenderer;
    private Color baseColor;
    private float hoverColorMultiplier = 1.5f;
    private float selectionColorMultiplier = 2;
    private bool isSelected;

    public bool checkedForFloodFill = false; // this bool is used for FloodFill algorithm ONLY
    
    private int colIdx;
    private int rowIdx;

    GameObject cube;
    public Element type;

    // Start is called before the first frame update
    void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();
        transform.localScale = new Vector3(size, size, size);
    }

    private void OnMouseEnter()
    {
        if (!isSelected)
        {
            mRenderer.material.color = baseColor * hoverColorMultiplier;
        }
    }

    private void OnMouseExit()
    {
        if (!isSelected)
        {
            mRenderer.material.color = baseColor;
        }
    }
    
    private void OnMouseDown()
    {
        //GameManager.Instance.TrySelectBlock(this);
        GameManager.Instance.PROTOTYPE_FLOODFILL(this);
    }

    public int GetMatrixCol()
    {
        return colIdx;
    }

    public int GetMatrixRow()
    {
        return rowIdx;
    }

    public void SetMatrixLocation(int col, int row)
    {
        colIdx = col;
        rowIdx = row;
    }

    public void SetSelection(bool selection)
    {
        isSelected = selection;
        if (selection)
        {
            mRenderer.material.color = baseColor * selectionColorMultiplier;
        }
        else
        {
            mRenderer.material.color = baseColor;
        }
    }

    public void SetElement(Element newType)
    {
        this.type = newType;
        switch (newType)
        {
            case Element.Earth:
                {
                    baseColor = new Color(0.5f, 0.25f, 0);
                    break;
                }
            case Element.Water:
                {
                    baseColor = Color.blue;
                    break;
                }
            case Element.Air:
                {
                    baseColor = Color.white;
                    break;
                }
            case Element.Fire:
                {
                    baseColor = new Color(1, 0.5f, 0);
                    break;
                }
            case Element.Plants:
                {
                    baseColor = Color.green;
                    break;
                }
            case Element.Ice:
                {
                    baseColor = Color.cyan;
                    break;
                }
            case Element.Thunder:
                {
                    baseColor = new Color(0.5f, 0.3f, 0.4f);
                    break;
                }
            case Element.Magma:
                {
                    baseColor = Color.red;
                    break;
                }
        }
        mRenderer.material.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
