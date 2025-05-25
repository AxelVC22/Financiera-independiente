using Independiente.DataAccess.Repositories;
using Independiente.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independiente.Services.Mappers;
using System.Globalization;
using Independiente.Model;
using System.Windows.Forms.VisualStyles;

namespace Independiente.Services
{

    public interface IPaymentService
    {
        int CountPayments(PaymentQuery query);
        List<Model.Payment> GetPayments(PaymentQuery query);

        bool GenerateLayout(string path, ChargeQuery query);

        int UploadCharges(string path, Independiente.Model.Payment payment);

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

        public static void ExportToCSV(List<ChargeView> datos, string rutaArchivo)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CLABE,NombreCliente,Banco,FechaCobro,PagoFijo,IdCredito");

            foreach (var item in datos)
            {
                var linea = string.Format(
                    "{0},{1},{2},{3},{4},{5}",
                    item.CLABE,
                    EscapeCsv(item.ClientName + " " + item.Lastname + " " + item.Surname),
                    EscapeCsv(item.BankName),
                    item.PaymentDate.ToString("yyyy-MM-dd"), 
                    item.FixedPayment.ToString("F2", CultureInfo.InvariantCulture),
                    item.CreditId.ToString()
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

        public bool GenerateLayout(string path, ChargeQuery query)
        {
            bool result = false;
            List<ChargeView> charges = _paymentRepository.GetCharges(query);

            if (charges != null && charges.Count > 0)
            {
                string fileName = System.IO.Path.Combine(path, "Layout.csv");
                ExportToCSV(charges, fileName);
                result = true;
            }
            else
            {
                throw new ArgumentException("No hay datos para generar el layout");
            }

            return result;
        }




        public List<ChargeView> ReadCharges(string rutaCsv)
        {
            var charges = new List<ChargeView>();

            var lineas = System.IO.File.ReadLines(rutaCsv).ToList();
            if (lineas.Count <= 1) return charges;

            foreach (var linea in lineas.Skip(1))
            {
                var columnas = linea.Split(',');

                if (columnas.Length > 7)
                {
                    throw new ArgumentException("El numero de columnas es mayor al esperado");
                }

                var charge = new ChargeView
                {
                    CLABE = columnas[0],
                    ClientName = columnas[1],
                    BankName = columnas[2],
                    PaymentDate = DateTime.Parse(columnas[3]),
                    FixedPayment = decimal.Parse(columnas[4], CultureInfo.InvariantCulture),
                    CreditId = int.TryParse(columnas[5], out var creditId) ? creditId : 0,
                    Status = columnas[6],
                };

                charges.Add(charge);
            }

            return charges;
        }

        public int UploadCharges(string path, Independiente.Model.Payment payment)
        {
            int result = 0;
            var charges = ReadCharges(path);

            if (charges == null || charges.Count == 0)
            {
                throw new ArgumentException("No hay datos para cargar en este archivo");
            }

            var completedCharges = charges
                .Where(c => string.Equals(c.Status, PaymentStatus.Completed.ToString(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            var actualAmount = completedCharges.Sum(c => c.FixedPayment);
            var actualCredits = completedCharges.Count;

            var newPayment = new Independiente.DataAccess.Payment
            {
                ActualAmount = actualAmount,
                ActualCredits = actualCredits,
                PaymentId = payment.PaymentId,
                UploadDate = DateTime.Now,
                Status = PaymentStatus.Completed.ToString()
            };

            result = _paymentRepository.UploadCharges(charges, newPayment);

            return result;
        }

    }
}

