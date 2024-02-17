using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace pract3
{
    public partial class MainWindow : Window
    {
        public static Schedule schedule;
        public static DateTime selectedDate = DateTime.Today;

        public MainWindow()
        {
            InitializeComponent();
            schedule = new Schedule(selectedDate);
            UpdateList();
            dateContainer.SelectedDate = selectedDate;
        }

        public void UpdateList()
        {
            selectedDate = schedule.selectedDate;
            TaskContainer.Items.Clear();
            schedule.RefreshNotes();
            foreach (Note note in schedule.todayNotes)
            {
                TaskContainer.Items.Add(note.title);
            }
            titleBox.Text = "";
            descriptionBox.Text = "";
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            string title = titleBox.Text;
            string description = descriptionBox.Text;
            schedule.NewNote(title, description, selectedDate);
            UpdateList();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            string title = titleBox.Text;
            string description = descriptionBox.Text;
            schedule.EditNote(title, description, selectedDate);
            UpdateList();
        }

        private void ChangeSelect(object sender, SelectionChangedEventArgs e)
        {
            if (TaskContainer.SelectedIndex != -1)
            {
                schedule.selectedId = TaskContainer.SelectedIndex;
                Note selectedNote = schedule.todayNotes[schedule.selectedId];
                titleBox.Text = selectedNote.title;
                descriptionBox.Text = selectedNote.desc;
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DateTime.TryParse(dateContainer.Text, out DateTime selectedDate))
                {
                    schedule.selectedDate = selectedDate;
                    UpdateList();
                }
            }
            catch 
            {
                
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (schedule.selectedId != -1)
            {
                schedule.DeleteNote();
                UpdateList();
            }
        }
    }
}
