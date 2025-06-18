using Independiente.DataAccess.Repositories;
using Independiente.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Independiente
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SessionService SessionService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUserRepository userRepository = new UserRepository();
            SessionService = new SessionService(userRepository);
        }
    }
}
