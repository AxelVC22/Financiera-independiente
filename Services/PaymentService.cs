using Independiente.DataAccess.Repositories;
using Independiente.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independiente.Services.Mappers;
using System.Globalization;

namespace Independiente.Services
{

    public interface IPaymentService
    {
        int CountPayments(PaymentQuery query);
        List<Model.Payment> GetPayments(PaymentQuery query);

        void GenerateLayout(string path, ChargeQuery query);


    }
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        private bool ValidateQuery(PaymentQuery query)
        {

            if ((query.ToDate != null && query.FromDate != null) && (query.FromDate > query.ToDate))
            {
                throw new ArgumentException("Rango de fecha invalido");
            }

           

            return true;
        }

        public int CountPayments(PaymentQuery query)
        {
            int result = 0;
            if (query != null)
            {
                result = _paymentRepository.CountPayments(query);
            }
            return result;
        }

        public List<Model.Payment> GetPayments(PaymentQuery query)
        {
            List<PaymentView> result = new List<PaymentView>();

            List<Model.Payment> payments = new List<Model.Payment>();

            if (ValidateQuery(query))
            {

                result = _paymentRepository.GetPayments(query);

                foreach (var p in result)
                {
                    payments.Add(PaymentMapper.ToViewModel(p));
                }
            }
            return payments;
        }

        public static void ExportarACsv(List<ChargeView> datos, string rutaArchivo)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CLABE,NombreCliente,Banco,FechaCobro,PagoFijo");

            foreach (var item in datos)
            {
                var linea = string.Format(
                    "{0},{1},{2},{3},{4}",
                    item.CLABE,
                    EscapeCsv(item.ClientName + " " + item.Lastname + " " + item.Surname),
                    EscapeCsv(item.BankName),
                    item.PaymentDate.ToString("yyyy-MM-dd"), 
                    item.FixedPayment.ToString("F2", CultureInfo.InvariantCulture)
                );
                sb.AppendLine(linea);
            }

            System.IO.File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeCsv(string valor)
        {
            if (valor.Contains(",") || valor.Contains("\""))
            {
                valor = valor.Replace("\"", "\"\"");
                return $"\"{valor}\"";
            }
            return valor;
        }

        public void GenerateLayout(string path, ChargeQuery query)
        {
            List<ChargeView> charges = _paymentRepository.GetCharges(query);

            if (charges != null && charges.Count > 0)
            {
                string fileName = System.IO.Path.Combine(path, "Layout.csv");
                ExportarACsv(charges, fileName);
            }
            else
            {
                throw new ArgumentException("No hay datos para generar el layout");
            }

        }
    }
}

