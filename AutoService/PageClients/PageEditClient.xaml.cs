using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using AutoService.ApplicationData;
using AutoService.SystemAppClass;
using Microsoft.Win32;

namespace AutoService.PageClients
{
    /// <summary>
    /// Логика взаимодействия для PageEditClient.xaml
    /// </summary>
    public partial class PageEditClient : Page
    {
        bool Edit = false;
        BitmapImage image;
        string imagePath = "";
        Client originClient;
        List<Tag> tag;
        public PageEditClient()
        {
            InitializeComponent();
            Edit = false;
        }

        public PageEditClient(Client client)
        {
            InitializeComponent();
            Edit = true;
            originClient = client;
            if (client != null)
            {
                TbID.Text = client.IdClient.ToString();
                TbFirstName.Text = client.FirstName.ToString();
                TbLastName.Text = client.LastName.ToString();
                TbMiddleName.Text = client.MiddleName.ToString();
                TbEmail.Text = client.Emall.ToString();
                TbEmail.Text = client.Emall.ToString();
                TbPhone.Text = client.Phone.ToString();
                TbDateBirth.SelectedDate = client.BirhDate;
                switch (client.IdGender)
                {
                    case "м":
                        {
                            CbGender.SelectedIndex = 0;
                            break;
                        }
                    case "ж":
                        {
                            CbGender.SelectedIndex = 1;
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Ошибка базы данных");
                            break;
                        }
                }
                SetPhoto(Convert.ToInt32(client.IdPhoto));
                var clientTags = AppConnect.modelOdb.ClientTag.Where(a => a.IdClient == client.IdClient).ToString();
                foreach(var item in clientTags)
                {
                    tag.Add(AppConnect.)
                }
            }

        }

        private void SetPhoto(int Photo)
        {
            var clientPhoto = AppConnect.modelOdb.ClientPhoto
                .Where(x => x.IdClientPhoto.Equals(Photo)).ToList();
            byte[] buffer = clientPhoto[0].Photo.ToArray();
            MemoryStream stream = new MemoryStream(buffer);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            ImageClient.Source = image;
        }

        private void AddPhoto(int Photo)
        {
            var clientPhoto = AppConnect.modelOdb.ClientPhoto
                .Where(x => x.IdClientPhoto.Equals(Photo)).ToList();
            byte[] buffer = clientPhoto[0].Photo.ToArray();
            MemoryStream stream = new MemoryStream(buffer);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            ImageClient.Source = image;
        }


        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            ClientPhoto clienPhoto = new ClientPhoto();
            int maxId = 0;
            UInt64 memoryImage = 0;
            if (Edit)
            {
                //AppConnect.modelOdb.Client
            }
            else
            {
                byte[] buffer = null;
                if (File.Exists(imagePath))
                {
                    byte[] bufferFile = File.ReadAllBytes(imagePath);
                    memoryImage = Convert.ToUInt64(bufferFile.Length);

                }
                if (image != null && imagePath != "")
                {
                    if (memoryImage < 2097152 && memoryImage != 0)
                    {
                        clienPhoto.Photo = File.ReadAllBytes(imagePath);
                        maxId = AppConnect.modelOdb.ClientPhoto.Max(a => a.IdClientPhoto);
                        maxId++;
                        clienPhoto.IdClientPhoto = maxId + 1;
                    }
                    else
                    {
                        MessageBox.Show("Изображение слишком большое", "Warning");
                    }
                }

                clienPhoto = AppConnect.modelOdb.ClientPhoto.FirstOrDefault(x => x.Photo.Equals(buffer));

                Client client = new Client();
                client.IdPhoto = clienPhoto.IdClientPhoto;
                client.BirhDate = TbDateBirth.SelectedDate.Value;
                client.Emall = TbEmail.Text;
                client.FirstName = TbFirstName.Text;
                client.LastName = TbLastName.Text;
                client.MiddleName = TbMiddleName.Text;
                client.Phone = TbPhone.Text;
                client.RegDate = DateTime.Now;
                if (CbGender.SelectedIndex.Equals(0))
                {
                    client.IdGender = "м";
                }
                else
                {
                    client.IdGender = "ж";
                }

                try
                {
                    AppConnect.modelOdb.Client.Add(client);
                    AppConnect.modelOdb.SaveChanges();
                    SystemAppClass.AppFrame.SelectedFrame.Navigate(new PageListClients());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка базы данных" + ex.ToString());
                }
            }
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.SelectedFrame.Navigate(new PageListClients());
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = sender.ToString();
            text = text.Substring(text.IndexOf(":") + 2, text.Length - 1 - text.IndexOf(":") - 2);

            if (text.Length > 50)
            {
                e.Handled = false;
                e.Source = null;
            }
        }

        private bool CheckFio(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            var allowedChars = new List<char>();
            allowedChars.Add('-');
            allowedChars.Add(' ');
            if (str.Any(c => !Char.IsLetter(c) && !allowedChars.Contains(c)))
            {
                return false;
            }
            {
                return true;
            }
        }

        private bool CheckPhone(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            var allowedChars = new List<char>();
            allowedChars.Add('+');
            allowedChars.Add('-');
            allowedChars.Add('+');
            allowedChars.Add('-');
            allowedChars.Add('+');
            allowedChars.Add('-');
            if (str.Any(c => !Char.IsLetter(c) && !allowedChars.Contains(c)))
            {
                return false;
            }
            {
                return true;
            }
        }

        private bool CheckEmail(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }

            Regex regex = new Regex(@"\w+@\w+\.\w+");
            if (regex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TbEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            bool checkmail = CheckEmail(TbEmail.Text);
            if (!checkmail)
            {
                MessageBox.Show("Неверный формат поля email");
                TbEmail.Text = "";
            }
        }


        private void TbLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool checkLastName = CheckFio(TbLastName.Text);
            if (!checkLastName)
            {
                string buffer = TbLastName.Text;
                buffer = buffer.Remove(buffer.Length - 1, 1);
                TbLastName.Text = buffer;

            }
            TbID.Focus();
            TbLastName.Focus();
        }

        private void TbFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool checkLastName = CheckFio(TbFirstName.Text);
            if (!checkLastName)
            {
                string buffer = TbFirstName.Text;
                buffer = buffer.Remove(buffer.Length - 1, 1);
                TbFirstName.Text = buffer;
            }
            TbID.Focus();
            TbFirstName.Focus();
        }

        private void TbMiddleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool checkLastName = CheckFio(TbMiddleName.Text);
            if (!checkLastName)
            {
                string buffer = TbMiddleName.Text;
                buffer = buffer.Remove(buffer.Length - 1, 1);
                TbMiddleName.Text = buffer;
            }
            TbID.Focus();
            TbMiddleName.Focus();
        }

        private void TbPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool checkPhone = CheckPhone(TbPhone.Text);
            if (checkPhone)
            {
                string buffer = TbPhone.Text;
                buffer = buffer.Remove(buffer.Length - 1, 1);
                TbPhone.Text = buffer;
            }
            TbID.Focus();
            TbPhone.Focus();
        }

        private void EditPhoto_Click(object sender, RoutedEventArgs e)
        {
            string path = null;
            var PictureDialog = new OpenFileDialog();
            PictureDialog.Filter = "(*.bmp, *.jpg) | *.bmp; *.jpg";

            if (PictureDialog.ShowDialog() == true)
            {
                imagePath = PictureDialog.FileName;
            }

            if (path != null)
            {
                Uri pathImage = new Uri(path);
                image = new BitmapImage(pathImage);
                ImageClient.Source = image;
            }
        }
    }
}
