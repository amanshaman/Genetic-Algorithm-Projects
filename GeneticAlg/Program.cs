using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlg
{
    class Program
    {
        static void Main(string[] args)
        {
            ///treasure init.
            int[,] arr = new int[30, 40];
            for (int y = 29; y >= 0; y--)
            {
                for (int x = 0; x < 40; x++)
                {
                    arr[y, x] = Convert.ToInt32(Math.Round(Math.Max((y + 1) / 30.0, (1 / 40.0) * (41.0 - (x + 1))), 4) * 10000);
                }
            }

            Word foo = new Word(arr);
            foo.MemberInicialization(0);
            Selection sel = new Selection(0, foo.ListOfMembers);
            List<Member> newMembers = new List<Member>();
            newMembers = sel.NewMembers;

            Console.Read();
        }
    }
    
    class Word : Member
    {
        private int[,] word;
        private List<Member> listOfMembers = new List<Member>();

        /// <summary>
        /// basic inicialization
        /// </summary>
        public Word()
        {
            this.word = null;
        }

        /// <summary>
        /// Inicialization with int[,] array.
        /// </summary>
        /// <param name="word">Array containing word above which we work.</param>
        public Word(int[,] word)
        {
            if (word == null)
            {
                Console.WriteLine("Array is empty. Give me filled array.");
                return;
            }
            this.word = word;
        }       

        /// <summary>
        /// inicialization of member for pre-created tasks. If you want to created your own member type. Use directly Member class.
        /// </summary>
        /// <param name="a">Number from 0 to 2.</param>
        public void MemberInicialization(int a)
        {
            Random r = new Random();

            for (int i = 0; i < 100; i++)
            {
                listOfMembers.Add(
                    new Member(a, r));
            }

            //int[] a = new int[68] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            //1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            //1, 1, 1, 1, 1, 1, 1, 1, 1,
            //0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            //0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            //0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            //0, 0, 0, 0, 0, 0, 0, 0, 0,};

            //listOfMembers.Add(
            //    new Member(a));

            SetFitness();
            SetPropability();
            
        }
        
        public void SetFitness()
        {
            int counter = 0, x = 0, y = 0;

            for (int i = 0; i < listOfMembers.Count(); i++)
            {
                for (int j = 0; j < listOfMembers[i].Individual.Count(); j++)
                {
                    counter += word[y, x];
                    if (listOfMembers[i].Individual[j] == 0)
                    {
                        x++;
                    }
                    else
                    {
                        y++;
                    }
                }
                listOfMembers[i].Fitness = counter;
                counter = 0;
                x = 0;
                y = 0;
            }
            
        }

        public void SetPropability()
        {
            double sumOfFitness = 0;            
            foreach (var item in listOfMembers)
            {
                sumOfFitness += item.Fitness;
            }
            double sumOfProbability = 0;
            foreach (var item in listOfMembers)
            {
                sumOfProbability = Math.Round((double)(item.Fitness / sumOfFitness) + sumOfProbability,4);
                item.Propability = sumOfProbability;
            }
        }

        public List<Member> ListOfMembers
        {
            get { return listOfMembers; }
            set { listOfMembers = value; }
        }

    }

    class Member
    {
        private int[] individual;
        private double fitness;
        private double propability;

        public Member()
        {
            //do nothing
        }

        /// <summary>
        /// Allows to create individuals by user.
        /// </summary>
        /// <param name="individual"></param>
        public Member(int[] individual)
        {
            this.individual = individual;
        }

        /// <summary>
        /// Choose one of the predefined individuals.
        /// 0 is for Find a treasure.
        /// 1 is for Traveling Salesman Problem.
        /// 2 is for Robotic arms.
        /// </summary>
        /// <param name="c">Number from 0-x.Choose wisely.</param>
        /// <param name="r">Random seed. Needed for creaton of different members</param>
        public Member(int c, Random r)
        {
            switch (c)
            {
                case 0:
                    FindTreasure(r);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        private void FindTreasure(Random r)
        {
            int x = 39, y = 29;
            individual = new int[68];
            for (int i = 0; i < 68; i++)
            {
                if (r.Next(0,2) == 0)
                {
                    if (x > 0)
                    {
                        individual[i] = 0;
                        x--;
                    }
                    else
                    {
                        individual[i] = 1;
                        y--;
                    }
                }
                else
                {
                    if (y > 0)
                    {
                        individual[i] = 1;
                        y--;
                    }
                    else
                    {
                        individual[i] = 0;
                        x--;
                    }
                }
            }
        }

        public int[] Individual
        {
            get { return individual; }
            set { individual = value; }
        }

        public double Fitness
        {
            get { return fitness; }
            set { fitness = value; }
        }

        public double Propability
        {
            get { return propability; }
            set { propability = value; }
        }
    }

    class Selection 
    {
        List<Member> newMembers = new List<Member>();
        public Selection()
        {
            //basic inicialization
        }

        /// <summary>
        /// Select type of selection.
        /// </summary>
        /// <param name="a"></param>
        public Selection(int a, List<Member> listOfMembers)
        {
            switch (a)
            {
                case 0:
                    SSwR(listOfMembers);
                    break;
                default:
                    break;
            }
        }

        private void SSwR(List<Member> listOfMembers)
        {
            listOfMembers.Sort((x, y) => x.Propability.CompareTo(y.Propability));
            //List<Member> SortedList = listOfMembers.OrderBy(o => o.Propability).ToList(); 
            Random rand = new Random();
            for (int i = 0; i < listOfMembers.Count(); i++)
            {
                double r = rand.NextDouble();
                if (r < listOfMembers[0].Propability)
                {
                    newMembers.Add(listOfMembers[0]);
                }
                else if (r > listOfMembers[listOfMembers.Count()-1].Propability)
                {
                    newMembers.Add(listOfMembers[listOfMembers.Count()-1]);
                }
                else
                {
                    for (int j = 0; j < listOfMembers.Count()-2; j++)
                    {
                        if (r > listOfMembers[j].Propability && r < listOfMembers[j + 1].Propability)
                        {
                            newMembers.Add(listOfMembers[j + 1]);
                            break;
                        }
                    }
                }                
            }
        }

        public List<Member> NewMembers
        {
            get { return newMembers; }
            set { newMembers = value; }
        }

    }

    class Mutations
    {
        public Mutations()
        {
            ///basic class
        }

        public Mutations(List<Member> childs, double chance)
        {
            Random r = new Random();
            for (int i = 0; i < childs.Count(); i++)
            {
                if (r.NextDouble() < chance)
                {
                    int a = r.Next(0, childs.Count());
                    int b = r.Next(0, childs.Count());

                    int temp = childs[i].Individual[a];
                    childs[i].Individual[a] = childs[i].Individual[b];
                    childs[i].Individual[b] = temp;
                }
                else
                {
                    //not doing mutation
                }
            }
        }
    }



                
}
