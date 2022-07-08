using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Calcolatrice
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Soluzione_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            float num1 = float.Parse(Num1.Text);

            float num2 = float.Parse(Num2.Text);

            float c = num1 + num2;

            string d = c.ToString();

            Soluzione.Text = d;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            float num1 = float.Parse(Num1.Text);

            float num2 = float.Parse(Num2.Text);

            float c = num1 - num2;

            string d = c.ToString();

            Soluzione.Text = d;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            float num1 = float.Parse(Num1.Text);

            float num2 = float.Parse(Num2.Text);

            float c = num1 * num2;

            string d = c.ToString();

            Soluzione.Text = d;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            float num1 = float.Parse(Num1.Text);

            float num2 = float.Parse(Num2.Text);

            if (num2 == 0)
            {

                Soluzione.Text = ("Il secondo numero deve essere diverso da 0");
            
            }

            else
            {

                float c = num1 / num2;

                string d = c.ToString();

                Soluzione.Text = d;

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            float num1 = float.Parse(Num1.Text);

            float num2 = float.Parse(Num2.Text);

            double c = Math.Pow(num1, num2);

            string d = c.ToString();

            Soluzione.Text = d;
        }
    }
}
