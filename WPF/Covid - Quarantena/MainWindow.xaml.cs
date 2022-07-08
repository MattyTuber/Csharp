using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mesures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int positive, quarantine, total;

            if (string.IsNullOrEmpty(positivi.Text) || string.IsNullOrEmpty(quarantenati.Text))
            {
                MessageBox.Show("Non sono ammessi campi vuoti");
                return;
            }

            if (!int.TryParse(positivi.Text, out positive) || !int.TryParse(quarantenati.Text, out quarantine))
            {
                MessageBox.Show("Sono ammessi soltanto numeri");
                return;
            }

            positive = Convert.ToInt32(positivi.Text);//#FFC9B5FF
            quarantine = Convert.ToInt32(quarantenati.Text);

            if(positive < 0 || quarantine < 0)
            {
                MessageBox.Show("Sono ammessi soltanto numeri superiori a zero");
                return;
            }

            total = positive + quarantine;

            switch (total)
            {
                case 0:
                    mask.Stroke = System.Windows.Media.Brushes.Red;
                    ffp2.Stroke = System.Windows.Media.Brushes.LimeGreen;
                    mask_boost.Stroke = System.Windows.Media.Brushes.Red;
                    ffp2_boost.Stroke = System.Windows.Media.Brushes.LimeGreen;
                    dad.Stroke = System.Windows.Media.Brushes.Black;
                    dad_boost.Stroke = System.Windows.Media.Brushes.Black;
                    ddi.Stroke = System.Windows.Media.Brushes.Black;
                    ddi_boost.Stroke = System.Windows.Media.Brushes.Black;
                    break;
                
                case 1:
                    ffp2.Stroke = System.Windows.Media.Brushes.Red;
                    ffp2_boost.Stroke = System.Windows.Media.Brushes.Red;
                    mask.Stroke = System.Windows.Media.Brushes.Black;
                    mask_boost.Stroke = System.Windows.Media.Brushes.Black;
                    dad.Stroke = System.Windows.Media.Brushes.Black;
                    dad_boost.Stroke = System.Windows.Media.Brushes.Black;
                    ddi.Stroke = System.Windows.Media.Brushes.Black;
                    ddi_boost.Stroke = System.Windows.Media.Brushes.Black;

                    misure.Text = "Indossare FFP2 per 10 giorni.\nPasto da consumare a scuola solo se alla distanza di almeno 2metri";
                    break;

                case 2:
                    ffp2_boost.Stroke = System.Windows.Media.Brushes.Red;
                    ddi.Stroke = System.Windows.Media.Brushes.Red;
                    mask.Stroke = System.Windows.Media.Brushes.Black;
                    ffp2.Stroke = System.Windows.Media.Brushes.Black;
                    mask_boost.Stroke = System.Windows.Media.Brushes.Black;
                    dad.Stroke = System.Windows.Media.Brushes.Black;
                    dad_boost.Stroke = System.Windows.Media.Brushes.Black;
                    ddi_boost.Stroke = System.Windows.Media.Brushes.Black;

                    misure.Text = "Per i vaccinati o guariti da più di 120 giorni:\nDDI per 10 giorni\n\nPer gli altri studenti:\nIndossare FFP2 per 10 giorni.\nPasto da consumare a scuola solo se alla distanza di almeno 2 metri";
                    break;
            };

            if (total >= 3)
            {
                dad.Stroke = System.Windows.Media.Brushes.Red;
                dad_boost.Stroke = System.Windows.Media.Brushes.Red;
                mask.Stroke = System.Windows.Media.Brushes.Black;
                ffp2.Stroke = System.Windows.Media.Brushes.Black;
                mask_boost.Stroke = System.Windows.Media.Brushes.Black;
                ffp2_boost.Stroke = System.Windows.Media.Brushes.Black;
                ddi.Stroke = System.Windows.Media.Brushes.Black;
                ddi_boost.Stroke = System.Windows.Media.Brushes.Black;

                misure.Text = "DAD di 10 giorni per tutti gli studenti";
            }
        }
    }
}
