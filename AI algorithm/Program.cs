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

            FileStream fs = new FileStream("output_HillClimbSearch.txt", FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);
            g.HillClimbSearch(start, end, sWriter);
            sWriter.Flush();

            fs = new FileStream("output_BestFirstSearch.txt", FileMode.Create);
            sWriter = new StreamWriter(fs, Encoding.UTF8);
            g.BestFirstSearch(start, end, sWriter);
            sWriter.Flush();

            fs = new FileStream("output_DFS.txt", FileMode.Create);
            sWriter = new StreamWriter(fs, Encoding.UTF8);
            g.DFS(start, end, sWriter);
            sWriter.Flush();

            fs = new FileStream("output_BFS.txt", FileMode.Create);
            sWriter = new StreamWriter(fs, Encoding.UTF8);
            g.BFS(start, end, sWriter);
            sWriter.Flush();

            fs = new FileStream("output_ASearchAlgorithm.txt", FileMode.Create);
            sWriter = new StreamWriter(fs, Encoding.UTF8);
            g.ASearchAlgorithm(start, end, sWriter);
            sWriter.Flush();

            fs = new FileStream("output_BranchAndBound.txt", FileMode.Create);
            sWriter = new StreamWriter(fs, Encoding.UTF8);
            g.BranchAndBound(start, end, sWriter);

            sWriter.Flush();
            Console.WriteLine("Hoan thanh xuat file");
            sWriter.Close();

            Console.ReadKey();
        }
    }
}
