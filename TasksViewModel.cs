using CheckMate.Data;
using CheckMate.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReplayKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.ViewModels
{
    public partial class TasksViewModel: ObservableObject
    {
        private readonly DatabaseContext _context;
        public TasksViewModel(DatabaseContext context) 
        { 
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<UserTask> _tasks;

        [ObservableProperty]
        private UserTask _operatingTask;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        [RelayCommand]
        // Come here when adding subtasks to database

        // If doesn't work come back here!!!
        private async Task LoadTaskAsync()
        {
            var tasks = await _context.GetAllAsync<UserTask>();

            if(tasks is not null && tasks.Any() ) 
            {
                tasks ??= new ObservableCollection<UserTask>();

                foreach(var task in Tasks)
                {
                    Tasks.Add(task);
                }
            }
        }
        [RelayCommand]
        private void SetOperatingTask(UserTask? task)
        {
            OperatingTask = task ?? new();
        }

        [RelayCommand]
        private async Task SaveTaskASync()
        {
            if(OperatingTask is null)
            {
                return;
            }

            if(OperatingTask.Id == 0)
            {
                // Creating the task
                await _context.AddTaskAsync<UserTask>(OperatingTask);
                Tasks.Add(OperatingTask);
            }
            else
            {
                // Updating the task
                await _context.UpdateTaskAsync<UserTask>(OperatingTask);
                var taskCopy = OperatingTask.Clone();

                var index = Tasks.IndexOf(OperatingTask);
                Tasks.RemoveAt(index);

                Tasks.Insert(index, taskCopy);
            }

            SetOperatingTaskCommand.Execute(new());
        }

        [RelayCommand]
        private async Task DeleteTaskAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteTaskByKeyAsync<UserTask>(id))
                {
                    var task = Tasks.FirstOrDefault(p => p.Id == id);
                    Tasks.Remove(task);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Task was not deleted", "OK");
                }
            }, "Deleting task");
            IsBusy = true;
            BusyText = "Deleting task...";
        }
        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
    }
}
