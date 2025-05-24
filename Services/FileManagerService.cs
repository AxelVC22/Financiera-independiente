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
        string SaveFile(string defaultFileName);
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

        public string SaveFile(string defaultFileName)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = defaultFileName,
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                DefaultExt = System.IO.Path.GetExtension(defaultFileName)
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }
    }

}
