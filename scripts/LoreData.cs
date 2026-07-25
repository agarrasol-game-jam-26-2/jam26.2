using UnityEngine;

[CreateAssetMenu(fileName = "LoreData", menuName = "Lore/LoreData")]
public class LoreData : ScriptableObject
{
    public string title;
    [TextArea(3, 10)]
    public string body;
}
