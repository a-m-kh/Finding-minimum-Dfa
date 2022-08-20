using System;
using System.Collections.Generic;

namespace MinimumDFA
{
    class Group
    {
        public int Step;
        public int number;
        public string name;
    }
    class State
    {
        public Group group;
        public string name;
        public bool final = false;
        public List<Yal> yals = new List<Yal>();
        public State(string Name)
        {
            name = Name;
        }
    }

    class Yal
    {
        public State now;
        public string language;
        public State next;
    }
    class Program
    {

        static List<State> FindState(List<State> Fin, List<string> Language)
        {
            List<State> see = new List<State>();
            foreach (var state in Fin)
            {
                foreach (Yal yal in state.yals)
                {
                    see.Add(yal.next);
                }
            }
            return see;
        }
        static List<State> Diffrent(ref List<State> All, List<State> See)
        {
            foreach (var state in See)
            {
                if (All.Find(a => a.name == state.name) != null)
                {
                    continue;
                }
                All.Add(state);
            }
            return All;
        }
        static bool Is_Diffrent(List<State> All, List<State> see)
        {
            int i = 0;
            foreach (var state in see)
            {
                if (All.Find(a => a.name == state.name) != null)
                {
                    i++;
                }
            }
            if (i == see.Count)
            {
                return false;
            }
            return true;
        }
        static void Detect(ref List<State> All, List<State> Find, List<string> Language)
        {
            List<State> between = new List<State>();
            All = Diffrent(ref All, Find);
            Find = FindState(Find, Language);
            if (!Is_Diffrent(All, Find))
            {
                return;
            }
            Detect(ref All, Find, Language);
        }

        static string[,] WRite(List<State> start, List<string> Language)
        {
            string[,] alls = new string[start.Count, 2 * Language.Count + 2];
            int i = 0;
            foreach (var s in start)
            {
                alls[i, 0] = s.name;
                int k = 1;
                foreach (var L in Language)
                {
                    alls[i, k] = L;
                    k++;
                    alls[i, k] = s.yals.Find(a => a.language == L).next.group.name;
                    k++;
                }
                if(s.final)
                {
                    alls[i, k] = "1";

                }
                else
                {
                    alls[i, k] = "2";
                }
                i++;
            }
            return alls;
        }
        static string[,] WRite2(List<State> start, List<string> Language)
        {
            string[,] alls = new string[start.Count, 2 * Language.Count + 2];
            int i = 0;
            foreach (var s in start)
            {
                alls[i, 0] = s.name;
                int k = 1;
                foreach (var L in Language)
                {
                    alls[i, k] = L;
                    k++;
                    alls[i, k] = s.yals.Find(a => a.language == L).next.name;
                    k++;
                }
                if (s.final)
                {
                    alls[i, k] = "1";

                }
                else
                {
                    alls[i, k] = "2";
                }
                i++;
            }
            return alls;
        }

        static int determine(string[,] fin, int length, int height)
        {
            int j = 1;
            int NumberOfGroup = 0;
            while (true)
            {
                int k = 0;
                string str = j.ToString();
                for (int i = 0; i < height; i++)
                {
                    if (fin[i, length] == str)
                    {
                        k++;
                    }
                }
                if (k == 0)
                {
                    break;
                }
                NumberOfGroup++;
                j++;
            }
            return NumberOfGroup;
        }

        static string[,] design(string[,] doing, int length, int height, string[] Language, string[,] AL)
        {
            for (int j = 0; j < height; j++)
            {
                int k = 2;
                for (int i = 0; i < Language.Length; i++)
                {
                    string str = AL[j, k];
                    for (int l = 0; l < height; l++)
                    {
                        if (doing[l, 0] == str)
                        {
                            doing[j, k] = doing[l, length];
                            break;
                        }
                    }
                    k += 2;
                }
            }
            return doing;
        }

        static string[,] Grouped(ref string[,] fin, int length, int height)
        {
            string[] Group = new string[height];
            for(int i =0;i<height;i++)
            {
                Group[i] = fin[i, length];
            }
            List<string> Grouped = new List<string>();
            int NumberOfGroup = 1;
            for (int i = 0; i < height; i++)
            {
                if (Grouped.Contains(fin[i, 0]))
                {
                    continue;
                }
                fin[i, length] = NumberOfGroup.ToString();
                Grouped.Add(fin[i, 0]);
                for (int k = 1; k < height; k++)
                {
                    if (Grouped.Contains(fin[k, 0]))
                    {
                        continue;
                    }
                    if (i == k)
                    {
                        continue;
                    }
                    int error = 0;
                    for (int j = 2; j < length; j += 2)
                    {
                        if (fin[i, j] != fin[k, j] || Group[i] != Group[k])
                        {
                            error++;
                        }
                    }
                    if (error == 0)
                    {
                        fin[k, length] = NumberOfGroup.ToString();
                        Grouped.Add(fin[k, 0]);
                    }
                }
                NumberOfGroup++;
            }
            return fin;
        }

