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
    
    public partial class ApplicationOfUnemployed
    {
        public int ID { get; set; }
        public int ID_Applicant { get; set; }
        public Nullable<int> ID_Worker { get; set; }
        public bool Disabled { get; set; }
        public bool EmploymentData { get; set; }
        public System.DateTime DateApplicant { get; set; }
        public System.DateTime TimeApplicant { get; set; }
        public Nullable<int> Status { get; set; }
        public string CommentWorker { get; set; }
        public Nullable<System.DateTime> DateComment { get; set; }
        public Nullable<System.DateTime> TimeComment { get; set; }
    
        public virtual Applicants Applicants { get; set; }
        public virtual StatusUnemployed StatusUnemployed { get; set; }
        public virtual Workers Workers { get; set; }
    }
}
