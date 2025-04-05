using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public interface IFilePickerService
    {
        string PickFile();
    }

    public class FilePickerService : IFilePickerService
    {
        public string PickFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName; 
            }
            return null; 
        }
    }

}
