// using UnityEngine;

// public class HoverEffect : MonoBehaviour
// {
//     public Transform enlargedImagePrefab; // Prefab de la imagen ampliada
//     private GameObject enlargedInstance;

//     void OnMouseEnter()
//     {
//         if (enlargedInstance == null)
//         {
//             enlargedInstance = Instantiate(enlargedImagePrefab, new Vector3( /* posición donde quieres que aparezca */ ), Quaternion.identity);
//             enlargedInstance.transform.SetParent(enlargedImagePrefab);
//             enlargedInstance.transform.localScale = new Vector3(1, 1, 1); // Escala si es necesario
//         }
//     }

//     void OnMouseExit()
//     {
//         if (enlargedInstance != null)
//         {
//             Destroy(enlargedInstance);
//         }
//     }
// }
