using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCommand : ICommand
{

    private GameObject _gameObject;
    private Color _color;
    private int _previousColor;
    public ClickCommand(GameObject gameObject, Color color){
        _gameObject = gameObject;
        _color = color;
        _previousColor = ColorConverter.ToInt(color);
    }
    public void Execute(){
        GameManager.Instance.PlayMove(_color);
        CommandManager.Instance.AddToBuffer(this);
    }

    public int GetLastColor(){
        return _previousColor;
    }

    public void Undo(){

    }


}
