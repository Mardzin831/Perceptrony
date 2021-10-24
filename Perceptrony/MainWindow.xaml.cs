using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Perceptrony
{
    public partial class MainWindow : Window
    {
        public List<int> example = new List<int>();
        public List<List<double>> perceptron = new List<List<double>>();
        public List<List<int>> examples = new List<List<int>>();
        public List<int> answers = new List<int>();
        //public List<double> w = new List<double>();
        double learn_const = 0.1;
        
        public MainWindow()
        {
            InitializeComponent();
            using (StreamReader r = new StreamReader("Przykłady.json"))
            {
                string json = r.ReadToEnd();
                if(json.Length > 0)
                {
                    examples = JsonConvert.DeserializeObject<List<List<int>>>(json);
                }                
            }
            using (StreamReader r = new StreamReader("Odpowiedzi.json"))
            {
                string json = r.ReadToEnd();
                if (json.Length > 0)
                {
                    answers = JsonConvert.DeserializeObject<List<int>>(json);
                }
            }
            for (int i = 0; i < 35; i++)
            {
                example.Add(-1);
            }
            for (int i = 0; i < 10; i++)
            {
                perceptron.Add(Train(examples, answers, i));
            }
        }
        public List<double> Weights(List<double> w)
        {
            Random rand = new Random();
            for(int i = 0; i < 36; i++)
            {
                w.Add(2 * rand.NextDouble() - 1);
            }
            return w;
        }

        private List<double> Train(List<List<int>> E, List<int> ans, int perc)
        {
            List<double> w = new List<double>();
            w = Weights(w);
            List<double> pocket = w;
            Random randE = new Random();
            int drawn;
            int T;
            int O;
            int ERR;
            int lifespan = 0;
            int record = 0;

            int rounds = 0;
            while (rounds < 100)
            {
                drawn = randE.Next(E.Count());
                double sum = 0;

                if(ans[drawn] == perc)
                {
                    T = 1;
                }
                else
                {
                    T = -1;
                }

                for (int i = 1; i < 36; i++)
                {
                    sum += w[i] * E[drawn][i - 1];   
                }

                if(sum < w[0])
                {
                    O = -1;
                }
                else
                {
                    O = 1;
                }

                ERR = T - O;
                if(ERR == 0)
                {
                    lifespan++;
                    if(lifespan > record)
                    {
                        record = lifespan;
                        pocket = w;
                    }
                }
                else
                {
                    for (int i = 1; i < 36; i++)
                    {
                        w[i] += learn_const * ERR * E[drawn][i - 1];
                    }
                    w[0] -= learn_const * ERR;
                    lifespan = 0;
                }
                //E.RemoveAt(drawn);
                //T.RemoveAt(drawn);
                rounds++;
            }
            return pocket;
        }
        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Background == Brushes.Transparent)
            {
                bt.Background = Brushes.Black;
            }
            else
            {
                bt.Background = Brushes.Transparent;
            }
            int a = Int32.Parse(bt.Tag.ToString()) - 1;
            if(example[a] == -1)
            {
                example[a] = 1;
            }
            else
            {
                example[a] = -1;
            }
            predictNumber.Content = $"";
        }
        private void AddExample(object sender, RoutedEventArgs e)
        {
            if(answerBox.Text.Length != 1)
            {
                MessageBox.Show("Podaj odpowiedź do przykładu.");
                return;
            }
            foreach(List<int> list in examples)
            {
                if (list.SequenceEqual(example))
                {
                    MessageBox.Show("Taki przykład już istnieje.");
                    return;
                }
            }
            
            examples.Add(example);
            answers.Add(Int32.Parse(answerBox.Text));
            string json = JsonConvert.SerializeObject(examples);
            File.WriteAllText("Przykłady.json", json);
            json = JsonConvert.SerializeObject(answers);
            File.WriteAllText("Odpowiedzi.json", json);
            example = new List<int>(example);
        }
    }
}
