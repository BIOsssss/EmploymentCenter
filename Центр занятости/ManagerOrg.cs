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
    
    public partial class ManagerOrg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ManagerOrg()
        {
            this.Organization = new HashSet<Organization>();
        }
    
        public int ID { get; set; }
        public int ID_User { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int Gender { get; set; }
        public System.DateTime Birthday { get; set; }

        public string Org
        {
            get
            {
                string s = "";
                foreach (var x in Organization)
                {
                    s = x.AbbreviatedName;
                }
                return s;
            }
            set
            {

            }
        }

        public string FIO
        {
            get
            {
                return $"{LastName} {FirstName} {Patronymic}";
            }
        }

        public virtual Genders Genders { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Organization> Organization { get; set; }
    }
}
