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
    
    public partial class RegistrationAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegistrationAddress()
        {
            this.Applicants = new HashSet<Applicants>();
        }
    
        public int ID { get; set; }
        public int Type { get; set; }
        public Nullable<int> Region { get; set; }
        public Nullable<int> Locality { get; set; }
        public string Street { get; set; }
        public string Flat { get; set; }
        public string PostalCode { get; set; }

        public string Adres
        {
            get
            {
                return $"{Localities.Name}, {Street}, {Flat}";
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Applicants> Applicants { get; set; }
        public virtual Localities Localities { get; set; }
        public virtual Regions Regions { get; set; }
        public virtual RegistrationOfResidence RegistrationOfResidence { get; set; }
    }
}
