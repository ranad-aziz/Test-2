using UnityEngine;

public class item : MonoBehaviour
{
    public GameObject besket;
    public GameObject Particle;
    bool canPickup = false;

    // Update is called once per frame
    void Update()
    {
      if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            if (besket != null)
            {
                besket.SetActive(false);
                Particle.SetActive(false);
            }
}  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) canPickup = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) canPickup = false;
    }
}