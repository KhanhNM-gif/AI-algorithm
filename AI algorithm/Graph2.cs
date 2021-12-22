using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        WirteFile(streamWriter, Format, isFirst ? s : "", item.Item1, item.Item2, w[item.Item1], item.Item2, w[item.Item1] + item.Item2, ((queue != null && isFirst) ? string.Join(",", queue.Select(x => x.Item2 + (w[x.Item2] + x.Item3))) : ""));
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
                    LinkedList<Tuple<string, int>> list1 = new LinkedList<Tuple<string, int>>(list.OrderByDescending(x => (w[x.Item1] + x.Item2)).ThenBy(x=>x.Item1).ToArray());

                    foreach (var val in list1)
                        queue.AddFirst(new Tuple<string, string, int>(s, val.Item1, val.Item2 + TotalCost));

                    bool isFirst = true;
                    foreach (var item in list)
                    {
                        WirteFile2(streamWriter, Format, isFirst ? s : "", item.Item1, item.Item2, w[item.Item1], item.Item2, w[item.Item1] + item.Item2, isFirst ? string.Join(",", list1.Reverse().Select(x => x.Item1 + (TotalCost+w[x.Item1] + x.Item2))):"", ((queue != null && isFirst) ? string.Join(",", queue.Select(x => x.Item2 + (w[x.Item2] + x.Item3))) : ""));
                        if (isFirst) isFirst = false;
                    }
                }
                streamWriter.WriteLine("".CenterString(116, '-'));

            }
            streamWriter.WriteLine($"\nKhông tìm thấy đường");
        }
    }
}
