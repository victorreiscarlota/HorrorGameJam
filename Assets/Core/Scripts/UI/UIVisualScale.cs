using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Scripts.UI
{
    public class UIVisualScale: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float scaleModifier = 1.15f;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * scaleModifier;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
        }
    }
}