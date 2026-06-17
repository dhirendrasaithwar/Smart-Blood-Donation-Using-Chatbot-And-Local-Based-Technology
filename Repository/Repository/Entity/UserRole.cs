namespace Repository
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual User USER { get; set; }
        public virtual Role ROLE { get; set; }
    }
}
