using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{
    public interface IFilePickerService
    {
        string PickFile();
        string SaveFile(string defaultFileName);
        string SelectPath();
        bool SaveFileFromBytes(byte[] fileContent, string defaultFileName);
        bool SaveFileFromBytesToPath(byte[] fileContent, string filePath); 
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

        public bool SaveFileFromBytes(byte[] fileContent, string defaultFileName)
        {
            if (fileContent == null || fileContent.Length == 0)
                return false;

            string filePath = SaveFile(defaultFileName);
            if (string.IsNullOrEmpty(filePath))
                return false;

            return SaveFileFromBytesToPath(fileContent, filePath);
        }

        public bool SaveFileFromBytesToPath(byte[] fileContent, string filePath)
        {
            try
            {
                if (fileContent == null || fileContent.Length == 0)
                    return false;

                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllBytes(filePath, fileContent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
