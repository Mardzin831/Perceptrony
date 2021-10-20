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
        public int [,] array = new int [5, 7];
        public List<Array> examples = new List<Array>();
        public List<int> answers = new List<int>();
        public List<double> w = new List<double>();
        public MainWindow()
        {
            InitializeComponent();
            /*using (StreamReader r = new StreamReader("Przykład.json"))
            {
                string json = r.ReadToEnd();
                examples = JsonConvert.DeserializeObject<List<Array>>(json);
            }*/
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
            int a = Int32.Parse(bt.Tag.ToString()) / 10;
            int b = Int32.Parse(bt.Tag.ToString()) % 10;
            if(array[a, b] == 0)
            {
                array[a, b] = 1;
            }
            else
            {
                array[a, b] = 0;
            }
            predictNumber.Content = array[a, b];
        }
        private void AddExample(object sender, RoutedEventArgs e)
        {
            if(answerBox.Text.Length != 1)
            {
                return;
            }
            if (examples.Contains(array))
            {
                MessageBox.Show("Taki przykład już istnieje.");
                return;
            }
            examples.Add(array);
            answers.Add(Int32.Parse(answerBox.Text));
            string json = JsonConvert.SerializeObject(examples);
            System.IO.File.WriteAllText("Przykłady.json", json);

        }
    }
}
