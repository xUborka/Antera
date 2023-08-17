using Mirror;
using UnityEngine;
using System.Collections;

public class GoblinScript : NetworkBehaviour
{
    [SyncVar] public int Health = 10;

    public ParticleSystem boom;
    public SpriteRenderer sr;
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

    public void Hurt(){
        Health -= 1;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink(){
        sr.color = new Color(1f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        sr.color = new Color(1f, 1f, 1f);
    }
}
