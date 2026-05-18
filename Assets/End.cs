using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject basket; 
    public GameObject winUI;  

    private bool playerAtEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (playerAtEnd && !basket.activeSelf)
        {
            winUI.SetActive(true); 
            Time.timeScale = 0f;  
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerAtEnd = true;
        }
    }

}