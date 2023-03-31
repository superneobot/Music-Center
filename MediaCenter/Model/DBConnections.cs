using MySql.Data.MySqlClient;

namespace MediaCenter.Model {
    public class DBConnection {
        public static string Server { get; set; }
        public static string DatabaseName { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }

        MySqlConnection Connection = new MySqlConnection("server=localhost;UID=root;password=;database=basa");


        public DBConnection() {
            Connection.Ping();
        }

        public void openConnection() {
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
        }
        public void closeConnection() {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
        }

        public MySqlConnection getConnection() {
            return Connection;
        }
    }
}
