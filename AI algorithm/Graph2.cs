using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AI_algorithm
{
    class Graph2
    {
        Dictionary<string, LinkedList<Tuple<string, int>>> _adj;
        Dictionary<string, int> w;

        public Graph2()
        {
            _adj = new Dictionary<string, LinkedList<Tuple<string, int>>>();
            w = new Dictionary<string, int>();
        }

        public void AddEdge(string v, string w, int c)
        {
            if (_adj.TryGetValue(v, out LinkedList<Tuple<string, int>> linkList))
            {
                linkList.AddLast(new Tuple<string, int>(w, c));
            }
            else
            {
                LinkedList<Tuple<string, int>> linklistnew = new LinkedList<Tuple<string, int>>();
                linklistnew.AddLast(new Tuple<string, int>(w, c));
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
        public void WirteFile(StreamWriter streamWriter, string format, string TT, string TTK, int k, int h, int g, int f, string list)
        {
            streamWriter.WriteLine(String.Format(format, TT.CenterString(6), TTK.CenterString(6), k.ToString().CenterString(6), h.ToString().CenterString(6), g.ToString().CenterString(6), f.ToString().CenterString(6), list.CenterString(30)));
        }
        public void WirteFile2(StreamWriter streamWriter, string format, string TT, string TTK, int k, int h, int g, int f, string list1, string list)
        {
            streamWriter.WriteLine(String.Format(format, TT.CenterString(6), TTK.CenterString(6), k.ToString().CenterString(6), h.ToString().CenterString(6), g.ToString().CenterString(6), f.ToString().CenterString(6), list1.CenterString(25), list.CenterString(30)));
        }
        public string PrintWayToFile(List<Tuple<string, string, int>> l, Tuple<string, string, int> t, string s, string k)
        {
            if (s != t.Item1) return PrintWayToFile(l, l.Where(x => x.Item2 == t.Item1).FirstOrDefault(), s, k) + $" -> {t.Item2 }";
            else return $" {t.Item1 } -> {t.Item2 }";
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
                    }
                    else
                    {
                        for (int i = 2; i < str.Count(); i += 2)
                            AddEdge(str[0], str[i], int.Parse(str[i + 1]));

                        AddWeight(str[0], int.Parse(str[1]));
                    }
                }
            }
        }

        public void ASearchAlgorithm(string s, string e, StreamWriter streamWriter)
        {
            string s1 = s;
            LinkedList<Tuple<string, string, int>> queue = new LinkedList<Tuple<string, string, int>>();

            int TotalCost = 0;

            string Format = "| {0} | {1} | {2} | {3} | {4} | {5} | {6} |";
            streamWriter.WriteLine(String.Format(Format, "TT".CenterString(6), "TTK".CenterString(6), "k(u,v)".CenterString(6), "h(v)".CenterString(6), "g(v)".CenterString(6), "f(v)".CenterString(6), "Danh sách L".CenterString(30)));
            streamWriter.WriteLine("".CenterString(88, '-'));

            List<Tuple<string, string, int>> t = new List<Tuple<string, string, int>>();

            queue.AddLast(new Tuple<string, string, int>(s, s, 0));

            while (queue.Count > 0)
            {
                Tuple<string, string, int> tuple = queue.First();
                queue.RemoveFirst();
                t.Add(tuple);
                TotalCost = tuple.Item3;

                s = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format($"| {{0}} | {{1}} |", s.CenterString(6), $"TTKT-DỪNG, đường đi:{PrintWayToFile(t, tuple, s1, e)} độ dài {TotalCost}".CenterString(75)));
                    return;
                }

                if (_adj.TryGetValue(s, out LinkedList<Tuple<string, int>> list))
                {
                    foreach (var val in list)
                        queue.AddLast(new Tuple<string, string, int>(s, val.Item1, val.Item2 + TotalCost));

                    queue = new LinkedList<Tuple<string, string, int>>(queue.OrderBy(x => (w[x.Item2] + x.Item3)).ThenBy(x => x.Item1).ToArray());

                    bool isFirst = true;
                    foreach (var item in list)
                    {
                        WirteFile(streamWriter, Format, isFirst ? s : "", item.Item1, item.Item2, w[item.Item1], item.Item2 + TotalCost, w[item.Item1] + item.Item2 + TotalCost, ((queue != null && isFirst) ? string.Join(",", queue.Select(x => x.Item2 + (w[x.Item2] + x.Item3))) : ""));
                        if (isFirst) isFirst = false;
                    }
                    streamWriter.WriteLine("".CenterString(88, '-'));
                }
            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }

        public void BranchAndBound(string s, string e, StreamWriter streamWriter)
        {
            string s1 = s;
            LinkedList<Tuple<string, string, int>> queue = new LinkedList<Tuple<string, string, int>>();

            int TotalCost = 0;

            string Format = "| {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |";
            streamWriter.WriteLine(String.Format(Format, "TT".CenterString(6), "TTK".CenterString(6), "k(u,v)".CenterString(6), "h(v)".CenterString(6), "g(v)".CenterString(6), "f(v)".CenterString(6), "DS L1".CenterString(25), "Danh sách L".CenterString(30)));
            streamWriter.WriteLine("".CenterString(116, '-'));

            List<Tuple<string, string, int>> t = new List<Tuple<string, string, int>>();

            queue.AddLast(new Tuple<string, string, int>(s, s, 0));

            while (queue.Count > 0)
            {
                Tuple<string, string, int> tuple = queue.First();
                queue.RemoveFirst();
                t.Add(tuple);
                TotalCost = tuple.Item3;

                s = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format("| {0} | {1} | {2} |", s.CenterString(6), $"TTKT, tìm được đường đi tạm thời:{PrintWayToFile(t, tuple, s1, e)} độ dài {TotalCost}".CenterString(70), string.Join(",", queue.Select(x => x.Item2 + (w[x.Item2] + x.Item3))).CenterString(30)));
                    streamWriter.WriteLine("".CenterString(116, '-'));
                    if (queue.Count > 0 && queue.First().Item3 < TotalCost) continue;
                    else return;
                }

                if (_adj.TryGetValue(s, out LinkedList<Tuple<string, int>> list))
                {
                    LinkedList<Tuple<string, int>> list1 = new LinkedList<Tuple<string, int>>(list.OrderByDescending(x => (w[x.Item1] + x.Item2)).ThenBy(x => x.Item1).ToArray());

                    foreach (var val in list1)
                        queue.AddFirst(new Tuple<string, string, int>(s, val.Item1, val.Item2 + TotalCost));

                    bool isFirst = true;
                    foreach (var item in list)
                    {
                        WirteFile2(streamWriter, Format, isFirst ? s : "", item.Item1, item.Item2, w[item.Item1], item.Item2, w[item.Item1] + item.Item2, isFirst ? string.Join(",", list1.Reverse().Select(x => x.Item1 + (TotalCost + w[x.Item1] + x.Item2))) : "", ((queue != null && isFirst) ? string.Join(",", queue.Select(x => x.Item2 + (w[x.Item2] + x.Item3))) : ""));
                        if (isFirst) isFirst = false;
                    }
                }
                streamWriter.WriteLine("".CenterString(116, '-'));

            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
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
                List<string> list = new List<string>();
                if (_adj.TryGetValue(s, out LinkedList<Tuple<string, int>> outlist))
                {
                    list = outlist.Select(x => x.Item1).ToList();
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

                List<string> list = new List<string>();
                if (_adj.TryGetValue(s, out LinkedList<Tuple<string, int>> outlist))
                {
                    list = outlist.Select(x => x.Item1).ToList();
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

                List<string> list = new List<string>();
                if (_adj.TryGetValue(s, out LinkedList<Tuple<string, int>> outlist))
                {
                    list = outlist.Select(x => x.Item1).ToList();

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

            string Format = "| {0} | {1} | {2} | {3} |";
            streamWriter.WriteLine(String.Format(Format, "Phát triển TT".CenterString(6), "Trạng thái kề".CenterString(15), "Danh sách L1".CenterString(15), "Danh sách L".CenterString(15)));

            queue.AddFirst(new Tuple<string, string>(s, s));

            while (queue.Count > 0)
            {
                Tuple<string, string> tuple = queue.First();
                queue.RemoveFirst();
                t.Add(tuple);

                s = tuple.Item2;
                if (s == e)
                {
                    streamWriter.WriteLine(String.Format(Format, s.CenterString(6), "TTKT-DỪNG".CenterString(15), "".CenterString(30)));
                    streamWriter.Write($"\nĐường đi:{PrintWayToFile(t, tuple, s1, e)}");
                    return;
                }
                LinkedList<string> listSort = new LinkedList<string>();
                IEnumerable<string> list;
                if (_adj.TryGetValue(s, out LinkedList<Tuple<string, int>> outlist))
                {
                    list = outlist.Select(x => x.Item1);
                    listSort = new LinkedList<string>(listtest.OrderByDescending(x => w[x]).ToArray());

                    foreach (var val in listSort)
                        queue.AddFirst(new Tuple<string, string>(s, val));
                }

                streamWriter.WriteLine(String.Format(Format, s.CenterString(6), (listSort != null ? string.Join(",", listSort) : "").CenterString(15), (listSort != null ? string.Join(",", listSort) : "").CenterString(15), string.Join(",", queue.Select(x => x.Item2).ToArray()).CenterString(15)));
            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }
    }
}
