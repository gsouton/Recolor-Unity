using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Solver
{
    private int[] moves;

    public List<int[]> solutions = new List<int[]>();
    private Game game;
    public int nbSolutions { get; private set; }
    public int numberOfCombinations { get; private set; }
    private bool solutionFound = false;
    public StreamWriter sw;

    public Solver(Game game)
    {
        this.game = game;
        InitializeMoves(-1);
        numberOfCombinations = 0;
        nbSolutions = 0;
    }

    

    public Solver()
    {
        this.game = null;
        moves = null;
    }

    public void LoadGame(Game game)
    {
        this.game = game;
        InitializeMoves(-1);
        solutions.Clear();
        numberOfCombinations = 0;
        nbSolutions = 0;
        solutionFound = false;
    }

    private void InitializeMoves(int value)
    {
        moves = new int[game.maxMoves];
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i] = value;
        }
    }


    public void resetSolver()
    {
        solutionFound = false;
        InitializeMoves(-1);
        numberOfCombinations = 0;
        nbSolutions = 0;
    }

    public bool foundSolution()
    {
        return solutionFound;
    }


    public void BruteForce(Game game, int move = 0)
    {
        if (move >= game.maxMoves)
        {
            return;
        }
        Game tmp = game.Copy();
        for (int i = 0; i < game.nbColor; i++)
        {
            tmp.PlayMove(i);
            moves[move] = i;
            if (tmp.IsAllColored())
            {
                solutions.Add((int[])moves.Clone());
                nbSolutions++;
            }
            BruteForce(tmp, move + 1);
            tmp.ResetGame(game);
        }
    }


    public void SmartBacktracking(Game game, int move = 0)
    {
        if (move >= game.maxMoves)
        {
            return;
        }
        Game tmp = game.Copy();
        foreach (var neighbor in game.neighbors)
        {
            if (game.MovesLeft() > game.NbColorInGrid() - 2)
            {
                tmp.PlayMove(neighbor);
                moves[move] = neighbor;
                if (tmp.IsAllColored())
                {
                    solutions.Add((int[])moves.Clone());
                    ++nbSolutions;
                    solutionFound = true;
                    return;
                }
                SmartBacktracking(tmp, move + 1);
                tmp.ResetGame(game);
                if (solutionFound)
                {
                    return;
                }
            }
        }
    }

    public int FindAnySolution(Game game){
        int move = 0;
        int[] n = new int[game.nbColor];
        while(!game.IsAllColored()){ 
            game.neighbors.CopyTo(n);
            game.PlayMove(n[0]);
            move++;
        }
        game.Restart();
        return move;
    }




    public int[] Solution()
    {
        return solutions[0];
    }

    



}