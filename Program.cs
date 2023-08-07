using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Congratulator
{
     class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DRFriend"].ConnectionString;
        private static SqlConnection sqlConnection = null;

        //вывод меню
        public static void MenuPrint()
        {
            Console.WriteLine(
                "1 - Показать ближайшие дни рождения\n" +
                "2 - Показать все дни рождения\n"+
                "3 - Добавить запись\n"+
                "4 - Удалить запись\n" +
                "5 - Редактировать запись\n" +
                "6 - Сортировать записи\n" +
                "7 - Выйти\n\n");
        }
        public static void Create_Comm(string command)
        {
            SqlDataReader sqlDataReader = null;
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Close();
        }
        //вывод всего списка
        public static void All_Note(string command)
        {
  
            SqlDataReader sqlDataReader = null;
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Console.WriteLine($"{sqlDataReader["Id"]} {sqlDataReader["Name"]} {sqlDataReader["Last_Name"]} {sqlDataReader["Birthday"]}");
            }
            sqlDataReader.Close();
        }
        //вывод ближайших 
        public static void Immediate_Note()
        {
            SqlDataReader sqlDataReader = null;
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Friend WHERE MONTH(Birthday) = MONTH(getdate()) AND DAY(Birthday)>=DAY(getdate()) AND DAY(Birthday)<=DAY(getdate()+7);", sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                Console.WriteLine($"{sqlDataReader["Id"]} {sqlDataReader["Name"]} {sqlDataReader["Last_Name"]} {sqlDataReader["Birthday"]}");
                Console.WriteLine(" ");
            }
            sqlDataReader.Close();
        }
        //добавление записей
        public static void Add_Note()
        {
            string a, b, c,command;
            Console.WriteLine("Введите имя:");
            a = Console.ReadLine();
            Console.WriteLine("Введите фамилию:");
            b = Console.ReadLine();
            Console.WriteLine("Введите дату рождения в формате ДД.ММ.ГГГГ:");
            c = Console.ReadLine();
            command = "INSERT INTO Friend (Name,Last_Name,Birthday)VALUES(N'"+a+"',N'"+b+"','"+c+"')";
            SqlDataReader sqlDataReader = null;
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Close();
            Console.WriteLine("Запись успешно добавлена!");
        }
        //удаление записей
        public static void Del_Note()
        {
            string a,command;
            Console.WriteLine("Введите ID записи, которую хотите удалить:");
            a = Console.ReadLine();
            command = "DELETE FROM Friend WHERE id=" + a;
            SqlDataReader sqlDataReader = null;
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Close();
            Console.WriteLine("Запись успешно удалена!");
            
        }
        //редактирование записей
        public static void Edit_Note()
        {
             string a,c, command;
            int b;
             Console.WriteLine("Введите ID записи, которую хотите редактировать:");
             a = Console.ReadLine();
             Console.WriteLine("Введите номер поля, которого хотите редактировать.\n1 - Имя\n2 - Фамилия \n3 - Дата");
             b = Convert.ToInt32(Console.ReadLine());
            switch (b)
            {
                case 1: 
                    Console.WriteLine("Введите новое имя:");
                    c = Console.ReadLine();
                    command = "UPDATE Friend SET Name=N'" + c + "' WHERE id=" + a;
                    Create_Comm(command);
                    break;
                case 2: 
                    Console.WriteLine("Введите новую фамилию:");
                    c =Console.ReadLine();
                    command = "UPDATE Friend SET Last_Name=N'" + c + "' WHERE id=" + a;
                    Create_Comm(command);
                    break;
                case 3: 
                    Console.WriteLine("Введите новую дату рождения в формате ДД.ММ.ГГГГ:");
                    c =Console.ReadLine();
                    command = "UPDATE Friend SET Birthday='" + c + "' WHERE id=" + a;
                    Create_Comm(command);
                    break;
                    default: 
                    Console.WriteLine(" Не удалось определить команду");
                    break;
            }
             
        }
        //сортировка записей
        public static void Sort_Note()
        {
            int a;
            Console.WriteLine("Выберете вид сортировки:\n1 - По алфавиту\n2 - Прошедшие дни рождения\n3 - Предстоящие дни рождения\n");
             a = Convert.ToInt32(Console.ReadLine());
            switch(a)
            {
                case 1:
                    All_Note("SELECT * FROM Friend ORDER BY Name");
                    break;
                case 2:
                    All_Note("SELECT * FROM Friend WHERE MONTH(Birthday) <= MONTH(getdate()) AND DAY(Birthday)<DAY(getdate())");
                    break;
                case 3:
                    All_Note("SELECT * FROM Friend WHERE MONTH(Birthday) >= MONTH(getdate()) AND DAY(Birthday)>=DAY(getdate())");
                    break;
            }

        }
        public static void Main(string[] args)
        {
            int answer = 1;
            Boolean t = true;
            string command = string.Empty;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            while (t)
            {

                switch (answer)
                {
                    case 1: 
                        Immediate_Note();
                        break;
                    case 2:
                        All_Note("SELECT * FROM Friend");
                        break;
                    case 3:
                        Add_Note(); 
                        break;
                    case 4:  
                        Del_Note();
                        break;
                    case 5:  
                        Edit_Note();
                        break;
                    case 6:  
                        Sort_Note();
                        break;
                    case 7: 
                        t = false;
                        sqlConnection.Close();
                        break;
                        default: 
                        Console.WriteLine("Введена несуществующая операция"); 
                        break;
                }
                
                if (t)
                {
                    MenuPrint();
                    Console.WriteLine("Введите номер операции:");
                    answer = Convert.ToInt32(Console.ReadLine());
                }

            }

            Console.ReadKey();
        }
    }
}
