using MediaCenter.Model;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace MediaCenter.Views {
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : BlurryControls.Controls.BlurryWindow, IDisposable {
        public User user;

        public AuthWindow() {
            InitializeComponent();

            user = new User();
        }

        public void Dispose() {
            this.Close();
        }

        private void close_btn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e) {
            string login = login_tb.Text;
            string password = password_pb.Password;

            var db = new DBConnection();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `users` WHERE `Login` = {login} AND `Password` = {password}", db.getConnection());
            command.Parameters.Add("Login", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("Password", MySqlDbType.VarChar).Value = password;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0) {
                user.Login = login;
                DialogResult = true;
            } else {
                user = null;
                DialogResult = false;
            }
        }

        private void registration_btn_Click(object sender, RoutedEventArgs e) {
            Process.Start("https://mymusiccenter.ru/registration/reg.php");
            //string login = login_tb.Text;
            //string password = password_pb.Password; 

            //var db = new DBConnection();
            //DataTable table = new DataTable();
            //MySqlDataAdapter adapter = new MySqlDataAdapter();
            //MySqlCommand command = new MySqlCommand($"INSERT INTO `users`(`Login`, `Password`, `Avatar`) VALUES ( '{login}', '{password}', 'null'\r\n)", db.getConnection());
            //command.Parameters.Add("Login", MySqlDbType.VarChar).Value = login;
            //command.Parameters.Add("Password", MySqlDbType.VarChar).Value=password;

            //adapter.SelectCommand= command;
            //adapter.Fill(table);
        }
    }
}
