// using UnityEngine;
// using UnityEngine.EventSystems;  // Necesario para manejar eventos de puntero

// public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
// {
//     private Vector3 originalScale;  // Para almacenar la escala original de la carta
//     public float zoomFactor = 1.5f;  // Factor por el que se ampliará la carta
//     private bool isDragging = false; // Estado para controlar si la carta está siendo arrastrada
//     private bool isZoomed = false;   // Controla si el zoom está actualmente aplicado

//     void Start()
//     {
//         originalScale = transform.localScale;  // Guarda la escala original al iniciar
//     }

//     public void OnPointerEnter(PointerEventData eventData)
//     {
//         if (!isDragging) // Solo aplica zoom si la carta no está siendo arrastrada
//         {
//             transform.localScale = originalScale * zoomFactor;
//             isZoomed = true;
//         }
//     }

//     public void OnPointerExit(PointerEventData eventData)
//     {
//         // Restaura la escala original siempre, independientemente del arrastre
//         if (isZoomed)
//         {
//             transform.localScale = originalScale;
//             isZoomed = false;
//         }
//     }

//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         isDragging = true;  // Marca que la carta está siendo arrastrada
//         if (isZoomed)  // Si se está aplicando zoom, restaura la escala
//         {
//             transform.localScale = originalScale;
//             isZoomed = false;
//         }
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         // Aquí se podría añadir lógica adicional si se requiere durante el arrastre
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         isDragging = false;  // Marca que la carta ha dejado de ser arrastrada
//     }
// }
