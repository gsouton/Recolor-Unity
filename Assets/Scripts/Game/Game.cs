using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Game
{
    /** Game properties **/
    public int[,] grid { get; private set; }

    private int[,] saveGrid;
    public int width { get; private set; }
    public int height { get; private set; }
    public int maxMoves { get;set; }
    public int nbColor { get; private set; }
    public int actualMove { get; private set; }

    /** Game extras properties **/
    public HashSet<int> setOfColors { get; private set; }

    public Dictionary<(int, int), bool> visitedNodes { get; private set; }

    public HashSet<int> neighbors { get; private set; }

    public HashSet<int> numberOfDifferentColors { get; private set; }

    public int ownedTerritory { get; private set; }


    public Game(int[,] grid, int maxMoves, int nbColor)
    {
        numberOfDifferentColors = new HashSet<int>();
        visitedNodes = new Dictionary<(int, int), bool>();
        setOfColors = new HashSet<int>();
        neighbors = new HashSet<int>();
        saveGrid = grid;
        this.grid = new int[grid.Length, grid.Length];
        this.grid = (int[,])grid.Clone();
        this.maxMoves = maxMoves;
        this.nbColor = nbColor;
        width = grid.GetLength(1);
        height = grid.GetLength(0);
        actualMove = 0;
        ownedTerritory = 0;
        InitalizeOwnedTerritory();
    }

    public Game(int[,] grid, int nbMaxMoves, int nbColor, int actualMove, HashSet<int> setOfColors, HashSet<int> numberOfDifferentColors, int owndeTerritory,
          Dictionary<(int, int), bool> visitedNodes, HashSet<int> neighbors)
    {
        this.grid = (int[,])grid.Clone();
        this.maxMoves = nbMaxMoves;
        this.nbColor = nbColor;
        width = grid.GetLength(1);
        height = grid.GetLength(0);
        this.actualMove = actualMove;
        this.ownedTerritory = owndeTerritory;
        this.setOfColors = new HashSet<int>(setOfColors);
        this.numberOfDifferentColors = new HashSet<int>(numberOfDifferentColors);
        this.visitedNodes = new Dictionary<(int, int), bool>(visitedNodes);
        this.neighbors = new HashSet<int>(neighbors);
        

    }

    public Game(string filename)
    {
        this.numberOfDifferentColors = new HashSet<int>();
        this.visitedNodes = new Dictionary<(int, int), bool>();
        this.neighbors = new HashSet<int>();
        this.setOfColors = new HashSet<int>();

        StreamReader sr = new StreamReader(filename); // open the file
        LoadGameFromFile(sr); // load the game from the file
        saveGrid = (int[,])grid.Clone(); // last grid of the game
        actualMove = 0;
        ownedTerritory = 0;

        InitalizeOwnedTerritory();
        sr.Close();
    }

    /// <summary>
    /// Load a Game from a file, set the private variables, width, height, maxMoves, nbColor and the grid
    /// </summary>
    /// <param name="sr">StreamReader to read from</param>
    private void LoadGameFromFile(StreamReader sr)
    {
        string line = sr.ReadLine(); // read a line
        string[] explodedLine = line.Split(' '); // explode the line
        width = int.Parse(explodedLine[(int)GameProperties.Width]); // get height width and maxMoves,
        height = int.Parse(explodedLine[(int)GameProperties.Height]);
        maxMoves = int.Parse(explodedLine[(int)GameProperties.MaxMoves]);
        grid = new int[height, width]; // create the grid
        for (int y = 0; y < height; y++)
        {
            line = sr.ReadLine();
            explodedLine = line.Split(' ');
            for (int x = 0; x < width; x++)
            {
                int color = int.Parse(explodedLine[x]);
                grid[y, x] = color;
                setOfColors.Add(color);
                numberOfDifferentColors.Add(color);
            }
        }
        nbColor = setOfColors.Count;
    }

    public void SetMaxMoves(int moves){
        maxMoves = moves;
    }

    /// <summary>
    /// Play a move for a given color, if the game is over won't play the move
    /// if you're trying to play the same color as top Left it won't play it
    /// </summary>
    /// <param name="color">int color requested to play</param>
    public void PlayMove(int color)
    {
        
        if (color != grid[0, 0])
        {
            clearNeighbors();
            actualMove++;
            ChangeColor(color, grid[0, 0]);

        }

    }

    /// <summary>
    /// Look for cell that are already owned, or all cells connected of the same colors following the rule of the game
    /// </summary>
    private void InitalizeOwnedTerritory()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                visitedNodes
    .Add((x, y), false);
            }
        }
        AnalyzeTerritory();
    }

    /// <summary>
    /// Go through the game and count the cell that are of the same colors and owned 
    /// </summary>
    /// <param name="x"> int x optional parameter position to start from </param>
    /// <param name="y"> int x optional parameter position to start from </param>
    /// <param name="lastColor"> lastColor optional parameter, to add if you want to exclude a color</param>
    private void AnalyzeTerritory(int x = 0, int y = 0, int lastColor = -1)
    {
        visitedNodes[(x, y)] = true;
        ownedTerritory++;
        if (x < width - 1)
        {
            int rightCell = GetColor(x + 1, y);
            if (rightCell == grid[0, 0] && !visitedNodes
[(x + 1, y)])
            {
                AnalyzeTerritory(x + 1, y, lastColor);
            }
            else if (rightCell != grid[0, 0])
            {
                neighbors.Add(rightCell);
            }
        }

        //check if down cell has the same color
        if (y < height - 1)
        {
            int downCell = GetColor(x, y + 1);
            if (downCell == grid[0, 0] && !visitedNodes
[(x, y + 1)])
            {
                AnalyzeTerritory(x, y + 1, lastColor);
            }
            else if (downCell != grid[0, 0])
            {
                neighbors.Add(downCell);
            }
        }

        //check if the left cell has the same color
        if (x > 0)
        {
            int leftCell = GetColor(x - 1, y);
            if (leftCell == grid[0, 0] && !visitedNodes
[(x - 1, y)])
            {
                AnalyzeTerritory(x - 1, y, lastColor);
            }
            else if (leftCell != grid[0, 0])
            {
                neighbors.Add(leftCell);
            }
        }

        //check if the upper cell has the same color
        if (y > 0)
        {
            int upperCell = GetColor(x, y - 1);
            if (upperCell == grid[0, 0] && !visitedNodes
[(x, y - 1)])
            {
                AnalyzeTerritory(x, y - 1, lastColor);
            }
            else if (upperCell != grid[0, 0])
            {
                neighbors.Add(upperCell);
            }
        }
    }

    /// <summary>
    /// Change the color for a game following the rule, Playing topLeft
    /// </summary>
    /// <param name="newColor"> The color you want to apply on a position </param>
    /// <param name="lastColor"> The last Color of the position x and y </param>
    /// <param name="x">Optional int, normally play topleft</param>
    /// <param name="y">Optional int, normally play topleft</param>
    private void ChangeColor(int newColor, int lastColor, int x = 0, int y = 0)
    {
        grid[y, x] = newColor;
        visitedNodes[(x, y)] = true;
        //check if the right cell has the same color
        if (x < grid.GetLength(1) - 1)
        {
            int rightCell = grid[y, x + 1];
            if (rightCell == lastColor)
            {
                ChangeColor(newColor, lastColor, x + 1, y);
            }
            else if (rightCell == newColor && !visitedNodes
[(x + 1, y)])
            {
                AnalyzeTerritory(x + 1, y, lastColor);
            }
            else if (rightCell != newColor)
            {
                neighbors.Add(rightCell);
            }
        }

        //check if down cell has the same color
        if (y < grid.GetLength(0) - 1)
        {
            int downCell = grid[y + 1, x];
            if (downCell == lastColor)
            {
                ChangeColor(newColor, lastColor, x, y + 1);
            }
            else if (downCell == newColor && !visitedNodes
[(x, y + 1)])
            {
                AnalyzeTerritory(x, y + 1, lastColor);
            }
            else if (downCell != newColor)
            {
                neighbors.Add(downCell);
            }
        }

        //check if the left cell has the same color
        if (x > 0)
        {
            int leftCell = grid[y, x - 1];
            if (leftCell == lastColor)
            {
                ChangeColor(newColor, lastColor, x - 1, y);
            }
            else if (leftCell == newColor && !visitedNodes
[(x - 1, y)])
            {
                AnalyzeTerritory(x - 1, y, lastColor);
            }
            else if (leftCell != newColor)
            {
                neighbors.Add(leftCell);
            }
        }

        //check if the upper cell has the same color
        if (y > 0)
        {
            int upperCell = grid[y - 1, x];
            if (upperCell == lastColor)
            {
                ChangeColor(newColor, lastColor, x, y - 1);
            }
            else if (upperCell == newColor && !visitedNodes
[(x, y - 1)])
            {
                AnalyzeTerritory(x, y - 1, lastColor);
            }
            else if (upperCell != newColor)
            {
                neighbors.Add(upperCell);
            }
        }
    }

    /// <summary>
    /// Clear the neighbors data structure
    /// </summary>
    public void clearNeighbors()
    {
        neighbors.Clear();
    }

    /// <summary>
    /// Check if a the game is over, Game is over if you number of moves == 0 
    /// </summary>
    /// <returns>Boolean</returns>
    public bool IsGameOver()
    {
        return actualMove >= maxMoves;
    }

    /// <summary>
    /// Get the color for a given position
    /// </summary>
    /// <param name="x">int x</param>
    /// <param name="y">int y</param>
    /// <returns>int corresponding to a color</returns>
    public int GetColor(int x, int y)
    {
        return grid[y, x];
    }

    /// <summary>
    /// Check if all the grid is colored
    /// </summary>
    /// <returns>Boolean</returns>
    public bool IsAllColored()
    {
        return ownedTerritory == grid.Length;
    }

    /// <summary>
    /// Check if the game is won or not, if false it doesn't mean that the game is over
    /// </summary>
    /// <returns>Boolean</returns>
    public bool IsWinning()
    {
        return IsAllColored() && actualMove <= maxMoves;
    }

    /// <summary>
    /// Reset the grid to the starting game
    /// </summary>
    private void ResetGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[y, x] = saveGrid[y, x];
            }
        }
    }

    /// <summary>
    /// Reset a game to a given game, the game will be set exactly to the given game state
    /// </summary>
    /// <param name="game">Game to copy the state from</param>
    public void ResetGame(Game game)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[y, x] = game.GetColor(x, y);
            }
        }

        actualMove = game.actualMove;
        ownedTerritory = game.ownedTerritory;
        visitedNodes = game.visitedNodes;
        visitedNodes = new Dictionary<(int, int), bool>(game.visitedNodes);
        neighbors = new HashSet<int>(game.neighbors);
    }

    /// <summary>
    /// Restart a game
    /// </summary>
    public void Restart()
    {
        actualMove = 0;
        ownedTerritory = 0;
        ResetGrid();
        visitedNodes.Clear();
        clearNeighbors();
        InitalizeOwnedTerritory();
    }

    /// <summary>
    /// Give you a copy of you game
    /// </summary>
    /// <returns></returns>
    public Game Copy()
    {
        Game g = new Game(grid, maxMoves, nbColor, actualMove, setOfColors, numberOfDifferentColors, ownedTerritory, visitedNodes, neighbors);
        return g;
    }

    /// <summary>
    /// Give you the number of moves left for a game
    /// </summary>
    /// <returns>int </returns>
    public int MovesLeft()
    {
        return maxMoves - actualMove;
    }

    /// <summary>
    /// Give you the number of different color in the game
    /// </summary>
    /// <returns> int </returns>
    public int NbColorInGrid()
    {
        List<int> colors = new List<int>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int color = GetColor(x, y);
                if (!colors.Contains(color))
                {
                    colors.Add(color);
                }
            }
        }
        return colors.Count();
    }

    /// <summary>
    /// Play a solution given as an array of colors
    /// </summary>
    /// <param name="solution"> int[] array of the solution</param>
    public void PlaySolution(int[] solution)
    {
        for (int i = 0; i < solution.Length; i++)
        {
            PlayMove(solution[i]);
        }
    }

    /// <summary>
    /// Check if the given color is a direct neighboor to the current ownedTerritory
    /// 
    /// </summary>
    /// <param name="color"> int color to check </param>
    /// <returns>Boolean </returns>
    public bool IsNeighboor(int color)
    {
        return neighbors.Contains(color);
    }

    /// <summary>
    /// Display the Neighbors in the console
    /// </summary>
    public void DisplayNeighbors()
    {
        foreach (var color in neighbors)
        {
            Console.Write(color + " ");
        }
    }

    
    public override string ToString()
    {
        return $"Game : nbColor = {nbColor}, maxMoves = {maxMoves}, actualMove = {actualMove}";
    }

    /// <summary>
    /// Display the grid in the console
    /// </summary>
    public void DisplayGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(GetColor(x, y) + " ");
            }

            Console.Write("\n");
        }
        Console.Write("\n");
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        // TODO: write your implementation of Equals() here
        Game g = (Game)obj;
        return maxMoves == g.maxMoves && nbColor == g.nbColor && saveGrid == g.saveGrid;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        //throw new System.NotImplementedException();
        return base.GetHashCode();
    }
    

}