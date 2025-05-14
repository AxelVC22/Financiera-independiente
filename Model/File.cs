using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public enum FileType
    {
        INE,
        POA,
        CA
    }

    public class File : INotifyPropertyChanged
    {
        private int _fileId;

        private FileType _fileType;

        private byte[] _fileContent;

        private Client _client;
        public int FileId
        {
            get => _fileId;
            set
            {
                if (_fileId != value)
                {
                    _fileId = value;
                    OnPropertyChanged(nameof(FileId));
                }
            }
        }

        public FileType FileType
        {
            get => _fileType;
            set
            {
                if (_fileType != value)
                {
                    _fileType = value;
                    OnPropertyChanged(nameof(FileType));
                }
            }
        }

        public byte[] FileContent
        {
            get => _fileContent;
            set
            {
                if (_fileContent != value)
                {
                    _fileContent = value;
                    OnPropertyChanged(nameof(FileContent));
                }
            }
        }

        public Client Client
        {
            get => _client;
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged(nameof(Client));
                }
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
