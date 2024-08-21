using UnityEngine;
using UnityEngine.UI;

public class SyncScroll : MonoBehaviour
{
    public ScrollRect textScrollRect;
    public ScrollRect lineNumberScrollRect;
    public Scrollbar verticalScrollbar;

    void Update()
    {
        // Sincronizar la posición vertical
        if (lineNumberScrollRect != null && textScrollRect != null)
        {
            // Debug.Log("lineNumberScrollRect: "+lineNumberScrollRect.verticalNormalizedPosition);
            Debug.Log("textScrollRect: "+verticalScrollbar.value);
            // lineNumberScrollRect.verticalNormalizedPosition = textScrollRect.verticalNormalizedPosition;
        }
    }
}
