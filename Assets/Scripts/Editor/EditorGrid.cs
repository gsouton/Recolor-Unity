/*using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (GridManager))]
public class EditorGrid : Editor
{

    private void OnSceneGUI() {

        Canvas canvas = FindObjectOfType<Canvas>(); //Get reference of canvas


        // the top right position of canvas is the center of viewportPoint 
        // but (0,0) in viewPortPoint is bottomLeft
        Vector2 canvasViewPort = Camera.main.ScreenToViewportPoint(canvas.transform.position);

        // change the matrix for position to canavs 
        //Handles.matrix = canvas.transform.localToWorldMatrix;
        Handles.matrix = Camera.main.transform.localToWorldMatrix;
        Handles.color = Color.green; 
        
        //get grid manager reference
        GridManager grid = (GridManager) target;
        grid.InitalizeDimension(); // calculate the dimensions of the grid giving position world point of each corner

        /*Vector2 topLeft =  Camera.main.ViewportToScreenPoint( 
            (Vector2) Camera.main.WorldToViewportPoint(grid.TopLeft()) - canvasViewPort);
        Vector2 topRight = Camera.main.ViewportToScreenPoint(
            (Vector2) Camera.main.WorldToViewportPoint(grid.TopRight()) - canvasViewPort);
        Vector2 bottomLeft = Camera.main.ViewportToScreenPoint(
            (Vector2) Camera.main.WorldToViewportPoint(grid.BottomLeft()) - canvasViewPort);
        Vector2 bottomRight = Camera.main.ViewportToScreenPoint(
            ( Vector2) Camera.main.WorldToViewportPoint(grid.BottomRight()) - canvasViewPort);

        
        //Drawing
        Handles.DrawLine(topLeft, topRight);
        Handles.DrawLine(topLeft, bottomLeft);
        Handles.DrawLine(topRight, bottomRight);
        Handles.DrawLine(bottomLeft, bottomRight);*/
        /*float offset = grid.square.transform.localScale.x /2f;
        Vector2 topLeft = grid.TopLeft();
        Vector2 topRight = grid.TopRight();
        Vector2 bottomLeft = grid.BottomLeft();
        Vector2 bottomRight = grid.BottomRight();

        topLeft.x -= offset;
        topLeft.y += offset;

        topRight.x += offset;
        topRight.y += offset;

        bottomLeft.x -= offset;
        bottomLeft.y -= offset;

        bottomRight.x += offset;
        bottomRight.y -= offset;


        Handles.DrawLine(topLeft, topRight);
        Handles.DrawLine(topLeft, bottomLeft);
        Handles.DrawLine(topRight, bottomRight);
        Handles.DrawLine(bottomLeft, bottomRight);

        
    }
}*/
