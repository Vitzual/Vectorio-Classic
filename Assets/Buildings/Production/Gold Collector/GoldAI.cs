using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAI : MonoBehaviour
{
    // Tile layer
    public LayerMask TileLayer;
    public static float Speed = 20f;

    // Contains all active coins in scene
    [System.Serializable]
    public class ActiveCoins
    {
        // Constructor method
        public ActiveCoins(Transform Object, Vector3 Location, ConveyorAI Target, ConveyorAI Previous, Vector3 Destination, int Amount, bool AtEntrance, bool IsStagnant)
        {
            this.Object = Object;
            this.Location = Location;
            this.Target = Target;
            this.Previous = Previous;
            this.Destination = Destination;
            this.Amount = Amount;
            this.AtEntrance = AtEntrance;
            this.IsStagnant = IsStagnant;
        }

        // Class variables
        public Transform Object { get; set; }
        public Vector3 Location { get; set; }
        public ConveyorAI Target { get; set; }
        public ConveyorAI Previous { get; set; }
        public Vector3 Destination { get; set; }
        public int Amount { get; set; }
        public bool AtEntrance { get; set; }
        public bool IsStagnant { get; set; }
    }
    public List<ActiveCoins> Coins;

    // Every frame, update position of all coins
    void FixedUpdate()
    {
        for (int i = 0; i < Coins.Count; i++)
        {
            try
            {
                if (Coins[i].IsStagnant)
                {
                    CheckInactiveCoin(i);
                    continue;
                }
                if (Coins[i].Object.position == Coins[i].Destination)
                    GetNewDestination(i);
                else 
                    Coins[i].Object.position = Vector2.MoveTowards(Coins[i].Object.position, Coins[i].Destination, Speed * Time.deltaTime);
            }
            catch
            {
                SetCoinInactive(i);
                i -= 1;
            }
        }
    }

    // Register a new coin under active class
    public void RegisterNewCoin(Transform Object, Vector3 Location, ConveyorAI Target, Vector3 Destination, int Amount)
    {
        Coins.Add(new ActiveCoins(Object, Location, Target, null, Destination, Amount, true, false));
    }

    // Sets a new target destination
    protected void GetNewDestination(int CoinID)
    {
        // Check here if at entrance or exit
        ConveyorAI ConveyorScript = Coins[CoinID].Target;

        if (Coins[CoinID].AtEntrance && !ConveyorScript.ExitOccupied)
        {
            Coins[CoinID].AtEntrance = false;
            Coins[CoinID].Destination = ConveyorScript.GetExitLocation();
            ConveyorScript.SetExitStatus(true);
            ConveyorScript.SetEntranceStatus(false);
        }
        else if (!Coins[CoinID].AtEntrance)
        {
            Vector2 RayLoc;
            switch (ConveyorScript.GetDirection())
            {
                case 1:
                    RayLoc = new Vector2(Coins[CoinID].Destination.x, Coins[CoinID].Destination.y + 5);
                    break;
                case 2:
                    RayLoc = new Vector2(Coins[CoinID].Destination.x + 5, Coins[CoinID].Destination.y);
                    break;
                case 3:
                    RayLoc = new Vector2(Coins[CoinID].Destination.x, Coins[CoinID].Destination.y - 5);
                    break;
                default:
                    RayLoc = new Vector2(Coins[CoinID].Destination.x - 5, Coins[CoinID].Destination.y);
                    break;
            }
            RaycastHit2D Target = Physics2D.Raycast(RayLoc, Vector2.zero, Mathf.Infinity, TileLayer);
            Coins[CoinID].Location = RayLoc;

            // Check target to see if it exists
            if (Target.transform != null && Target.transform.name == "Conveyor")
            {
                ConveyorAI Conveyor = Target.transform.GetComponent<ConveyorAI>();
                Coins[CoinID].Target = Conveyor;
                Coins[CoinID].Destination = Conveyor.GetEntranceLocation();
                if (!Target.transform.GetComponent<ConveyorAI>().EntranceOccupied)
                {
                    // If another valid conveyor has been found, move to it
                    ConveyorScript.SetExitStatus(false);
                    Coins[CoinID].AtEntrance = true;
                    Conveyor.SetEntranceStatus(true);
                    return;
                }
                Coins[CoinID].Previous = ConveyorScript;
                Coins[CoinID].IsStagnant = true;
                return;
            }
            Coins[CoinID].Target = null;
            Coins[CoinID].Previous = ConveyorScript;
            Coins[CoinID].IsStagnant = true;
        }
    }

    // Checks the inactive group
    protected void CheckInactiveCoin(int CoinID)
    {
        if (Coins[CoinID].Target == null)
        {
            RaycastHit2D Target = Physics2D.Raycast(Coins[CoinID].Location, Vector2.zero, Mathf.Infinity, TileLayer);
            if (Target.transform != null && Target.transform.name == "Conveyor")
            {
                ConveyorAI ConveyorScript = Target.transform.GetComponent<ConveyorAI>();
                Coins[CoinID].Target = ConveyorScript;
                if (ConveyorScript.GetEntranceLocation() != Vector3.zero)
                {
                    Coins[CoinID].AtEntrance = true;
                    Coins[CoinID].IsStagnant = false;
                    Coins[CoinID].Previous.SetExitStatus(false);
                    Coins[CoinID].Target.SetEntranceStatus(true);
                    Coins[CoinID].Destination = ConveyorScript.GetEntranceLocation();
                }
            }
        }
        else if (!Coins[CoinID].Target.IsEntranceOccupied())
        {
            if (Coins[CoinID].Target.GetEntranceLocation() != Vector3.zero)
            {
                Coins[CoinID].Destination = Coins[CoinID].Target.GetEntranceLocation();
                Coins[CoinID].AtEntrance = true;
                Coins[CoinID].IsStagnant = false;
                Coins[CoinID].Previous.SetExitStatus(false);
                Coins[CoinID].Target.SetEntranceStatus(true);
            }
        }
    }

    // Removes coins from the master class
    public void RemoveFloatingCoins(Vector3 position)
    {
        // Check active group
        Debug.Log("Attempting to remove floating coins");
        bool RemovedOne = false;
        for (int i = 0; i < Coins.Count; i++)
        {
            if (Vector3.Distance(Coins[i].Object.position, position) <= 3f)
            {
                Debug.Log("Removed coin at " + Coins[i].Object.position);
                SetCoinInactive(i);
                i -= 1;
                if (RemovedOne)
                    return;
                else
                    RemovedOne = true;
            }
        }
    }

    // Destroys a coin 
    protected void SetCoinInactive(int CoinID)
    {
        Transform Coin = Coins[CoinID].Object;
        Coins.RemoveAt(CoinID);
        Destroy(Coin.gameObject);
    }
}
