using Engine;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent = null;
    public Vector3 startPosition;  
    public CardData cardData;  // Referencia a sus propios datos
    public BattleField currentBattleField;  // Referencia al campo de batalla donde actualmente se encuentra
    public bool dropSuccess = false;  // Nuevo estado para rastrear si el drop fue exitoso
    public bool isDraggable = true;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
           
        startPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(transform.root);  // Esto es para asegurar que no esté bloqueado por otros elementos UI.
        GetComponent<CanvasGroup>().blocksRaycasts = false;  // Esto asegura que el evento raycast no se bloquee después de comenzar a arrastrar
        Debug.Log("VALORES EN OnBeginDrag-------" + $"OnBeginDrag - startPosition: {startPosition}, originalParent: {originalParent.name}");
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        if (eventData.pressEventCamera != null)
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = eventData.pressEventCamera.nearClipPlane; // Asegúrate de que la posición Z es correcta.
            Vector3 newPos = eventData.pressEventCamera.ScreenToWorldPoint(screenPoint);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z); // Mantiene la posición z original.
        }
        else
        {
            // Fallback para Canvas en Screen Space - Overlay donde no hay cámara.
            transform.position = Input.mousePosition;
        }
    }
  
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        if (!dropSuccess)  // Si el drop no fue exitoso
        {
            Debug.Log("No se completó el drop correctamente. Regresando a la posición y padre originales.");
            Debug.Log($"Posicion actual: {transform.position} y startposition:  {startPosition}");
            Debug.Log($"originalParent:  {originalParent.name} padre actual: {transform.parent.name}" );
            transform.position = startPosition;
            transform.SetParent(originalParent);
            Debug.Log($"Posicion actual: {transform.position} y startposition:  {startPosition}");

        }
        dropSuccess = false;  // Resetea el estado para el próximo drag
        GetComponent<CanvasGroup>().blocksRaycasts = true;  // Reactiva el raycast
    }


}

