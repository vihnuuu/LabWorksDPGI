using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace DPGI_lab2
{
    public partial class MainWindow : Window
    {
        // Команди для кнопок
        public ICommand OpenCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Реєстрація команд
            OpenCommand = new RelayCommand(OpenFile);
            ClearCommand = new RelayCommand(ClearText);

            // Створення прив'язки та приєднання обробників для команди Save
            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, execute_Save, canExecute_Save);
            CommandBindings.Add(saveCommand);

            // Прив'язка контексту до вікна
            this.DataContext = this;
        }

        // Обробник кнопки "Відкрити"
        private void OpenFile(object parameter)
        {
            // Відкриття діалогового вікна для вибору файлу
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = System.IO.File.ReadAllText(filePath);
                inputTextBox.Text = fileContent; // Вставлення вмісту файлу в TextBox
            }
        }

        // Обробник кнопки "Стерти"
        private void ClearText(object parameter)
        {
            // Очищення вмісту TextBox
            inputTextBox.Clear();
        }

        // Команди для збереження
        void canExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            // Перевірка, чи є текст для збереження
            if (inputTextBox.Text.Trim().Length > 0) e.CanExecute = true; else e.CanExecute = false;
        }

        void execute_Save(object sender, ExecutedRoutedEventArgs e)
        {
            // Запис вмісту TextBox у файл
            System.IO.File.WriteAllText("C:\\Users\\vihnu\\OneDrive\\Рабочий стол\\dpgi_lab2.txt", inputTextBox.Text);
            MessageBox.Show("The file was saved!");
        }
    }

    // Реалізація RelayCommand для команди
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
