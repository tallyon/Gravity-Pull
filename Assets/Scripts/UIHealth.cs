using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    private PlayerController player;
    [SerializeField]
    private Text healthText;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        healthText.text = player.Health.ToString();
    }
}
