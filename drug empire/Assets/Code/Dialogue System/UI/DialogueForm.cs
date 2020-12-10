using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
   public class DialogueForm : MonoBehaviour
   {
      private GridLayoutGroup _gridLayoutGroup;

      private void Start()
      {
         _gridLayoutGroup = GetComponent<GridLayoutGroup>();
      }

      private void Update()
      {
         if (transform.childCount == 1)
            _gridLayoutGroup.cellSize = new Vector2(100f, 90f);
         else if (transform.childCount == 2)
            _gridLayoutGroup.cellSize = new Vector2(100f, 45f);
         
      }
      
   }
}