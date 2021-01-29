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
        public ActiveCoins(Transform Object, ConveyorAI Target, Vector3 Destination, int Amount, bool AtEntrance)
        {
            this.Object = Object;
            this.Target = Target;
            this.Destination = Destination;
            this.Amount = Amount;
            this.AtEntrance = AtEntrance;
        }

        // Class variables
        public Transform Object { get; set; }
        public ConveyorAI Target { get; set; }
        public Vector3 Destination { get; set; }
        public int Amount { get; set; }
        public bool AtEntrance { get; set; }
    }
    public List<ActiveCoins> Coins;

    // Contains all inactive coins in scene
    [System.Serializable]
    public class InactiveCoins
    {
        // Constructor method
        public InactiveCoins(Transform Object, ConveyorAI Target, int Amount, Vector2 Location, ConveyorAI Previous)
        {
            this.Object = Object;
            this.Target = Target;
            this.Amount = Amount;
            this.Location = Location;
            this.Previous = Previous;
        }

        // Class variables
        public Vector2 Location { get; set; }
        public Transform Object { get; set; }
        public ConveyorAI Target { get; set; }
        public ConveyorAI Previous { get; set; }
        public int Amount { get; set; }
    }
    public List<InactiveCoins> StagnantCoins;

    // Every frame, update position of all coins
    void FixedUpdate()
    {
        CheckInactiveCoins();
        for (int i = 0; i < Coins.Count; i++)
        {
            try
            {
                if (Coins[i].Object.position == Coins[i].Destination)
                    i -= GetNewDestination(i);
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
    public void RegisterNewCoin(Transform Object, ConveyorAI Target, Vector3 Destination, int Amount)
    {
        Coins.Add(new ActiveCoins(Object, Target, Destination, Amount, true));
    }

    // Sets a new target destination
    protected int GetNewDestination(int CoinID)
    {
        // Check here if at entrance or exit
        ConveyorAI ConveyorScript = Coins[CoinID].Target;
        Debug.Log("Looking if next conveyor has an exit open");
        if (Coins[CoinID].AtEntrance && !ConveyorScript.ExitOccupied) // something
        {
            Coins[CoinID].AtEntrance = false;
            Coins[CoinID].Destination = ConveyorScript.GetExitLocation();
            ConveyorScript.SetExitStatus(true);
            ConveyorScript.SetEntranceStatus(false);
            return 0;
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

            // Check target to see if it exists
            if (Target.transform != null && Target.transform.name == "Conveyor")

                if (!Target.transform.GetComponent<ConveyorAI>().EntranceOccupied)
                {
                    // If another valid conveyor has been found, move to it
                    ConveyorScript.SetExitStatus(false);
                    Coins[CoinID].AtEntrance = true;
                    ConveyorAI Conveyor = Target.transform.GetComponent<ConveyorAI>();
                    Coins[CoinID].Target = Conveyor;
                    Coins[CoinID].Destination = Conveyor.GetEntranceLocation();
                    Conveyor.SetEntranceStatus(true);
                    return 0;
                }

            StagnantCoins.Add(new InactiveCoins(Coins[CoinID].Object, null, Coins[CoinID].Amount, RayLoc, Coins[CoinID].Target.GetComponent<ConveyorAI>()));
            Coins.RemoveAt(CoinID);
            return 1;
        }
        return 0;
    }

    // Checks the inactive group
    protected void CheckInactiveCoins()
    {
        for (int i = 0; i < StagnantCoins.Count; i++)
        {
            if (StagnantCoins[i].Target == null)
            {
                RaycastHit2D Target = Physics2D.Raycast(StagnantCoins[i].Location, Vector2.zero, Mathf.Infinity, TileLayer);
                if (Target.transform != null && Target.transform.name == "Conveyor")
                {
                    ConveyorAI ConveyorScript = Target.transform.GetComponent<ConveyorAI>();
                    if (!ConveyorScript.IsEntranceOccupied() && ConveyorScript.GetEntranceLocation() != Vector3.zero)
                    {
                        Coins.Add(new ActiveCoins(StagnantCoins[i].Object, ConveyorScript, ConveyorScript.GetEntranceLocation(), StagnantCoins[i].Amount, true));
                        StagnantCoins[i].Previous.SetExitStatus(false);
                        StagnantCoins.RemoveAt(i);
                        i -= 1;
                    }
                }
            }
            else if (!StagnantCoins[i].Target.IsEntranceOccupied())
            {
                Coins.Add(new ActiveCoins(StagnantCoins[i].Object, StagnantCoins[i].Target, StagnantCoins[i].Target.GetEntranceLocation(), StagnantCoins[i].Amount, true));
                StagnantCoins[i].Previous.SetExitStatus(false);
                StagnantCoins.RemoveAt(i);
                i -= 1;
            }
        }
    }

    // Removes coins from the master class
    public void RemoveFloatingCoins(Vector3 position)
    {
        // Check inactive group
        bool bonn_is_bad_and_dumb_and_uhg = false;
        for (int i=0; i < StagnantCoins.Count; i++)
        {
            if (Vector3.Distance(StagnantCoins[i].Object.position, position) <= 2.5f)
            {
                Debug.Log("Removed coin at " + StagnantCoins[i].Object.position);
                SetStagnantCoinInactive(i);
                bonn_is_bad_and_dumb_and_uhg = true;
                break;
            }
        }

        // Check active group
        for (int i = 0; i < Coins.Count; i++)
        {
            if (Vector3.Distance(Coins[i].Object.position, position) <= 2.5f)
            {
                Debug.Log("Removed coin at " + Coins[i].Object.position);
                SetCoinInactive(i);
                if (bonn_is_bad_and_dumb_and_uhg)
                    return;
                else bonn_is_bad_and_dumb_and_uhg = true;
            }
        }
    }

    // Moves an inactive coin
    protected void SetStagnantCoinInactive(int CoinID)
    {
        Transform Coin = StagnantCoins[CoinID].Object;
        StagnantCoins.RemoveAt(CoinID);
        Destroy(Coin.gameObject);
    }

    // Destroys a coin 
    protected void SetCoinInactive(int CoinID)
    {
        Transform Coin = Coins[CoinID].Object;
        Coins.RemoveAt(CoinID);
        Destroy(Coin.gameObject);
    }
}
