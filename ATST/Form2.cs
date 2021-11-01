using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ATST
{
    public partial class Form2 : Form
    {


        public Form2(int choice = 1)
        {
            InitializeComponent();

        }

        public void button1_Click(object sender, EventArgs e)
        {
            string[] name = new string[7] { "Михаил", "Максим", "Артём", "Василий", "Иван", "Фёдор", "Алексей" };
            string[] sername = new string[7] { "Михайлович", "Максимович", "Артёмьевич", "Васильевич", "Иванович", "Фёдорович", "Александрович" };
            string passportSerial, passnumber, sum1 = "", snilsAll, Namefile = "", pack = "", sqls = "";
            int namenumb, sernamenumb, key, passportnumb1, passportnumb2, phonenumber, pay = 5000;
            int Net, Clog;
            int[] snils = new int[9];
            int[] snilscontrol = new int[9];
            string[] NamesFiles = new string[Convert.ToInt32(numericUpDown1.Value)];
            Random rnd = new Random();
            //id контрагента
            Guid PersonID, DocumentID,ScrapId;
            int ks = 0;
            for (int s = 0; s < numericUpDown1.Value; s++)
            {

                string conn_params = "Server=192.168.1.75;Port=5432;User Id=postgres;Password=ASDqwe!23;Database=test_workstations_dataPM";
                NpgsqlConnection conn = new NpgsqlConnection(conn_params);
                string sql="",result="";
                //проверка существует ли id в бд    
                conn.Open();//Открываем соединение.
                do
                {
                    PersonID = Guid.NewGuid();
                    sqls = Convert.ToString(PersonID);
                     //Например: "Server=127.0.0.1;Port=5432;User Id=postgres;Password=mypass;Database=mybase;"
                    sql = "SELECT id FROM public.persons where id ='" + sqls + "' ";
                    NpgsqlCommand comm = new NpgsqlCommand(sql, conn);

                   
                    try
                    {
                        // если id существует всё пройдёт хорошо
                        result = comm.ExecuteScalar().ToString(); //Выполняем нашу команду.
                        ks = 1;
                    }
                    catch
                    {
                        //если его нету то выдаст ошибку  
                        ks = 0;
                    }
                     
                } while (ks == 1);
                do
                {
                    DocumentID = Guid.NewGuid();
                    sqls = Convert.ToString(DocumentID);
                    sql = "SELECT id FROM public.documents where id ='" + sqls + "' ";
                    NpgsqlCommand comm = new NpgsqlCommand(sql, conn);

                    try
                    {
                        // если id существует всё пройдёт хорошо
                        result = comm.ExecuteScalar().ToString();//Выполняем нашу команду.
                        ks = 1;
                    }
                    catch
                    {
                        //если его нету то выдаст ошибку
                        ks = 0;
                    }

                } while (ks==1);
                do
                {
                    ScrapId = Guid.NewGuid();
                    sqls = Convert.ToString(ScrapId);
                    //без понятия что такое scrap id и откуда брать
                    sql = "SELECT id FROM public.generic_types where id ='" + sqls + "' ";
                    NpgsqlCommand comm = new NpgsqlCommand(sql, conn);

                    try
                    {
                        // если id существует всё пройдёт хорошо
                        result = comm.ExecuteScalar().ToString();//выполняем нашу команду.
                        ks = 1;
                    }
                    catch
                    {
                        //если его нету то выдаст ошибку
                        ks = 0;
                    }

                } while (ks == 1);
                conn.Close();//Закрываем соединение.


                ////

                //серия делится на 2 цифры первые две (код региона), 2 следующие (год выдачи) 89
                passportnumb1 = rnd.Next(10, 89);
                // год выдачи взят с ориентиром на то, что человеку меньше 60 и больше 18
                passportnumb2 = rnd.Next(58, 99);
                passportSerial = Convert.ToString(passportnumb1) + Convert.ToString(passportnumb2);
                namenumb = rnd.Next(0, 6);
                sernamenumb = rnd.Next(0, 6);
                passnumber = Convert.ToString(rnd.Next(100000, 999999));
                //генерация параметров платёжки
                Net = rnd.Next(0, 9999);
                string nett = Net.ToString("0000");
                Clog = rnd.Next(0, 99);
                string clogg = Clog.ToString("00");
                //брутто
                int gros1 = rnd.Next(0, 99999);
                string gross = gros1.ToString("00000");

                //начинается формирование снился


                //) Проверка контрольного числа Страхового номера проводится только для номеров больше номера 001-001-998 
                for (int n = 0; n < snils.Length; n++)
                    {
                        if (n == 2)
                        { snils[n] = rnd.Next(1, 9); }
                        if (n == 5)
                        { snils[n] = rnd.Next(2, 9); }
                        snils[n] = rnd.Next(0, 9);
                        snilscontrol[n] = snils[n];
                    }
                int position = 9;
                int sum = 0;
                for (int n = 0; n < snilscontrol.Length; n++)
                {
                    snilscontrol[n] = position * snilscontrol[n];
                    sum = sum + snilscontrol[n];
                }
                if (sum > 101)
                { sum = sum % 101; }
                //Если сумма равна 100 или 101 то используем sum1, т.к, контрольная сумма равная одному из этих чисел будет равна 00
                if (sum == 100 || sum == 101)
                {
                    sum = 0;
                    sum1 = ("00");
                }

                if (sum == 0)
                {
                    snilsAll = String.Join("", snils) + sum1;
                }
                else
                {
                    snilsAll = String.Join("", snils) + sum;
                }
                phonenumber = rnd.Next(100000000, 999999999);
                //Пользователь выбирает какой именно платёж хочет
                if (radioButton2.Checked == true)
                {
                    pay = rnd.Next(1, 59999);
                }
                if (radioButton1.Checked == true)
                {
                    pay = rnd.Next(1, 149999);
                }
                int Cost = rnd.Next(0, pay);
                int k = rnd.Next(001, 100);
                // снилс формируется так, первые  9 цифр генерируется а два последних являются контрольной суммой, которые перемножаются на позицию, а после складываются
                //snils = rnd.Next(100000000000, 99999999999);
                DateTime thisDay = DateTime.Now;
                //считывание файла с номером документа который находится в директиве программы
                key = Convert.ToInt32(File.ReadAllText("key.txt"));
                Namefile = "TST-" + key;
                //текст самого xml документа
                // существует две оплаты первое назвать "Оплата" там оплата до 1 до 60000, второе назвать Оплата через Евросеть и там от 0 до 600000
                //(Person Id and PersonId) идентификатор контр агента
                if (checkBox1.Checked == true)
                {
                    pack = "<?xml version='1.0' encoding='utf-8'?><Root><Person Id='" + PersonID +
                        "' LastName='Федоров' FirstName='" + name[namenumb] +
                        "' MiddleName='" + sername[sernamenumb] +
                        "' DocType='паспорт' DocSerial='" + passportSerial +
                        "' DocNumber='" + passnumber +
                        "' DocFull='Паспорт РФ серия " + passportSerial + " № " + passnumber +
                        " выдан 04.06.2013 Отделом УФМС России по г. Москве по р-ну Коньково  д/р 01.03.1993' Snils='" + snilsAll +
                        "' PhoneNumber='9" + phonenumber +
                        /*Уникальный номер документа генерируется*/
                        "' /><Document Id='" + DocumentID + "' Number='" + Namefile +
                        "' Date='" + thisDay.ToShortDateString() +
                        "' Total='" + pay +
                        ".00' PersonId='" + PersonID + "'>" +
                        //не знаю, нужно ли генерить
                        "<Position ScrapName='Черный лом(группа 12А)'" +
                        " ScrapId='" + ScrapId + "'" +

                        " Cost='" + Cost + "' " +

                        "Gross='0." + gross + "'" +

                        " Net='0." + nett + "'" +

                        " Clog='" + clogg + "' />" +
                        "</Document></Root>";
                }
                else
                {
                    pack = "<?xml version='1.0' encoding='utf-8'?><Root><Person Id='" + PersonID +
                        "' LastName='Федоров' FirstName='" + name[namenumb] +
                        "' MiddleName='" + sername[sernamenumb] +
                        "' DocType='паспорт' DocSerial='" + passportSerial +
                        "' DocNumber='" + passnumber +
                        "' DocFull='Паспорт РФ серия " + passportSerial + " № " + passnumber +
                        " выдан 04.06.2013 Отделом УФМС России по г. Москве по р-ну Коньково  д/р 01.03.1993' Snils='" + snilsAll +
                        "' PhoneNumber='9" + phonenumber +
                        /*Уникальный номер документа генерируется*/
                        "' /><Document Id='" + DocumentID + "' Number='" + Namefile +
                        "' Date='" + thisDay.ToShortDateString() +
                        "' Total='" + pay +
                        ".00' PersonId='" + PersonID + "'>" +
                        "</Document></Root>";
                }
               
                XmlDocument doc = new XmlDocument();
                //создаём папку куда будут отправляться контрагенты
                Directory.CreateDirectory("TST");
                doc.LoadXml(pack);
                doc.Save("TST\\in_RLRP_" + Namefile + ".xml");
                NamesFiles[s] = "TST\\in_RLRP_" + Namefile + ".xml";
                // Перезапись файла(нумерация файла идёт на 1 больше)
                StreamWriter sr = new StreamWriter("key.txt", false);
                sr.WriteLine(++key);
                sr.Close();
                // File.
                // for (int n=0; n< Convert.ToInt32(numericUpDown1); n++)
                // {
                // }
            }
            Hide();
            Form3 f = new Form3();
            //из-за того, что мы не знаем кол-ва файлов которые запросит пользователь нужно создать массив данных стринг в который будут вносится все имен файлов(директории где они лежат)
            for (int s = 0; s < NamesFiles.Length; s++)
            {
                f.textBox2.Text += NamesFiles[s] + "\r\n";
            }
            f.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 f = new Form1();
            f.ShowDialog();
            this.Close();

        }
    }
}
