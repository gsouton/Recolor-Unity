using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private void OnMouseDown() {
        ICommand clickCommand = new ClickCommand(this.gameObject, this.gameObject.GetComponent<SpriteRenderer>().color);
        clickCommand.Execute();  
    }


}
