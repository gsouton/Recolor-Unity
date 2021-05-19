using UnityEngine;

public static class RandomGameGenerator 
{
    private static int defaultWidth;
    private static int defaultHeight;
    private static int maxMoves;
    private static int nbColor;
    
    private static Solver solver;

    static RandomGameGenerator(){
        defaultWidth = 12;
        defaultHeight = 12;
        nbColor = 4;
        maxMoves = 14;
        solver = new Solver();
    }

    private static int[,] RandomGrid(){
        int[,] grid = new int[defaultHeight, defaultWidth];
        for(int y = 0; y < grid.GetLength(0); y++){
            for(int x = 0; x < grid.GetLength(1); x++){
                grid[y, x] = Random.Range(0, nbColor);
            }
        }
        return grid;
    }

    public static void SetParameters(int width = 12, int height = 12, int maxMoves = 12, int nbColor = 4){
        defaultWidth = width;
        defaultHeight = height;
        RandomGameGenerator.maxMoves = maxMoves;
        RandomGameGenerator.nbColor = nbColor;
    }

    public static Game RandomGame(){
        Game game = new Game(RandomGrid(), maxMoves, nbColor);
        game.SetMaxMoves(solver.FindAnySolution(game)-2);
        return game;
    }

    public static Game RandomGame(int maxMoves, int nbColor){
        return new Game(RandomGrid(), maxMoves, nbColor);
    }





}
