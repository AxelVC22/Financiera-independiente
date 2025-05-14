using Independiente.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services.Mappers
{
    public static class FileMapper
    {
        public static DataAccess.File ToDataModel(this Model.File source)
        {
            DataAccess.File file = new DataAccess.File();

            if (source != null)
            {
                file = new DataAccess.File
                {
                    File1 = source.FileContent,
                    ClientId = source.Client.ClientId,
                    Type = source.FileType.ToString(),
                };
            }

            return file;
        }

        public static Model.File ToViewModel(this DataAccess.File source)
        {
            Model.File file = new Model.File();

            if (source != null)
            {
                file = new Model.File
                {
                    FileContent = source.File1,
                    FileType = (FileType)Enum.Parse(typeof(FileType), source.Type)
                };
            }

            return file;
        }
    }
}
