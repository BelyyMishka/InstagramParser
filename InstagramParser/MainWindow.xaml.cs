using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;

namespace InstagramParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик кнопки Обзор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onFolderBrowserDialog(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            FolderTextBox.Text = folderBrowserDialog.SelectedPath;
        }

        /// <summary>
        /// Обработчик кнопки Начать парсинг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void onStartParse(object sender, RoutedEventArgs e)
        {
            string userId = UserIdTextBox.Text;
            string folder = string.Empty;
            string server = string.Empty;
            string port = string.Empty;
            string login = string.Empty;
            string password = string.Empty;
            string databaseName = string.Empty;

            if (Excel.IsChecked == true)
            {
                folder = FolderTextBox.Text;

                if (!Directory.Exists(folder))
                {
                    showMessageBox("Выберите существующую папку!");
                    return;
                }
            }
            else
            {
                server = ServerTextBox.Text;
                port = PortTextBox.Text;
                login = LoginTextBox.Text;
                databaseName = DatabaseNameTextBox.Text;
                password = PasswordTextBox.Password;

                if(string.IsNullOrWhiteSpace(server))
                {
                    showMessageBox("Вы не ввели сервер!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(port))
                {
                    showMessageBox("Вы не ввели порт!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(login))
                {
                    showMessageBox("Вы не ввели логин!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    showMessageBox("Вы не ввели пароль!");
                    return;
                }

                if(string.IsNullOrWhiteSpace(databaseName))
                {
                    showMessageBox("Вы не ввели название БД!");
                    return;
                }
            }
 
            if (string.IsNullOrWhiteSpace(userId))
            {
                showMessageBox("Вы не ввели id пользователей!");
                return;
            }

            switchButtonState(sender);
            switchCursorState();

            string[] userIdArray = splitUserId(userId);

            ParseResult parseResult = new ParseResult();

            if (Excel.IsChecked == true) await getDataAsync(userIdArray, folder, parseResult);
            else await getDataAsync(userIdArray, server, port, password, login, databaseName, parseResult);

            output(parseResult, userIdArray.Length);

            switchCursorState();
            switchButtonState(sender);
        }

        private void output(ParseResult parseResult, int count)
        {
            if (parseResult.getCount() != count) showMessageBox("Записано " + parseResult.getCount() + " из " + count + " пользователей. Проверьте правильность входных данных.");
            else showMessageBox("Готово! Записано " + parseResult.getCount() + " из " + count + " пользователей.");
        }

        /// <summary>
        /// Получаем данные
        /// </summary>
        /// <param name="item">Пользователь</param>
        /// <param name="folder">Папка</param>
        private void getData(string item, string folder, ParseResult parseResult)
        {
            bool isWrong = false;

            try
            {
                dynamic json = JSON.getScript(HTML.getScript(item));
                CSV.writeToCsv(item, InstagramData.fullName(json), InstagramData.webSite(json), InstagramData.followers(json), InstagramData.geolocation(json), folder);
            }
            catch
            {
                isWrong = true;
            }
            finally
            {
                if (!isWrong) parseResult.increaseCount();
            }
        }

       /// <summary>
       /// Получаем данные асинхронно
       /// </summary>
       /// <param name="userIdArray">Массив пользователей</param>
       /// <param name="folder">Папка</param>
       /// <returns></returns>
        private async Task getDataAsync(string[] userIdArray, string folder, ParseResult parseResult)
        {
            foreach(var item in userIdArray)
            {
                await Task.Run(() => getData(item, folder, parseResult));
            }
        }

        /// <summary>
        /// Получаем данные
        /// </summary>
        /// <param name="item">Пользователь</param>
        private void getData(string item, string server, string port, string password, string login, string databaseName, ParseResult parseResult)
        {
            bool isWrong = false;

            try
            {
                dynamic json = JSON.getScript(HTML.getScript(item));
                InstagramParser.DB.newRecord(server, port, databaseName, password, login, item, InstagramData.fullName(json), InstagramData.webSite(json), InstagramData.followers(json), InstagramData.geolocation(json));
            }
            catch
            {
                isWrong = true;
            }
            finally
            {
                if (!isWrong) parseResult.increaseCount();
            }
        }

        /// <summary>
        /// Получаем данные асинхронно
        /// </summary>
        /// <param name="userIdArray">Массив пользователей</param>
        /// <returns></returns>
        private async Task getDataAsync(string[] userIdArray, string server, string port, string password, string login, string databaseName, ParseResult parseResult)
        {
            foreach (var item in userIdArray)
            {
                await Task.Run(() => getData(item, server, port, password, login, databaseName, parseResult));
            }
        }

        /// <summary>
        /// Выводим сообщение на экран
        /// </summary>
        /// <param name="text">Текст</param>
        private void showMessageBox(string text)
        {
            System.Windows.MessageBox.Show(text);
        }

        /// <summary>
        /// Меняем курсор
        /// </summary>
        private void switchCursorState()
        {
            if(Mouse.OverrideCursor == null)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.AppStarting;
                return;
            }

            Mouse.OverrideCursor = null;
        }

        /// <summary>
        /// Меняем состояние кнопки
        /// </summary>
        /// <param name="sender">Источник события</param>
        private void switchButtonState(object sender)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            button.IsEnabled = !(button.IsEnabled);
        }

        /// <summary>
        /// Разделяем id пользователей
        /// </summary>
        /// <param name="userId">Id пользователей</param>
        /// <returns></returns>
        private string[] splitUserId(string userId)
        {
            userId = userId.ToLower();
            return userId.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
