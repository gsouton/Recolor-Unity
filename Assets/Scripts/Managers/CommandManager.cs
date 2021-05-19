using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    private static CommandManager _instance;
    public static CommandManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Command Manager is NULL !!");
                return null;
            }
            return _instance;
        }
    }

    private List<ICommand> commandBuffer = new List<ICommand>();
    private void Awake()
    {
        _instance = this;
    }

    public void AddToBuffer(ICommand command)
    {
        commandBuffer.Add(command);
    }

    public ICommand GetCommand(int index)
    {
        return commandBuffer[index];
    }

    public void ClearBuffer()
    {
        commandBuffer.Clear();
    }

    public void RestartGame()
    {
        GameManager.Instance.game.Restart(); // restart the game
        GameManager.Instance.UpdateGridManager(); // update the grid
        GameManager.Instance.InitUI(); // initialize ui
        ClearBuffer();
    }

    public void RandomGame()
    {
        GameManager.Instance.InitializeGame(RandomGameGenerator.RandomGame()); // load a new random game
        RestartGame(); // restart the game
        GameManager.Instance.SearchForSolution(); // calculate solution on a parrallel thread
    }

    public void ShowSolution()
    {
        GameManager.Instance.SetActivePlayerInteraction(false); // disable player Interaction
        RestartGame(); // restart the game
        GameManager.Instance.PlayAnimationSolution();
    }

    public bool IsSolutionValid()
    {
        int index = 0;
        foreach (ICommand command in commandBuffer)
        {
            if (GameManager.Instance.solver.Solution()[index] != ((ClickCommand)command).GetLastColor())
            {
                return false;
            }
            index++;
        }
        return true;

    }

    public void ShowHint(Image img)
    {
        Debug.Log("Calculating a solution !");
        if (GameManager.Instance.CalculateSolutionMainThread())
        {
            Color color = ColorConverter.ToColor(GameManager.Instance.solver.Solution()[GameManager.Instance.game.actualMove]);
            img.color = color;
            img.enabled = true;
        }

    }


}
