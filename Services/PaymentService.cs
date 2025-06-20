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
using System.IO;

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

            sb.AppendLine("CLABE,NombreCliente,Banco,FechaCobro,Monto,Folio");

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

            try
            {
                System.IO.File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);

            }
            catch(IOException e)
            {
                throw new InvalidOperationException("Ocurrió un error al querer generar el archivo, asegurese de tener los permisos necesarios y que no haya instancias abiertas de Layout.csv");
            }
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

            if (query.FromDate == null || query.ToDate == null)
            {
                throw new ArgumentException("Asigne un rango de fechas para poder generar el layout de cobro");
            }

            if (string.IsNullOrEmpty(query.BankName))
            {
                throw new ArgumentException("Asigne un banco para poder generar el layout");
            }

            if (query.FromDate > query.ToDate)
            {
                throw new ArgumentException("El rango de fechas no es apropiado");

            }

            List<ChargeView> charges = _paymentRepository.GetCharges(query);



            if (charges != null && charges.Count > 0)
            {
                string rutaFinal = GetUniqueFileName(path, "Layout", "csv");
                ExportToCSV(charges, rutaFinal);
                result = true;
            }
            else
            {
                throw new ArgumentException("No se encontraron cobros pendientes que cumplan éstas características");
            }

            return result;
        }

        public static string GetUniqueFileName(string rutaBase, string nombreArchivoBase, string extension)
        {
            string rutaCompleta = Path.Combine(rutaBase, $"{nombreArchivoBase}.{extension}");
            int contador = 1;

            while (System.IO.File.Exists(rutaCompleta))
            {
                rutaCompleta = Path.Combine(rutaBase, $"{nombreArchivoBase}({contador}).{extension}");
                contador++;
            }

            return rutaCompleta;
        }





        public List<ChargeView> ReadCharges(string rutaCsv)
        {
            var charges = new List<ChargeView>();

            var lines = System.IO.File.ReadLines(rutaCsv)
                                       .Where(l => !string.IsNullOrWhiteSpace(l))
                                       .ToList();

            if (lines.Count <= 1)
                return charges;

            for (int i = 1; i < lines.Count; i++)
            {
                var line = lines[i];
                var columns = line.Split(',');

                if (columns.Length < 6)
                {
                    throw new ArgumentException($"Línea {i + 1}: columnas insuficientes (esperadas al menos 6, recibidas {columns.Length})");
                }

                try
                {
                    var statusStr = columns.Length > 6 ? columns[6].Trim() : "Pending";

                    if (statusStr != "Completed" && statusStr != "Failed")
                    {
                        throw new ArgumentException($"Línea {i + 1}: el estado '{statusStr}' no es válido. Se espera 'Completed' o 'Failed'.");
                    }

                    var charge = new ChargeView
                    {
                        CLABE = columns[0].Trim(),
                        ClientName = columns[1].Trim(),
                        BankName = columns[2].Trim(),
                        PaymentDate = DateTime.ParseExact(columns[3].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        FixedPayment = decimal.Parse(columns[4].Trim(), CultureInfo.InvariantCulture),
                        CreditId = int.TryParse(columns[5].Trim(), out var creditId) ? creditId : 0,
                        Status = statusStr
                    };

                    charges.Add(charge);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error en línea {i + 1}: \"{line}\". Detalle: {ex.Message}");
                }
            }

            return charges;
        }


        public int UploadCharges(string path, Independiente.Model.Payment payment)
        {
            int result = 0;

            List<ChargeView> charges = new List<ChargeView>();

            try
            {
                charges = ReadCharges(path);

            }
            catch (IOException e)
            {
                throw new InvalidOperationException("Ocurrió un error inesperado al querer leer el archivo. Asegurese de que tiene los permisos necesarios o el archivo no está abierto");
            }

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

