using Frontend.Model;
using Frontend.ViewModel;
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
using Frontend.View;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                BoardsListView boardsList = new BoardsListView(u);
                boardsList.Show();
                this.Close();
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel v = viewModel.Register();
            if (v != null)
            {
                BoardsListView boardsList = new BoardsListView(v);
                boardsList.Show();
                this.Close();
            }

        }

        private void Usernamebox(object sender, TextChangedEventArgs e)
        {

        }

        private void Passbox(object sender, TextChangedEventArgs e)
        {

        }
    }
}
