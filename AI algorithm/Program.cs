using System;
using System.IO;
using System.Text;

namespace AI_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph2 g = new Graph2();
            g.ReadFileInput(@"input.txt", true, out string start, out string end);
            //g.ReadFileInput(@"C:\Users\minhk\Desktop\input.txt", true, out string start, out string end);

            FileStream fs = new FileStream("output.txt", FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);

            g.HillClimbSearch(start, end, sWriter);
            //g.DFS(start, end, sWriter);
            //g.BestFirstSearch(start, end, sWriter);
            //g.HillClimbSearch(start, end, sWriter);

            //g.ASearchAlgorithm(start, end, sWriter);

            sWriter.Flush();
            Console.WriteLine("Hoan thanh xuat file");
            sWriter.Close();

            Console.ReadKey();
        }
    }
}
