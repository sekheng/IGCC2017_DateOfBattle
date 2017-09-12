using UnityEngine;
using UnityEngine.EventSystems;

public class BlockMouseRaycast : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerManager.Instance.hasUIBlockRaycast = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerManager.Instance.hasUIBlockRaycast = false;
    }
}
