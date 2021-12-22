using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AI_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();
            g.ReadFileInput(@"input.txt", true, out string start, out string end);
            //g.ReadFileInput(@"C:\Users\minhk\Desktop\input.txt", true, out string start, out string end);

            FileStream fs = new FileStream("output.txt", FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);

            g.HillClimbSearch(start, end, sWriter);
            //g.DFS(start, end, sWriter);
            //g.BestFirstSearch(start, end, sWriter);
            //g.HillClimbSearch(start, end, sWriter);

            sWriter.Flush();
            Console.WriteLine("Hoan thanh xuat file");
            sWriter.Close();

            Console.ReadKey();
        }
    }
}
