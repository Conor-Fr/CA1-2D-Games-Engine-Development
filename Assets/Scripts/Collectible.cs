using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            Destroy(gameObject);
            player.CollectOre();
        }
    }
}
