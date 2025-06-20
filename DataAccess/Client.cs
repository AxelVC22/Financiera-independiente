//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Independiente.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.CreditApplication = new HashSet<CreditApplication>();
            this.File = new HashSet<File>();
        }
    
        public int ClientId { get; set; }
        public int DepositAccountId { get; set; }
        public int WorkCenterId { get; set; }
        public int PersonalDataId { get; set; }
        public int AddressDataId { get; set; }
        public Nullable<int> FirstReference { get; set; }
        public Nullable<int> SecondReference { get; set; }
        public int EmployeeId { get; set; }
        public int PaymentAccountId { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
        public virtual AddressData AddressData { get; set; }
        public virtual PersonalData PersonalData { get; set; }
        public virtual WorkCenter WorkCenter { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditApplication> CreditApplication { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> File { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual Reference Reference1 { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
