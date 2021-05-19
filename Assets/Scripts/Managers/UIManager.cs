using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   private static UIManager _instance;
   public static UIManager Instance{
       get{
           if(_instance == null){
               Debug.Log("UIManager is null");
               return null;
           }else{
               return _instance;
           }
       }
    }

    private float max;
    private Selectable[] buttons;

    public Selectable hintbutton;

    [Header("Canvas")]
    public Canvas backGroundCanvas;
    public Canvas forGroundCanvas;
    
    [Header("Text Elements")]
    public TextMeshProUGUI moves, ownedCells;

    [Header("Images")]
    public Image loadBar;
    public Image visualHint;

    [Header("Other")]
    public RectTransform Panel;
    [Header("Serialized Field")]
    [SerializeField]
    private float fillingSpeed = 0.1f;


    private void Awake() {
        _instance = this;
    }

    private void Start() {
        Initialize();
    }

    private void Initialize(){
        buttons = backGroundCanvas.GetComponentsInChildren<Selectable>();   
    }

    public void SetActiveForGroundCanvas(bool active){
        forGroundCanvas.enabled = active;
    }

    private void InitializeHintButton(){
        foreach(Selectable button in buttons){
            if(button.name == "HintButton"){
                hintbutton = button;
            }
        }
    }

    public void InitializeUI(int move, int cellsOwned, int max){
        this.max = max; // get maximum of cells in the game
        moves.text = ""+ move; // number of moves
        ownedCells.text = cellsOwned + "/" + max; // number of cells owned over the number of cells
        loadBar.fillAmount = (float) cellsOwned/ (float) max; // fill the loading bar for the amount of cells owned
        SetActiveForGroundCanvas(false);  // disable the foreground canvas (GameOver screen)
        ResetVisualHint(); 
    }

    public void ResetVisualHint(){
        visualHint.color = Color.white;
        visualHint.enabled = false;
    }

    public void SetActiveUI(bool active){
        foreach(Button button in buttons){
            button.enabled = active;
        }
    }

    public void GameOverScreen(){
        SetActiveForGroundCanvas(true);
    }


    public void UpdateMoves(int move){
        moves.text = ""+move;
    }

    public void UpdateCells(int cellsOwned, int max){
        ownedCells.text = cellsOwned + "/" + max;
    }

    private void Update(){
        float ownedCells = (float) GameManager.Instance.game.ownedTerritory;
        float bound = ownedCells/max;
        if(bound > 1f){
            bound = 1f;
        }
        if(loadBar.fillAmount < ownedCells/max){
            loadBar.fillAmount += fillingSpeed * Time.deltaTime;
        }
    }

    
}
