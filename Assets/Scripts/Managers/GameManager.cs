using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private GridManager gridManager;
    public Game game{get; private set;}
    public Solver solver{get; private set;}
    private static GameManager _instance;
    private bool playerInteraction = true;
    private bool pauseSolution = false;
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            Debug.Log("GameManager is null !!");
            return null;
        }
    }
    
    
    private void Awake()
    {
        _instance = this;
        gridManager = GridManager.Instance;
        InitializeGame(new Game(StaticGame.grid, StaticGame.MaxMoves, StaticGame.nbColor));
    }

    /// <summary>
    /// Initialize game of GameManager and load the game into the solver
    /// </summary>
    /// <param name="game">Game to load into the GameManager</param>
    public void InitializeGame(Game game){
        this.game = game;
        if(solver != null){
            solver.LoadGame(game);
        }else{
            solver = new Solver(game);
        }
    }


    private void Start()
    {
        InitUI();
        UpdateGridManager();
        StartCoroutine(CalculateSolution());
    }

    
    /// <summary>
    /// Initialize UI Elements
    /// </summary>
    public void InitUI()
    {
        UIManager.Instance.InitializeUI(game.MovesLeft(), game.ownedTerritory, game.grid.Length);
    }

    /// <summary>
    /// Update the Grid on screen with the correct colors
    /// </summary>
    public void UpdateGridManager()
    {
        for (int y = 0; y < game.height; y++)
        {
            for (int x = 0; x < game.width; x++)
            {
                Color color = ColorConverter.ToColor(game.GetColor(x, y));
                if (color != null)
                {
                    gridManager.ChangeColor(x, y, color);
                }
            }
        }
    }

   
    /// <summary>
    /// Change the game to a new instance and solver too
    /// </summary>
    /// <param name="game">Game that need to be loaded</param>
    public void SetGame(Game game){
        this.game = game;
        solver.LoadGame(game);
    }


    /// <summary>
    /// Play a move given a color, modify the game and then update the grid taking in consideration
    /// the new layout of the grid for the Game
    /// </summary>
    /// <param name="color">Color that you want to play</param>
    public void PlayMove(Color color)
    {
        if (playerInteraction && !game.IsGameOver()) // if player interaction is on and the game is not over
        {
            int c = ColorConverter.ToInt(color); // convert the color to an int
            game.PlayMove(c); // play the color (int)
            UpdateGridManager(); // update the grid on screen
            UpdateUI(); // update all UI Elements (score, moves lefts .. etc)
        }
        if(game.IsGameOver() && !game.IsWinning()){ // if the game is over and the player lost
            UIManager.Instance.GameOverScreen(); // displayer gameOver Screen
        }
    }

    /// <summary>
    /// Play a move given a color as an int, Mostly reserved for the solver
    /// Since that the player will never play with numbers but with colors
    /// 
    /// </summary>
    /// <param name="color">int color to play</param>
    public void PlayMove(int color)
    {
        game.PlayMove(color); // play the given color
        UpdateGridManager(); // update the grid manager
        UpdateUI(); // update UI elements
    }


    /// <summary>
    /// Search for a solution on another thread (Coroutine)
    /// Does not block the Game
    /// </summary>
    public void SearchForSolution(){
        StartCoroutine(CalculateSolution());
    }

    public bool CalculateSolutionMainThread(){
        solver.resetSolver();
        solver.SmartBacktracking(game);
        if(solver.foundSolution()){
            Debug.Log("Found a solution !");
            return true;
        }else{
            Debug.Log("Solution not found !");
            return false;
        }

    }

    

    /// <summary>
    /// Calculate a solution for the loaded game
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CalculateSolution(){
        solver.SmartBacktracking(game);
        yield return null;
    }



    /// <summary>
    /// Update all UI elements on screen
    /// </summary>
    public void UpdateUI()
    {
        UIManager.Instance.UpdateMoves(game.MovesLeft()); // number of moves
        UIManager.Instance.UpdateCells(game.ownedTerritory, game.grid.Length); // number of cells owned
        UIManager.Instance.ResetVisualHint(); // visual hint
    }


    
    /// <summary>
    /// Enable and disable player interaction with the game
    /// Last as long as it is not called again
    /// </summary>
    /// <param name="active">Bool true will enable interaction, false will disable</param>
    public void SetActivePlayerInteraction(bool active){
        playerInteraction = active;
        UIManager.Instance.SetActiveUI(active);
    }

    public void PlayAnimationSolution(){
        StartCoroutine(RoutineSolution());
    }


    /// <summary>
    /// Routine that play a solution
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RoutineSolution()
    {
        int[] moves = solver.Solution();
        for (int i = 0; i < moves.Length; i++)
        {
            yield return new WaitForEndOfFrame();
            PlayMove(moves[i]);
            yield return new WaitForSeconds(1f);

        }
        SetActivePlayerInteraction(true);
        yield return null;
    }
}
