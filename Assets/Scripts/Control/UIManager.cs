using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Synthesizer")]
    public GameObject CraftingUI;
    public RectTransform SynthButtonLayout;
    public GridLayoutGroup SynthLayoutGroup;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
