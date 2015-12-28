using System;
using System.Collections.Generic;
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

namespace SequentialTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Formula test = StringToFormula.Parse("P(x) -> #xQ(x) = #xP(x) -> Q(x)");
                Tree tree = new Tree(test);
                MessageBox.Show(tree.Check().ToString());
                string examples = "";
                foreach (var example in tree.CounterExamples)
                {
                    examples += example.ToString() + "\n";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Info);
            }
        }
    }
}
