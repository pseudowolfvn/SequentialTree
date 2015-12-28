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
                //Predicate trueP = new Predicate("P", new List<string> { "x" , "y" }, LogicalValue.False);
                //Predicate falseP = new Predicate("P", new List<string> { "y" , "x" }, LogicalValue.False);
                //MessageBox.Show(trueP.Equals(falseP).ToString());
                Formula test = StringToFormula.Parse("P(x) -> #xQ(x) = #xP(x) -> Q(x)");
                VarNamesGenerator.AddUsedNames(test.FreeVarNames());
                Tree tree = new Tree(test);
                MessageBox.Show(tree.Check().ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Info);
            }
        }
    }
}
