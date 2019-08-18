using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Text.Encodings.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace syedtakbar_maze_api
{
    [Route("api/[controller]/[action]")]
    public class MazeController : Controller
    {
        private readonly IMazeSolver _mazeSolver;
        public MazeController(IMazeSolver mazeSolver) {
            _mazeSolver = mazeSolver;
        }
        
        [HttpPost]
        public ActionResult testAPI([FromBody] string test)
        {
            //Console.WriteLine($"message received : {DateTime.Now} message content: {test}");            
            return Json(new {receivedText = test,  replyText = $"Thanks for your message! received at {DateTime.Now}"});
        }

        [HttpPost]
        //public Tuple<int, string>  solveMaze([FromBody] string [] mazeArrayInput)
         public string solveMaze([FromBody] string [] mazeArrayInput)
        {
            return  _mazeSolver.solveMaze(mazeArrayInput);
        } 
    }
}