using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game
{
    public class DraggableNote : MonoBehaviour
    {
        private Vector3 offset;
        private Camera mainCamera;
        public Transform targetObject;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void OnMouseDown()
        {
            offset = transform.position - GetMouseWorldPos();
        }

        private void OnMouseDrag()
        {
            if(transform.parent == null)
            transform.position = GetMouseWorldPos() + offset;
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mainCamera.nearClipPlane + 5f; 
            return mainCamera.ScreenToWorldPoint(mousePoint);
        }

        private void OnMouseUp()
        {
            if (Vector3.Distance(transform.position, targetObject.position) < 1f)
            {
                transform.SetParent(targetObject);
            }
        }

        private void Update()
        {
            if (transform.parent != null) 
            {
                transform.localPosition = Vector3.zero; 
                GetComponent<DraggableNote>().enabled = false; 
            }
        }
    }

}
