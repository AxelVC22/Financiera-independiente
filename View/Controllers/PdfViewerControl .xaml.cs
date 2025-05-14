using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Independiente.View.Controllers
{
    /// <summary>
    /// Lógica de interacción para PdfViewerControl.xaml
    /// </summary>
    public partial class PdfViewerControl : UserControl
    {
        private readonly PdfViewer _viewer;

        public PdfViewerControl()
        {
            InitializeComponent();
            _viewer = new PdfViewer { Dock = System.Windows.Forms.DockStyle.Fill };
            FormsHost.Child = _viewer;
        }

        public static readonly DependencyProperty PdfBytesProperty =
            DependencyProperty.Register(nameof(PdfBytes), typeof(byte[]), typeof(PdfViewerControl),
                new PropertyMetadata(null, OnPdfBytesChanged));

        public byte[] PdfBytes
        {
            get => (byte[])GetValue(PdfBytesProperty);
            set => SetValue(PdfBytesProperty, value);
        }

        private static void OnPdfBytesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PdfViewerControl)d;
            if (e.NewValue is byte[] bytes)
            {
                var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(tempPath, bytes);

                var doc = PdfiumViewer.PdfDocument.Load(tempPath);
                control._viewer.Document?.Dispose();
                control._viewer.Document = doc;



            }
        }
    }
}
