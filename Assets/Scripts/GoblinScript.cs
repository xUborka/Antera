using Mirror;
using UnityEngine;

public class GoblinScript : NetworkBehaviour
{
    [SyncVar] public int Health = 10;

    public ParticleSystem boom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0){
            ParticleSystem particle = Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(particle.gameObject, 1f);
        }
    }
}
