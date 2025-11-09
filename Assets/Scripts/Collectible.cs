using UnityEngine;

//This scipt is used by the Collectibles/Diamonds in the Game
public class Collectible : MonoBehaviour
{
    [SerializeField]
    public Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        
    }

    //This section outlines what happens when the player comes in contact with one of the Diamonds/Collectibles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If he entity that comes in contact with the Collectible is the Player then this code is triggered
        if(collision.name == "Player")
        {
            //The Diamond will be destroyed
            Destroy(gameObject);
            //The CollectOre code will be triggered in the Player script
            player.CollectOre();
        }
    }
}
