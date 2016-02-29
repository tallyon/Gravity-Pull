using UnityEngine;
using UnityEngine.UI;

public class UITrailProgressSlider : MonoBehaviour
{
    [SerializeField]
    private TrailController trailController;
    [SerializeField]
    private Text lastTrailPoint, currentTrailPoint;
    private Slider slider;

    void Start()
    {
        if (trailController == null)
            trailController = FindObjectOfType<TrailController>();

        slider = GetComponent<Slider>();
        slider.interactable = false;
        // Subscribe to receive notifications when TrailPoint is achieved
        TrailController.CallOnTrailPointAchieved += TrailPointAchieved;
        // Setup slider max value based on the number of elements in trail list and update UI text
        slider.maxValue = trailController.TrailList.Count - 1;
        lastTrailPoint.text = slider.maxValue.ToString();
    }

    private void TrailPointAchieved(int current, int max)
    {
        slider.value = current;
        try
        {
            currentTrailPoint.text = slider.value.ToString();
        }
        catch (MissingReferenceException e)
        {
            Debug.LogError(e.Message + "; " + e.StackTrace + "; " + e.Source + "; " + e.InnerException);
            currentTrailPoint = transform.FindChild("CurrentTrailPointText").GetComponent<Text>();
            slider = GetComponent<Slider>();
        }

        if (slider.maxValue != max)
        {
            slider.maxValue = max;
            lastTrailPoint.text = slider.maxValue.ToString();
        }
    }
}
