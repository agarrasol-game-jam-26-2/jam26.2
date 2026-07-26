using UnityEngine;

public class OneShotNarration : MonoBehaviour
{
    [SerializeField] private LoreData narrationLore;

    void Start()
    {
        if (narrationLore != null)
            LoreDisplayUI.Instance.Show(narrationLore);
    }
}