using System;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    public Action OnInputDrag;

    public Action OnInputPressed;

    public Action OnInputUnPressed;

    // When user press the screen
    private void OnMouseDown()
    {
        OnInputPressed();
    }

    // When user release the screen
    private void OnMouseUp()
    {
        OnInputUnPressed();
    }

    // While user dragging on the screen
    private void OnMouseDrag()
    {
        OnInputDrag();
    }
}
