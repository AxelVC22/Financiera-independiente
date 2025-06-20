using Independiente.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Independiente.ViewModel
{
    public class MenuOption
    {
        public string Name { get; set; }
        public RelayCommand Command { get; set; }
    }
}
