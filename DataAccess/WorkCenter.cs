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
    
    public partial class WorkCenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkCenter()
        {
            this.Client = new HashSet<Client>();
        }
    
        public int WorkCenterId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public System.DateTime HiringDate { get; set; }
        public decimal MontlyIncome { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Client { get; set; }
    }
}
