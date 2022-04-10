using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ArrestBarScript : MonoBehaviour
{
    public Image barImg;
    public GameObject[] uiElements;

    [SerializeField, Range(0, 1)]
    float barProgress;

    public bool isVisible;

    public bool IsBarVisible {
        get {
            return this.isVisible;
        }
        set
        {
            isVisible = value;
        
        }  
    }

    private void Awake()
    {
        isVisible = false;
    }

    private void Update()
    {
        barImg.fillAmount = barProgress;
    }

    void EnableBar(bool value)
    {
        foreach (var gm in uiElements)
        {
            gm.SetActive(value);
        }
        isVisible = value;
    }


}
