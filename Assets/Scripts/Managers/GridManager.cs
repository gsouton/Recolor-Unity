using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class GridManager : MonoBehaviour{
    public int lines, columns;
    public float topMargin = 0f;
    public float leftMargin = 0f;
    public float distanceBetweenTiles = .05f;
    public GameObject square;

    private LineRenderer line;

    public static GridManager Instance{
        get{
            if (_instance != null) return _instance;
            Debug.Log("Grid manager is null");
            return null;
        }
    }

    private float _offset;
    private GameObject[,] _cells;
    private Vector3 _center;
    private Vector3 _topLeftCorner;
    private Vector3 _topLeftGrid;
    private Vector3 _topRightGrid;
    private Vector3 _bottomLeftGrid;
    private Vector3 _bottomRightGrid;
    private static GridManager _instance;
    private static readonly int Flip = Animator.StringToHash("Flip");
    private UIManager uIManager;


    private void Awake(){
        _instance = this;
        
    }

    private void Start() {
        uIManager = UIManager.Instance;
        Vector2 canvasPos = Camera.main.ScreenToWorldPoint(uIManager.transform.position);
        Vector2 topLeftPanel = Camera.main.ScreenToWorldPoint(new Vector2(uIManager.Panel.rect.xMax, uIManager.Panel.rect.yMin));        
        //Debug.Log();
        InitalizeDimension();
        Generate();
        GetComponent<SpriteScaler>().Scale();
    }

    

    public Vector3 TopLeft(){
        return _topLeftGrid;
    }

    public Vector3 TopRight(){
        return _topRightGrid;
    }

    public Vector3 BottomRight(){
        return _bottomRightGrid;
    }

    public Vector3 BottomLeft(){
        return _bottomLeftGrid;
    }

    /*private void OnDrawGizmos() {
        Gizmos.matrix = m_CanvasTransform.localToWorldMatrix;
        Gizmos.DrawLine(m_Vector3From, m_Vector3To);
    }*/


   


    public void InitalizeDimension(){
        CalculateTopLeftRightPosition();
        float maxWidth = (Mathf.Abs(_center.x - (_topLeftCorner.x + leftMargin)) * 2f);
        float maxHeight = (Mathf.Abs(_center.x - (_topLeftCorner.y - topMargin)) * 2f);

        if (square.transform.localScale.x > maxWidth / columns || square.transform.localScale.y > maxHeight / lines){
            float min = Mathf.Min(maxWidth / columns, maxHeight / lines);
            Scale(square, min);
            
        }
        
        _offset = square.transform.localScale.x + distanceBetweenTiles;
        float c = columns / 2f;
        float l = lines / 2f;
        float sizeSquare = square.transform.localScale.x;
        float xPos = (_center.x - c * (sizeSquare + distanceBetweenTiles)) + sizeSquare / 2f + leftMargin;
        float yPos = (_center.y + l * (sizeSquare + distanceBetweenTiles)) - sizeSquare/2f;
        _topLeftGrid = new Vector3(xPos, yPos, 0);
        _topRightGrid = new Vector3(_topLeftGrid.x*-1, _topLeftGrid.y, 0f);
        _bottomLeftGrid = new Vector3(_topLeftGrid.x, _topLeftGrid.y * -1, 0f);
        _bottomRightGrid = new Vector3(_topRightGrid.x, _topRightGrid.y * -1, 0f);

    }

    public void CalculateTopLeftRightPosition(){
        _center = Camera.main.transform.position; // center of the screen
        _topLeftCorner =
            Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelHeight, 0f)); // top left corner 

        _topLeftCorner.x += square.transform.localScale.x / 2f; // top left where cube fit in screen
        _topLeftCorner.y -= square.transform.localScale.y / 2f;
        _topLeftCorner.z = 0f;

    }


    private void Scale(GameObject gameObject, float delta){
        gameObject.transform.localScale = new Vector3(delta, delta, delta);
    }

    private void Generate(){
        _cells = new GameObject[lines, columns];
        for (int y = 0; y < lines; y++){
            Vector3 pos = _topLeftGrid;
            pos.y -= _offset * y;
            for (int x = 0; x < columns; x++){
                _cells[y, x] = Instantiate(square, pos, Quaternion.identity);
                _cells[y, x].transform.SetParent(this.transform);
                
                pos.x += _offset;
            }
        }
        Scale(square, 1);
    }

    private void clearGrid(){
        for(int y = 0; y < _cells.GetLength(0); y++){
            for(int x = 0; x < _cells.GetLength(1); x++){
                DestroyImmediate(_cells[y,x]);
            }
        }
    }


    public void ChangeColor(int x, int y, Color color){
        if (!GetColor(x, y).Equals(color)){
            _cells[y, x].GetComponent<Animator>().SetTrigger(Flip); 
            SetColorCell(x, y, color);
        }
    }

    public void SetColorCell(int x, int y, Color color){
        _cells[y, x].GetComponent<SpriteRenderer>().color = color;
        //_cells[y, x].GetComponent<Renderer>().material.color = color;
    }

    public Color GetColor(int x, int y){
        return _cells[y, x].GetComponent<SpriteRenderer>().color;
    }
}