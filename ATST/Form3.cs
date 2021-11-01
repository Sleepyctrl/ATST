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
    public partial class Form3 : Form
    {
        
        
        public Form3()
        {
            InitializeComponent();
             }

        public void Form3_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            string[] name = new string[7] { "Михаил", "Максим", "Артём", "Василий", "Иван", "Фёдор", "Алексей" };
            string[] sername = new string[7] { "Михайлович", "Максимович", "Артёмьевич", "Васильевич", "Иванович", "Фёдорович", "Александрович" };
            //id контрагента
            string id = "1e70f4c6-3ce0-4544-824a-3afa253cae84";
            string passportSerial,passnumber;
            int namenumb, sernamenumb,key,passportnumb1,passportnumb2,phonenumber,pay=5000,snils;
            Random rnd = new Random();
            //серия делится на 2 цифры первые две (код региона), 2 следующие (год выдачи)
            passportnumb1 = rnd.Next(1,89);
            // год выдачи взят с ориентиром на то, что человеку меньше 60 и больше 18
            passportnumb2 = rnd.Next(58, 99);
            passportSerial = Convert.ToString(passportnumb1)+Convert.ToString(passportnumb2);
            namenumb = rnd.Next(0, 6);
            sernamenumb = rnd.Next(0, 6);
            passnumber = Convert.ToString(rnd.Next(100000, 999999));
            //) Проверка контрольного числа Страхового номера проводится только для номеров больше номера 001-001-998
            snils = rnd.Next(001001998, 999999999);
            string sn=snils.ToString("000000000");
            char[] snilsv = sn.ToCharArray();
            phonenumber = rnd.Next(100000000, 999999999);
            //Пользователь выбирает какой именно платёж хочет
            if(radioButton2.Checked==true)
            {
                pay = rnd.Next(1, 59999);
            }
            if(radioButton1.Checked==true)
            {
                pay = rnd.Next(1, 599999);
            }
            int k = rnd.Next(001, 100);
            // снилс формируется так, первые  9 цифр генерируется а два последних являются контрольной суммой, которые перемножаются на позицию, а после складываются
            //snils = rnd.Next(100000000000, 99999999999);
            DateTime thisDay = DateTime.Now;
            //считывание файла с номером документа который находится в директиве программы
            key = Convert.ToInt32(File.ReadAllText("key.txt"));
            string Namefile = "ATST -" + key;
            //текст самого xml документа
            // существует две оплаты первое назвать "Оплата" там оплата до 1 до 60000, второе назвать Оплата через Евросеть и там от 0 до 600000
            string pack = "<?xml version='1.0' encoding='utf-8'?><Root><Person Id='" + id + "' LastName='Федоров' FirstName='" + name[namenumb] +
                "' MiddleName='" + sername[sernamenumb] + "' DocType='паспорт' DocSerial='"+passportSerial+"' DocNumber='"+passnumber+
                "' DocFull='Паспорт РФ серия "+passportSerial+" № "+passnumber+
                " выдан 04.06.2013 Отделом УФМС России по г. Москве по р-ну Коньково  д/р 01.03.1993' Snils='14570498581' PhoneNumber='9"+phonenumber+
                "' /><Document Id='e41c92fd-0302-4a2a-9608-4957e2cfa95a' Number='"+Namefile+"' Date='"+ thisDay.ToShortDateString() + "' Total='"+pay+
                ".00' PersonId='0c8a0760-dbf4-11e7-826c-18a9053c5943'></Document></Root>";
            XmlDocument doc = new XmlDocument();
            //создаём папку куда будут отправляться контрагенты
            Directory.CreateDirectory("ATST");
            doc.LoadXml(pack);
            doc.Save("ATST\\"+Namefile+".xml");
            // Перезапись файла(нумерация файла идёт на 1 больше)
            StreamWriter sr = new StreamWriter("key.txt", false);
            sr.WriteLine(++key);
            sr.Close();
            // File.
            // for (int n=0; n< Convert.ToInt32(numericUpDown1); n++)
            // {

            // }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
