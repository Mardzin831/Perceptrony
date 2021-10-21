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
        public List<int> example = new List<int>(new int[35]);
        public List<List<int>> examples = new List<List<int>>();
        public List<int> answers = new List<int>();
        public List<double> w = new List<double>();
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

            Weights();
        }
        private void Weights()
        {
            Random rand = new Random();
            for(int i = 0; i < 30; i++)
            {
                w.Add(rand.NextDouble());
            }
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
            if(example[a] == 0)
            {
                example[a] = 1;
            }
            else
            {
                example[a] = 0;
            }
            predictNumber.Content = example[a];
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
