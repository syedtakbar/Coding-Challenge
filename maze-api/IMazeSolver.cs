using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace syedtakbar_maze_api
{
    public interface IMazeSolver
    {
        //Tuple<int, string> solveMaze(string [] mazeInput);
        string solveMaze(string [] mazeInput);
    }
}