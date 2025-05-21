using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{

    public class AmortizationScheduleQuery : INotifyPropertyChanged, IQueryObject<AmortizationSchedule>
    {
        public string _status;

        public int CreditApplicaitonId { get; set; }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Expression<Func<AmortizationSchedule, bool>> BuildExpression()
        {
            return c =>
          (string.IsNullOrEmpty(Status) || c.Status == Status) &&
          (CreditApplicaitonId == 0 || c.CreditId == CreditApplicaitonId);
        }
    }

    public interface IAmortizationScheduleRepository
    {
        List<AmortizationSchedule> GetAmortizationSchedule(AmortizationScheduleQuery query);
    }


    public class AmortizationScheduleRepository : IAmortizationScheduleRepository
    {
        public List<AmortizationSchedule> GetAmortizationSchedule(AmortizationScheduleQuery query)
        {
            List<AmortizationSchedule> amortizationSchedules = new List<AmortizationSchedule>();

            var predicate = query.BuildExpression();

            using (var context = new IndependienteEntities())
            {
                amortizationSchedules = context.AmortizationSchedule
                    .Where(predicate)
                    .ToList();
            }

            return amortizationSchedules;
        }
    }
}
