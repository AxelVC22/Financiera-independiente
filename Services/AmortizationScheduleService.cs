using Independiente.DataAccess.Repositories;
using Independiente.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Services
{

    public interface IAmortizationScheduleService
    {
        List<Independiente.Model.AmortizationSchedule> GetAmortizationSchedule(AmortizationScheduleQuery query);
    }
    public class AmortizationScheduleService : IAmortizationScheduleService
    {
        private readonly IAmortizationScheduleRepository _amortizationScheduleRepository;

        public AmortizationScheduleService(IAmortizationScheduleRepository amortizationScheduleRepository)
        {
            _amortizationScheduleRepository = amortizationScheduleRepository;
        }

        public List<Independiente.Model.AmortizationSchedule> GetAmortizationSchedule(AmortizationScheduleQuery query)
        {
            var amortizationSchedules = _amortizationScheduleRepository.GetAmortizationSchedule(query);
            return amortizationSchedules.Select(x => AmortizationScheduleMapper.ToViewModel(x)).ToList();
        }

    }
}
