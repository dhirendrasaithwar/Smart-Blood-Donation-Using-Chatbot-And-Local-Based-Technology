namespace Repository
{
    public class Role
    {
        public long RoleId { get; set; }
        public string RoleType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<UserRole> USERROLE { get; set;}
    }
}
