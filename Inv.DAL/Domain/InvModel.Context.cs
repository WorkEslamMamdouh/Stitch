﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inv.DAL.Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class InvEntities : DbContext
    {
        public InvEntities()
            : base("name=InvEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<A_D_VAT_TYPE> A_D_VAT_TYPE { get; set; }
        public virtual DbSet<A_PERIOD> A_PERIOD { get; set; }
        public virtual DbSet<G_AlertControl> G_AlertControl { get; set; }
        public virtual DbSet<G_AlertLog> G_AlertLog { get; set; }
        public virtual DbSet<G_AlertType> G_AlertType { get; set; }
        public virtual DbSet<G_BRANCH> G_BRANCH { get; set; }
        public virtual DbSet<G_Codes> G_Codes { get; set; }
        public virtual DbSet<G_COMPANY> G_COMPANY { get; set; }
        public virtual DbSet<G_CONTROL> G_CONTROL { get; set; }
        public virtual DbSet<G_Currency> G_Currency { get; set; }
        public virtual DbSet<G_ModuleHelp> G_ModuleHelp { get; set; }
        public virtual DbSet<G_MODULES> G_MODULES { get; set; }
        public virtual DbSet<G_Nationality> G_Nationality { get; set; }
        public virtual DbSet<G_News> G_News { get; set; }
        public virtual DbSet<G_Noteifications> G_Noteifications { get; set; }
        public virtual DbSet<G_NotificationCompany> G_NotificationCompany { get; set; }
        public virtual DbSet<G_PaperSize> G_PaperSize { get; set; }
        public virtual DbSet<G_REGION> G_REGION { get; set; }
        public virtual DbSet<G_REGION_BRANCH> G_REGION_BRANCH { get; set; }
        public virtual DbSet<G_ReportWebSetting> G_ReportWebSetting { get; set; }
        public virtual DbSet<G_Role> G_Role { get; set; }
        public virtual DbSet<G_RoleModule> G_RoleModule { get; set; }
        public virtual DbSet<G_RoleUsers> G_RoleUsers { get; set; }
        public virtual DbSet<G_SearchForm> G_SearchForm { get; set; }
        public virtual DbSet<G_SearchFormModule> G_SearchFormModule { get; set; }
        public virtual DbSet<G_SearchFormSetting> G_SearchFormSetting { get; set; }
        public virtual DbSet<G_SUB_SYSTEMS> G_SUB_SYSTEMS { get; set; }
        public virtual DbSet<G_SYSTEM> G_SYSTEM { get; set; }
        public virtual DbSet<G_TransCounter> G_TransCounter { get; set; }
        public virtual DbSet<G_TransCounterSetting> G_TransCounterSetting { get; set; }
        public virtual DbSet<G_USER_BRANCH> G_USER_BRANCH { get; set; }
        public virtual DbSet<G_USER_COMPANY> G_USER_COMPANY { get; set; }
        public virtual DbSet<G_USER_LOG> G_USER_LOG { get; set; }
        public virtual DbSet<G_USERS> G_USERS { get; set; }
        public virtual DbSet<G_VatNature> G_VatNature { get; set; }
        public virtual DbSet<I_Control> I_Control { get; set; }
        public virtual DbSet<I_D_CURRENCY> I_D_CURRENCY { get; set; }
        public virtual DbSet<HIJRA_CONVERT> HIJRA_CONVERT { get; set; }
        public virtual DbSet<GQ_GetUserBarnchAccess> GQ_GetUserBarnchAccess { get; set; }
        public virtual DbSet<GQ_GetUserBranch> GQ_GetUserBranch { get; set; }
        public virtual DbSet<GQ_GetUserRole> GQ_GetUserRole { get; set; }
        public virtual DbSet<GQ_Notifications> GQ_Notifications { get; set; }
        public virtual DbSet<GQ_ReportWebSetting> GQ_ReportWebSetting { get; set; }
        public virtual DbSet<I_VW_GetCompStatus> I_VW_GetCompStatus { get; set; }
        public virtual DbSet<Sls_Ivoice> Sls_Ivoice { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Sls_InvoiceDetail> Sls_InvoiceDetail { get; set; }
        public virtual DbSet<I_D_UOM> I_D_UOM { get; set; }
    
        public virtual int G_TOL_GetCounter(string system, Nullable<int> comp, Nullable<int> branch, Nullable<System.DateTime> dt, string trType, ObjectParameter trNo)
        {
            var systemParameter = system != null ?
                new ObjectParameter("System", system) :
                new ObjectParameter("System", typeof(string));
    
            var compParameter = comp.HasValue ?
                new ObjectParameter("Comp", comp) :
                new ObjectParameter("Comp", typeof(int));
    
            var branchParameter = branch.HasValue ?
                new ObjectParameter("Branch", branch) :
                new ObjectParameter("Branch", typeof(int));
    
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var trTypeParameter = trType != null ?
                new ObjectParameter("TrType", trType) :
                new ObjectParameter("TrType", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("G_TOL_GetCounter", systemParameter, compParameter, branchParameter, dtParameter, trTypeParameter, trNo);
        }
    
        [DbFunction("InvEntities", "GFun_Companies")]
        public virtual IQueryable<GFun_Companies_Result> GFun_Companies(string userCode)
        {
            var userCodeParameter = userCode != null ?
                new ObjectParameter("userCode", userCode) :
                new ObjectParameter("userCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GFun_Companies_Result>("[InvEntities].[GFun_Companies](@userCode)", userCodeParameter);
        }
    
        public virtual int G_ProcessTrans(Nullable<int> comp, Nullable<int> branch, string trType, string opMode, Nullable<int> trID, ObjectParameter trNo, ObjectParameter ok)
        {
            var compParameter = comp.HasValue ?
                new ObjectParameter("Comp", comp) :
                new ObjectParameter("Comp", typeof(int));
    
            var branchParameter = branch.HasValue ?
                new ObjectParameter("Branch", branch) :
                new ObjectParameter("Branch", typeof(int));
    
            var trTypeParameter = trType != null ?
                new ObjectParameter("TrType", trType) :
                new ObjectParameter("TrType", typeof(string));
    
            var opModeParameter = opMode != null ?
                new ObjectParameter("OpMode", opMode) :
                new ObjectParameter("OpMode", typeof(string));
    
            var trIDParameter = trID.HasValue ?
                new ObjectParameter("TrID", trID) :
                new ObjectParameter("TrID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("G_ProcessTrans", compParameter, branchParameter, trTypeParameter, opModeParameter, trIDParameter, trNo, ok);
        }
    
        public virtual ObjectResult<Prnt_Quotation_Result> Prnt_Quotation(Nullable<int> comp, Nullable<int> bra, string compNameA, string compNameE, string braNameA, string braNameE, string loginUser, Nullable<int> repType, Nullable<int> tRId)
        {
            var compParameter = comp.HasValue ?
                new ObjectParameter("comp", comp) :
                new ObjectParameter("comp", typeof(int));
    
            var braParameter = bra.HasValue ?
                new ObjectParameter("bra", bra) :
                new ObjectParameter("bra", typeof(int));
    
            var compNameAParameter = compNameA != null ?
                new ObjectParameter("CompNameA", compNameA) :
                new ObjectParameter("CompNameA", typeof(string));
    
            var compNameEParameter = compNameE != null ?
                new ObjectParameter("CompNameE", compNameE) :
                new ObjectParameter("CompNameE", typeof(string));
    
            var braNameAParameter = braNameA != null ?
                new ObjectParameter("BraNameA", braNameA) :
                new ObjectParameter("BraNameA", typeof(string));
    
            var braNameEParameter = braNameE != null ?
                new ObjectParameter("BraNameE", braNameE) :
                new ObjectParameter("BraNameE", typeof(string));
    
            var loginUserParameter = loginUser != null ?
                new ObjectParameter("LoginUser", loginUser) :
                new ObjectParameter("LoginUser", typeof(string));
    
            var repTypeParameter = repType.HasValue ?
                new ObjectParameter("RepType", repType) :
                new ObjectParameter("RepType", typeof(int));
    
            var tRIdParameter = tRId.HasValue ?
                new ObjectParameter("TRId", tRId) :
                new ObjectParameter("TRId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Prnt_Quotation_Result>("Prnt_Quotation", compParameter, braParameter, compNameAParameter, compNameEParameter, braNameAParameter, braNameEParameter, loginUserParameter, repTypeParameter, tRIdParameter);
        }
    }
}