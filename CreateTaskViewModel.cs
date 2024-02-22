using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate_App.ViewModel
{
    [ObservableObject]
    internal partial class CreateTaskViewModel
    {
        [ObservableProperty] private string taskName;
    }
}
