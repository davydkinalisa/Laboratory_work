//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinema_VP
{
    using System;
    using System.Collections.Generic;
    
    public partial class hall
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hall()
        {
            this.hall_showtime = new HashSet<hall_showtime>();
        }
    
        public int hall_id { get; set; }
        public string hall_name { get; set; }
        public string seats { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hall_showtime> hall_showtime { get; set; }
    }
}
