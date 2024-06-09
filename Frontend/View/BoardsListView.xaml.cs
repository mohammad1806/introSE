using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Frontend.ViewModel;
using Frontend.Model;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardsList.xaml
    /// </summary>
    public partial class BoardsListView : Window
    {
        private BoardsListViewModel viewModel;
        private UserModel user;
        public BoardsListView(UserModel u)
        {
            InitializeComponent();
            this.user = u;
            this.viewModel = new BoardsListViewModel(u);
            this.DataContext = viewModel;
            
        }
        
        

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_Button(object sender, RoutedEventArgs e)
        {
            string n = viewModel.getBrdName();
            int id = viewModel.GetBrdId();
            BoardView view = new BoardView(user, n, id);
            view.Show();
            this.Close();
        }
        private void Logout_Button(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}