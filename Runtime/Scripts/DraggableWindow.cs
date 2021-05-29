using UnityEngine;
using UnityEngine.EventSystems;

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#8-draggable-window")]
    public class DraggableWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isCursorOn = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isCursorOn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isCursorOn = false;
        }
    }
}