        static bool IS_OK(string[,] fin, int length, int height, string[,] fin2)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length + 1; j++)
                {
                    if (fin[i, length] != fin2[i, length])
                    {

                        //Console.WriteLine("====>" + i + " :" + fin[i, length] + "  ==> " + fin2[i, length] +"<==");
                        return false;
                    }
                }
            }
            return true;
        }
        static int Final(ref string[,] fin, int length, int height, string[] Language, ref string[,] AL)
        {
            /*Console.WriteLine("------------");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length + 1; j++)
                {
                    Console.Write(fin[i, j] + " ");
                }
                Console.WriteLine();
            }*/
            string[,] fin2 = new string[height, length + 1];
            while (true)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < length + 1; j++)
                    {
                        fin2[i, j] = fin[i, j];
                    }
                }
                fin = design(fin, length, height, Language, AL);
                fin = Grouped(ref fin, length, height);
                /*Console.WriteLine("------------");
                for (int i = 0; i < height; i++)*/
                /*{
                    for (int j = 0; j < length + 1; j++)
                    {
                        Console.Write(fin[i, j] + " ");
                    }
                    Console.WriteLine();
                }*/
                if (IS_OK(fin, length, height, fin2))
                {
                    break;
                }
                
            }
            return determine(fin, length, height);
        }
        static void Main(string[] args)
        {
            string[] states = Console.ReadLine().Split('{', '}')[1].Split(',');
            string[] language = Console.ReadLine().Split('{', '}')[1].Split(',');
            string[] FinalState = Console.ReadLine().Split('{', '}')[1].Split(',');
            int number = int.Parse(Console.ReadLine());
            string[,] yals = new string[number, 3];
            List<State> States = new List<State>();
            List<string> LA = new List<string>();
            foreach (string state in states)
            {
                State s = new State(state);
                States.Add(s);
            }
            foreach (string state in FinalState)
            {
                States.Find(a => a.name == state).final = true;
            }
            for (int i = 0; i < number; i++)
            {
                string[] yal = Console.ReadLine().Split(',');
                Yal y = new Yal();
                y.now = States.Find(a => a.name == yal[0]);
                y.language = yal[1];
                y.next = States.Find(a => a.name == yal[2]);
                States.Find(a => a.name == yal[0]).yals.Add(y);
            }

            Group g1 = new Group() { name = "1", Step = 0, number = 1 };
            Group g2 = new Group() { name = "2", Step = 0, number = 2 };
            
            for (int i = 0; i < states.Length; i++)
            {
                LA.Add(states[i]);
            }
            var xi = States.Find(a => a.name == states[0]);

            List<State> Find = new List<State> { xi };
            States = Find;
            Detect(ref States, Find, LA);
            foreach (var x in States)
            {
                if (x.final)
                {
                    x.group = g2;
                }
                else
                {
                    x.group = g1;
                }
            }
            List<string> Lang = new List<string>();
            for (int i = 0; i < language.Length; i++)
            {
                Lang.Add(language[i]);
            }
            //string[,] alm = WRite(States, Lang);
            string[,] A = WRite2(States, Lang);
            string[,] B = new string[States.Count, 2 * Lang.Count + 2];
            for (int i = 0; i < States.Count; i++)
            {
                for (int j = 0; j < 2 * Lang.Count + 2; j++)
                {
                    B[i, j] = A[i, j];
                }
            }
            /*Console.WriteLine("___________________________");
            for (int i = 0; i < States.Count; i++)
            {
                for (int j = 0; j < 2 * Lang.Count + 2; j++)
                {
                    Console.Write(A[i, j] + " ");
                }
                Console.WriteLine();
            }*/
            string[,] N = design(A, 2 * Lang.Count + 1, States.Count, language, A);
            /*Console.WriteLine("___________________________");
            for (int i = 0; i < States.Count; i++)
            {
                for (int j = 0; j < 2*Lang.Count + 2; j++)
                {
                    Console.Write(N[i, j] + " ");
                }
                Console.WriteLine();
            }*/
            N = Grouped(ref N, 2 * Lang.Count + 1, States.Count);
            /*Console.WriteLine("___________________________");
            for (int i = 0; i < States.Count; i++)
            {
                for (int j = 0; j < 2 * Lang.Count + 2; j++)
                {
                    Console.Write(N[i, j] + " ");
                }
                Console.WriteLine();
            }*/
            /*Console.WriteLine("________________________");
            for(int i = 0; i< States.Count; i++)
            {
                for(int j =0;j<2*Lang.Count +1;j++)
                {
                    Console.Write(B[i, j] + " ");
                }
                Console.WriteLine();
            }*/
            //Console.WriteLine("________________________");
            Console.WriteLine(Final(ref N, 2 * Lang.Count + 1, States.Count, language, ref B));
        }
    }
}
