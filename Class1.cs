using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace pract3
{
    public class Note
    {
        public int id;
        public DateTime date;
        public string title;
        public string desc;

        public Note(int id, string title, string desc, DateTime date)
        {
            this.title = title;
            this.desc = desc;
            this.date = date;
            this.id = id;
        }
    }

    public class Schedule
    {
        public readonly List<Note> todayNotes;
        public readonly List<Note> allNotes;
        public DateTime selectedDate;
        public int selectedId = -1;

        public Schedule(DateTime date)
        {
            todayNotes = Data.LoadNotes(date);
            allNotes = Data.LoadNotes();
            selectedDate = date;
        }

        public void RefreshNotes()
        {
            todayNotes.Clear();
            todayNotes.AddRange(Data.LoadNotes(selectedDate));
        }

        public void UpdateNotes()
        {
            Data.SaveNotes(allNotes);
        }

        public void NewNote(string title, string description, DateTime date)
        {
            int id = allNotes.Count > 0 ? allNotes[allNotes.Count - 1].id + 1 : 0; 
            Note note = new Note(id, title, description, date);
            allNotes.Add(note);
            UpdateNotes();
            RefreshNotes();
        }

        public void EditNote(string title, string description, DateTime date)
        {
            if (selectedId != -1 && selectedId < todayNotes.Count)
            {
                Note note = new Note(todayNotes[selectedId].id, title, description, date);
                DeleteNote();
                allNotes.Add(note);
                UpdateNotes();
                RefreshNotes();
                selectedId = -1;
            }
        }

        public void DeleteNote()
        {
            if (selectedId != -1 && selectedId < todayNotes.Count)
            {
                int id = todayNotes[selectedId].id;
                allNotes.RemoveAll(note => note.id == id);
                RefreshNotes();
                UpdateNotes();
            }
        }
    }

    public class Data
    {
        private const string notesFilePath = "notes.json";

        public static void SaveNotes(List<Note> notes)
        {
            try
            {
                File.WriteAllText(notesFilePath, JsonConvert.SerializeObject(notes));
            }
            catch 
            { 
               
            }
        }

        public static List<Note> LoadNotes(DateTime date = default)
        {
            List<Note> notes;
            try
            {
                string jsonData = File.ReadAllText(notesFilePath);
                notes = JsonConvert.DeserializeObject<List<Note>>(jsonData);

                if (date != default)
                {
                    notes = notes.FindAll(note => note.date == date);
                }
            }
            catch
            {
                notes = new List<Note>();
                SaveNotes(notes);
            }

            return notes;
        }
    }
}