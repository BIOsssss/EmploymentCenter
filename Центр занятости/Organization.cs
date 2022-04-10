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
    
    public partial class Organization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organization()
        {
            this.ReferralToWork = new HashSet<ReferralToWork>();
            this.Vacancy = new HashSet<Vacancy>();
        }
    
        public int ID { get; set; }
        public int ID_Manager { get; set; }
        public int TypeOrg { get; set; }
        public string AbbreviatedName { get; set; }
        public string FullNameOrg { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string OGRN { get; set; }
        public System.DateTime DateRegistration { get; set; }
        public string LegalAddress { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    
        public virtual ManagerOrg ManagerOrg { get; set; }
        public virtual TypeOrganization TypeOrganization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferralToWork> ReferralToWork { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancy { get; set; }
    }
}
