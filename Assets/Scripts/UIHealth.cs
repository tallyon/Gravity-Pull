using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Text healthText;

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        healthText.text = player.Health.ToString();
    }
}
