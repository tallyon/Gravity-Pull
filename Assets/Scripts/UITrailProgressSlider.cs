using UnityEngine;
using UnityEngine.UI;

public class UITrailProgressSlider : MonoBehaviour
{
    [SerializeField]
    private TrailGeneration trailGeneration;
    [SerializeField]
    private Text lastTrailPoint, currentTrailPoint;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.interactable = false;
        // Subscribe to receive notifications when TrailPoint is achieved
        TrailGeneration.CallOnTrailPointAchieved += TrailPointAchieved;
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
