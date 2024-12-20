using UnityEngine;
using UnityEngine.Tilemaps;

// Classe che si occupa di gestire le stanze del gioco
public class RoomManager : MonoBehaviour
{
    // Array contenente i possibili premi che possono essere droppati
    [SerializeField] private GameObject[] rewardPrefab;
    [SerializeField] private bool isLobbyRoom=false;
    public Collider2D[] enemies { get; private set; }
    
    /// <summary>
    /// This variable is used to check if the room is active or not
    /// room is active when an enemy is inside the room
    /// only active rooms can drop items
    /// </summary>
    private bool isRoomActive;

    private TilemapCollider2D tilemapCollider;

    private float roomX;
    private float roomY;
    private Vector3 roomCenter;

    private DoorController[] doors;

    void Awake()
    {
        // Comprime i bordi dei tilemap per evitare che risultino più grandi di quanto siano realmente
        var tilemaps = transform.parent.GetComponentsInChildren<Tilemap>();
        foreach (var tilemap in tilemaps)
        {
            tilemap.CompressBounds();
        }

        tilemapCollider = transform.parent.GetComponentInChildren<TilemapCollider2D>();
    }

    void Start()
    {
        // Calcola le dimensioni della stanza ed il suo centro
        roomX=tilemapCollider.bounds.size.x;
        roomY=tilemapCollider.bounds.size.y;
        roomCenter = tilemapCollider.bounds.center;

        isRoomActive = false;

        // Trova tutti i nemici presenti nella stanza, se ce ne sono la stanza è attiva
        FindEnemies();
        if(enemies.Length>0) isRoomActive = true;

        // Trova tutte le porte presenti nella stanza e le apre o chiude in base alla stanza
        doors = transform.parent.GetComponentsInChildren<DoorController>();
        foreach (var door in doors)
        {
            if(isRoomActive)    door.CloseDoor();
            else                door.OpenDoor();
        }
    }






    
    // TODO: Da rimuovere in produzione
    // permette di eliminare tutti i nemici nella stanza con la barra spaziatrice, utile per testare cose
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            var hit = Physics2D.OverlapBoxAll(roomCenter, new Vector2(roomX, roomY), 0);
            
            foreach (var player in hit)
            {
                if(player.CompareTag("Player")){
                    foreach (var item in hit)
                    {
                        if(item.CompareTag("Enemy")){
                            if(item.name.Contains("Jerry"))  return;
                            else item.GetComponent<Enemy>().TakeDamage(100);
                        }
                    }

                    break;
                }
            }
        }
    }






    // Finchè la stanza è attiva controlla se ci sono nemici al suo interno, se non ce ne sono più la stanza è stata completata

    void LateUpdate()
    {
        if(isRoomActive){
            CheckRoom();
        }
    }

    void FindEnemies(){
        enemies = Physics2D.OverlapBoxAll(roomCenter, new Vector2(roomX, roomY), 0, LayerMask.GetMask("Enemy"));
    }

    void CheckRoom(){
        FindEnemies();
        if(enemies.Length==0) RoomCleared();
    }

    // Istanzia il premio e apre le porte della stanza
    void RoomCleared(){
        isRoomActive = false;
        if(!isLobbyRoom) Instantiate(rewardPrefab[Random.Range(0,rewardPrefab.Length)], roomCenter, Quaternion.identity);
        foreach (var door in doors)
        {
            door.OpenDoor();
        }
    }

    // Quando il giocatore entra nella stanza, si imposta come stanza corrente e si attivano i nemici
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            FindEnemies();
            other.GetComponent<PlayerController>().SetCurrentRoom(this);
            PlayerManager.Instance.currentRoom = this;

            if(isLobbyRoom) return;

            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().isActive = true;
            }
        }
    }

    // Quando il giocatore esce dalla stanza, si disattivano i nemici
    void OnTriggerExit2D(Collider2D other)
    {
        if(isLobbyRoom) return;
        
        if(other.CompareTag("Player")){
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().isActive = false;
            }
        }
    }
}
