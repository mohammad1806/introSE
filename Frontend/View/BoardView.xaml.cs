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
using System.Windows.Shapes;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        public BoardViewModel viewModel;
        private UserModel user;

        public BoardView(UserModel user, string name, int id)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(user, name, id);
            this.DataContext = viewModel;
            this.user = user;
        }

     
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BoardsListView boardsList = new BoardsListView(user);
            boardsList.Show();
            this.Close();
        }
    }
}