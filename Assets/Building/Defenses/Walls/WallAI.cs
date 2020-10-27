using UnityEngine;

public class WallAI : TileClass
{

    // Wall auto place variables
    int top = 0;
    int right = 0;
    int bottom = 0;
    int left = 0;
    int total = 0;

    void Start()
    {
        health = 20;
        maxhp = 20;
    }

    ///// IMPORTANT! ///////////////////////////////////////////////////
    // Because Unity is seriously stupid af, you have to set the cost //
    // of the object here and not in the parent class. You also have  //
    // to change the name of the variable or else Unity dies          //
    ////////////////////////////////////////////////////////////////////

    // Cost & level variables
    private int WallCost = 5;
    private int WallLevel = 1;

    // How much the object costs
    public override int GetCost()
    {
        return WallCost;
    }

    // Default level of the object
    public override int GetLevel()
    {
        return WallLevel;
    }

    //////////////////////////////////////////////////////////////////

    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public int CheckTotal()
    {
        return top + right + bottom + left;
    }

    public void UpdateVertices(int a)
    {
        if (a == 4)
        {
            top = 1;
        }
        else if (a == 3)
        {
            right = 1;
        }
        else if (a == 2)
        {
            bottom = 1;
        }
        else if (a == 1)
        {
            left = 1;
        }
        else if (a == -1)
        {
            left = 0;
        }
        else if (a == -2)
        {
            bottom = 0;
        }
        else if (a == -3)
        {
            right = 0;
        }
        else if (a == -4)
        {
            top = 0;
        }
    }

    public void UpdateSprite(int a)
    {
        UpdateVertices(a);
        total = CheckTotal();
        SpriteRenderer SROBJ = gameObject.GetComponent<SpriteRenderer>();
        if (total == 1)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallDuo");
            if (bottom == 1)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }
            else if (right == 1)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
            } 
            else if (top == 1)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            }
        }
        else if (total == 2)
        {
            if (top == bottom || left == right)
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallCross");
                if (top == 1 && bottom == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
            }
            else
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallCorner");
                if (left == 1 && bottom == 1) 
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
                }
                else if (bottom == 1 && right == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                }
                else if (right == 1 && top == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                }
                else if (top == 1 && left == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
                }
            }
        }
        else if (total == 3)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallTrio");
            if (top == 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
            }
            else if (right == 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            }
            else if (bottom == 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180f));
            } 
            else
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }
        }
        else if (total == 4)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallQuad");
        }
    }
}
