//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Центр_занятости
{
    using System;
    using System.Collections.Generic;
    
    public partial class Localities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Localities()
        {
            this.RegistrationAddress = new HashSet<RegistrationAddress>();
        }
    
        public int ID { get; set; }
        public int ID_Region { get; set; }
        public string Name { get; set; }
    
        public virtual Regions Regions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegistrationAddress> RegistrationAddress { get; set; }
    }
}
