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

// Można poprawić wyświetlanie odpowiedzi bo coś jest nie tak.

namespace Perceptrony
{
    public partial class MainWindow : Window
    {
        public List<List<double>> perceptron = new List<List<double>>();
        public List<List<int>> examples = new List<List<int>>();
        public List<int> answers = new List<int>();
        public List<int> example = new List<int>();
        public double learn_const = 0.5;
        public Random randW = new Random();
        public Random randE = new Random();
        public Random randEj = new Random();
        public int maxRounds = 1000;


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
                check(examples, answers, i);
            }
            
        }
        
        public void check(List<List<int>> E, List<int> ans, int perc)
        {
            int T = -1;
            int O = -1;
            double sum = 0;
            double g = 0;
            for(int k = 0; k < 70; k++)
            {
                sum = 0;
                if (ans[k] == perc)
                {
                    T = 1;
                }
                else
                {
                    T = -1;
                }

                for (int i = 1; i < 36; i++)
                {
                    sum += perceptron[perc][i] * E[k][i - 1];
                }

                if (sum < perceptron[perc][0])
                {
                    O = -1;
                }
                else
                {
                    O = 1;
                }
                if(O == T)
                {
                    g++;
                }
            }
            Debug.WriteLine(g / 70 * 100);
        }
        public List<double> Weights(List<double> w)
        {
            for(int i = 0; i < 36; i++)
            {
                w.Add(2.0 * randW.NextDouble() - 1.0);
            }
            return w;
        }
        public void Reverse(List<List<int>> E, int drawn, int reverse)
        {

            //if (E[drawn][reverse] == -1)
            //{
            //    E[drawn][reverse] = 1;
            //}
            //else
            //{
            //    E[drawn][reverse] = -1;
            //}
        }
        private List<double> Train(List<List<int>> E, List<int> ans, int perc)
        {
            List<double> w = new List<double>();
            w = Weights(w);
            List<double> pocket = new List<double>();
            pocket.AddRange(w);
            int reverse;
            int drawn;
            int T;
            int O;
            int ERR;
            int lifespan = 0;
            int record = 0;
            int round = 0;
            while (round < maxRounds)
            {
                drawn = randE.Next(E.Count());
                reverse = randEj.Next(35);
                double sum = 0;

                Reverse(E, drawn, reverse);
                if (ans[drawn] == perc)
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
                        pocket.Clear();
                        pocket.AddRange(w);
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
                Reverse(E, drawn, reverse);
                round++;
            }
            return pocket;
        }
        public void Predictions()
        {
            Debug.WriteLine("--------------------");
            Debug.WriteLine("--------------------");
            for (int i = 0; i < 35; i++)
            {
                Debug.Write(example[i] == 1 ? "X" : "-");
                if ((i + 1) % 5 == 0)
                {
                    Debug.WriteLine("");
                }
           
                
            }
            double sum = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 36; j++)
                {
                    sum += perceptron[i][j] * example[j - 1];
                }
                if (sum < perceptron[i][0])
                {
                    Debug.Write("-");
                    switch (i)
                    {
                        case 0:
                            isNumber0.Content = "Nie";
                            isNumber0.Background = Brushes.Transparent;
                            break;
                        case 1:
                            isNumber1.Content = "Nie";
                            isNumber1.Background = Brushes.Transparent;
                            break;
                        case 2:
                            isNumber2.Content = "Nie";
                            isNumber2.Background = Brushes.Transparent;
                            break;
                        case 3:
                            isNumber3.Content = "Nie";
                            isNumber3.Background = Brushes.Transparent;
                            break;
                        case 4:
                            isNumber4.Content = "Nie";
                            isNumber4.Background = Brushes.Transparent;
                            break;
                        case 5:
                            isNumber5.Content = "Nie";
                            isNumber5.Background = Brushes.Transparent;
                            break;
                        case 6:
                            isNumber6.Content = "Nie";
                            isNumber6.Background = Brushes.Transparent;
                            break;
                        case 7:
                            isNumber7.Content = "Nie";
                            isNumber7.Background = Brushes.Transparent;
                            break;
                        case 8:
                            isNumber8.Content = "Nie";
                            isNumber8.Background = Brushes.Transparent;
                            break;
                        case 9:
                            isNumber9.Content = "Nie";
                            isNumber9.Background = Brushes.Transparent;
                            break;

                    }
                }
                else
                {
                    Debug.Write("1");
                    switch (i)
                    {
                        case 0:
                            isNumber0.Content = "Tak";
                            isNumber0.Background = Brushes.PaleGreen;
                            break;
                        case 1:
                            isNumber1.Content = "Tak";
                            isNumber1.Background = Brushes.PaleGreen;
                            break;
                        case 2:
                            isNumber2.Content = "Tak";
                            isNumber2.Background = Brushes.PaleGreen;
                            break;
                        case 3:
                            isNumber3.Content = "Tak";
                            isNumber3.Background = Brushes.PaleGreen;
                            break;
                        case 4:
                            isNumber4.Content = "Tak";
                            isNumber4.Background = Brushes.PaleGreen;
                            break;
                        case 5:
                            isNumber5.Content = "Tak";
                            isNumber5.Background = Brushes.PaleGreen;
                            break;
                        case 6:
                            isNumber6.Content = "Tak";
                            isNumber6.Background = Brushes.PaleGreen;
                            break;
                        case 7:
                            isNumber7.Content = "Tak";
                            isNumber7.Background = Brushes.PaleGreen;
                            break;
                        case 8:
                            isNumber8.Content = "Tak";
                            isNumber8.Background = Brushes.PaleGreen;
                            break;
                        case 9:
                            isNumber9.Content = "Tak";
                            isNumber9.Background = Brushes.PaleGreen;
                            break;
                    }
                }
            }
            Debug.WriteLine("");
        }
        private void ChangeColorClicked(object sender, RoutedEventArgs e)
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
            Predictions();
        }
        private void AddExampleClicked(object sender, RoutedEventArgs e)
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
