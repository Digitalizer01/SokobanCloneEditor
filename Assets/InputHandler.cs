using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    public GameObject EditorManager;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        if(rayHit.collider.gameObject.tag == "SelectSprite")
        {
            EditorManager.GetComponent<EditorManager>().ChangeCurrentSprite(rayHit.collider.gameObject);
        }
        if(rayHit.collider.gameObject.tag == "EditorSprite")
        {
            EditorManager.GetComponent<EditorManager>().ChangeEditorSprite(rayHit.collider.gameObject);
        }
    }
}
