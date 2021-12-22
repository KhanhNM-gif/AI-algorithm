using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_algorithm
{
    class Graph
    {
        Dictionary<string, LinkedList<string>> _adj;
        Dictionary<string, int> w;

        public Graph()
        {
            _adj = new Dictionary<string, LinkedList<string>>();
            w = new Dictionary<string, int>();
        }

        public void AddEdge(string v, string w)
        {
            if (_adj.TryGetValue(v, out LinkedList<string> linkList))
            {
                _adj[v].AddLast(w);
            }
            else
            {
                LinkedList<string> linklistnew = new LinkedList<string>();
                linklistnew.AddLast(w);
                _adj.Add(v, linklistnew);
            }
        }
        public void AddWeight(string v, int w)
        {
            this.w.Add(v, w);
        }

        public void PrintWay(List<Tuple<int, int>> l, Tuple<int, int> t, int s, int k)
        {
            if (s != t.Item1) PrintWay(l, l.Where(x => x.Item2 == t.Item1).FirstOrDefault(), s, k);
            Console.Write($" {t.Item1 } -> {t.Item2 }\n");
        }

        public void ReadFileInput(string filepath, bool hasWeight, out string start, out string end)
        {
            start = null; end = null; 
            foreach (var s in File.ReadLines(filepath))
            {
                string[] str = s.Trim().Split(' ');
                if (str.Count() > 0)
                {
                    if (start == null && end == null)
                    {
                        start = str[0];
                        end = str[1];
                        //if (str.Count() > 2) limit = int.Parse(str[2]);
                    }
                    else
                    {
                        for (int i = hasWeight ? 2 : 1; i < str.Count(); i++)
                            AddEdge(str[0], str[i]);

                        if (hasWeight) AddWeight(str[0], int.Parse(str[1]));
                    }
                }
            }
        }

        public string PrintWayToFile(List<Tuple<string, string>> l, Tuple<string, string> t, string s, string k)
        {
            if (s != t.Item1) return PrintWayToFile(l, l.Where(x => x.Item2 == t.Item1).FirstOrDefault(), s, k) + $" -> {t.Item2 }";
            else return $" {t.Item1 } -> {t.Item2 }";
        }

        public void PrivtMark(List<Tuple<int, int>> l)
        {
            foreach (var item in l)
                Console.Write($" {item.Item1 } -> {item.Item2 }\n");
        }
        public void BFS(string s, string e, StreamWriter streamWriter)
        {
            string s1 = s;


            Queue<Tuple<string, string>> queue = new Queue<Tuple<string, string>>();
            List<Tuple<string, string>> t = new List<Tuple<string, string>>();

            string Format = "| {0} | {1} | {2} |";
            streamWriter.WriteLine(String.Format(Format, "Đỉnh".CenterString(6), "Đỉnh Kề".CenterString(15), "Hàng Đợi".CenterString(15)));

            queue.Enqueue(new Tuple<string, string>(s, s));

            while (queue.Count > 0)
            {
                Tuple<string, string> tuple = queue.Dequeue();
                t.Add(tuple);

                var c = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format(Format, s.CenterString(6), "TTKT-DỪNG".CenterString(15), "".CenterString(15)));
                    streamWriter.Write($"\nĐường đi:{PrintWayToFile(t, tuple, s1, e)}");
                    return;
                }

                if (_adj.TryGetValue(s, out LinkedList<string> list))
                {
                    foreach (var val in list)
                        queue.Enqueue(new Tuple<string, string>(s, val));
                }

                streamWriter.WriteLine(String.Format(Format, s.CenterString(6), (list != null ? string.Join(",", list) : "").CenterString(15), string.Join(",", queue.Select(x => x.Item2).ToArray()).CenterString(15)));
            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }

        public void DFS(string s, string e, StreamWriter streamWriter)
        {
            string s1 = s;
            Stack<Tuple<string, string>> stack = new Stack<Tuple<string, string>>();

            List<Tuple<string, string>> t = new List<Tuple<string, string>>();

            string Format = "| {0} | {1} | {2} |";
            streamWriter.WriteLine(String.Format(Format, "Đỉnh".CenterString(6), "Đỉnh Kề".CenterString(15), "Stack".CenterString(15)));

            stack.Push(new Tuple<string, string>(s, s));

            while (stack.Count > 0)
            {
                Tuple<string, string> tuple = stack.Pop();
                t.Add(tuple);

                s = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format(Format, s.CenterString(6), "TTKT-DỪNG".CenterString(15), "".CenterString(15)));
                    streamWriter.Write($"\nĐường đi:{PrintWayToFile(t, tuple, s1, e)}");
                    return;
                }

                if (_adj.TryGetValue(s, out LinkedList<string> list))
                {
                    foreach (var val in list)
                        stack.Push(new Tuple<string, string>(s, val));
                }
                streamWriter.WriteLine(String.Format(Format, s.CenterString(6), (list != null ? string.Join(",", list) : "").CenterString(15), string.Join(",", stack.Select(x => x.Item2).ToArray()).CenterString(15)));
            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }

        /*public void DLS(int s, int e, int limit)
        {
            int s1 = s;
            Stack<Tuple<int, int, int>> stack = new Stack<Tuple<int, int, int>>();
            List<Tuple<int, int>> t = new List<Tuple<int, int>>();

            stack.Push(new Tuple<int, int, int>(s, s, 1));
            int dept;

            while (stack.Count > 0)
            {
                Tuple<int, int, int> tuple = stack.Pop();
                t.Add(new Tuple<int, int>(tuple.Item1, tuple.Item2));

                dept = tuple.Item3;
                s = tuple.Item2;

                if (s == e)
                {
                    Console.WriteLine("DLS\nQua trinh di chuyen");
                    PrivtMark(t);
                    Console.WriteLine("Duong di tim thay");
                    PrintWay(t, new Tuple<int, int>(tuple.Item1, tuple.Item2), s1, e);
                    return;
                }

                LinkedList<int> list = _adj[s];
                if (dept < limit)
                    foreach (var val in list)
                        stack.Push(new Tuple<int, int, int>(s, val, dept + 1));

            }
        }*/

        public void BestFirstSearch(string s, string e, StreamWriter streamWriter)
        {
            string s1 = s;
            LinkedList<Tuple<string, string>> queue = new LinkedList<Tuple<string, string>>();

            string Format = "| {0} | {1} | {2} |";
            streamWriter.WriteLine(String.Format(Format, "Đỉnh".CenterString(6), "Đỉnh Kề".CenterString(15), "Hàng Đợi".CenterString(15)));

            List<Tuple<string, string>> t = new List<Tuple<string, string>>();

            queue.AddLast(new Tuple<string, string>(s, s));

            while (queue.Count > 0)
            {
                Tuple<string, string> tuple = queue.First();
                queue.RemoveFirst();
                t.Add(tuple);

                s = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format(Format, s.CenterString(6), "TTKT-DỪNG".CenterString(15), "".CenterString(15)));
                    streamWriter.Write($"\nĐường đi:{PrintWayToFile(t, tuple, s1, e)}");
                    return;
                }

                if (_adj.TryGetValue(s, out LinkedList<string> list))
                {
                    foreach (var val in list)
                        queue.AddLast(new Tuple<string, string>(s, val));
                }

                queue = new LinkedList<Tuple<string, string>>(queue.OrderBy(x => w[x.Item2]).ToArray());
                streamWriter.WriteLine(String.Format(Format, s.CenterString(6), (list != null ? string.Join(",", list) : "").CenterString(15), string.Join(",", queue.Select(x => x.Item2).ToArray()).CenterString(15)));
            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }


        public void HillClimbSearch(string s, string e, StreamWriter streamWriter)
        {
            string s1 = s;
            LinkedList<Tuple<string, string>> queue = new LinkedList<Tuple<string, string>>();

            List<Tuple<string, string>> t = new List<Tuple<string, string>>();

            string Format = "| {0} | {1} | {2} |";
            streamWriter.WriteLine(String.Format(Format, "Đỉnh".CenterString(6), "Đỉnh Kề".CenterString(15), "Hàng Đợi".CenterString(15)));

            queue.AddFirst(new Tuple<string, string>(s, s));

            while (queue.Count > 0)
            {
                Tuple<string, string> tuple = queue.First();
                queue.RemoveFirst();
                t.Add(tuple);

                s = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format(Format, s.CenterString(6), "TTKT-DỪNG".CenterString(15), "".CenterString(15)));
                    streamWriter.Write($"\nĐường đi:{PrintWayToFile(t, tuple, s1, e)}");
                    return;
                }

                if (_adj.TryGetValue(s, out LinkedList<string> list))
                {
                    list = new LinkedList<string>(list.OrderByDescending(x => w[x]).ToArray());

                    foreach (var val in list)
                        queue.AddFirst(new Tuple<string, string>(s, val));
                }
                   
                streamWriter.WriteLine(String.Format(Format, s.CenterString(6), (list != null ? string.Join(",", list) : "").CenterString(15), string.Join(",", queue.Select(x => x.Item2).ToArray()).CenterString(15)));
            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }

    }
}

