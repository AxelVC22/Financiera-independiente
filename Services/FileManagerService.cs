using Microsoft.Win32;
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
<<<<<<< HEAD
=======


>>>>>>> f568a5bed656a25741bd745082a5d087572d9732
        string SaveFile(string defaultFileName);
        string SelectPath();
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
<<<<<<< HEAD
=======

        public string saveCSV(string defaultFileName)
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

>>>>>>> f568a5bed656a25741bd745082a5d087572d9732
        public string SelectPath()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Selecciona la carpeta donde se guardará el layout de cobro";

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    return dialog.SelectedPath;
                }

                return null;
            }
        }

    }

}
