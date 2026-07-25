using UnityEngine;
using TMPro;

public class InteractionHistoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI historyText;
    [SerializeField] private int linesToDisplay = 10;
    [SerializeField] private bool showTimestamp = true;

    private InteractionHistory interactionHistory;

    void Start()
    {
        interactionHistory = FindObjectOfType<InteractionHistory>();
        if (interactionHistory == null)
            Debug.LogWarning("InteractionHistory não encontrado na cena!");
    }

    void Update()
    {
        if (interactionHistory == null) return;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (historyText == null) return;

        var history = interactionHistory.GetHistory();
        string text = "<b>HISTÓRICO DE INTERAÇÕES</b>\n";
        text += new string('─', 40) + "\n";

        int startIndex = Mathf.Max(0, history.Count - linesToDisplay);

        for (int i = startIndex; i < history.Count; i++)
        {
            var record = history[i];
            string line = $"• {record.objectName}";

            if (!string.IsNullOrEmpty(record.animationTriggered))
                line += $" → [{record.animationTriggered}]";

            if (showTimestamp)
                line += $" ({record.timestamp:F1}s)";

            text += line + "\n";
        }

        text += new string('─', 40) + "\n";
        text += $"Total: {history.Count} interações";

        historyText.text = text;
    }
}
