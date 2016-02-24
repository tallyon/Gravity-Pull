using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class UIFPSButton : MonoBehaviour
{
    private Button btn;
    private Stopwatch watch;
    private float thresholdMSOfSingleFrame = 1.0f / 30 * 1000;
    [SerializeField]
    private KeyCode hideKey;
    private bool activeCounter;
    private GameObject textField;

    void Start()
    {
        btn = GetComponent<Button>();
        textField = GetComponentInChildren<Text>().gameObject;
        activeCounter = true;

        // Set tooltip about which key to press to toggle FPS counter
        transform.GetChild(1).GetComponent<Text>().text = "Press " + hideKey.ToString() + " to toggle FPS counter";

        watch = new Stopwatch();
        watch.Start();
    }

    void Update()
    {
        // FPS measurement
        if (activeCounter)
        {
            watch.Stop();
            int msElapsed = (int)watch.ElapsedMilliseconds;
            int fpsCount = msElapsed > 0 ? 1000 / msElapsed : 0;
            if (msElapsed >= thresholdMSOfSingleFrame)
                btn.image.color = Color.red;
            else
                btn.image.color = Color.green;
            btn.GetComponentInChildren<Text>().text = msElapsed + "ms / " + fpsCount + " FPS";
            watch.Reset();
            watch.Start();
        }
        //****************************

        // Show/hide of counter check
        if (Input.GetKeyDown(hideKey))
        {
            if (activeCounter)
            {
                GetComponent<Image>().enabled = false;
                textField.SetActive(false);
                activeCounter = false;
            }
            else
            {
                GetComponent<Image>().enabled = true;
                textField.SetActive(true);
                activeCounter = true;
            }
        }
        //****************************
    }
}
