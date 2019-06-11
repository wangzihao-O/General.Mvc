using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace General.Entities
{
    [Serializable]
    [Table("SysUser")]
    public partial class SysUser
    {
        public SysUser()
        {
            SysUserTokens = new HashSet<SysUserToken>();
            SysUserLoginLogs = new HashSet<SysUserLoginLog>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Account { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        public bool Enabled { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreationTime { get; set; }

        public int LoginFailedNum { get; set; }

        public DateTime? AllowLoginTime { get; set; }

        public bool LoginLock { get; set; }

        public DateTime? LastLoginTime { get; set; }

        [StringLength(50)]
        public string LastIpAddress { get; set; }

        public DateTime? LastActivityTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }

        public Guid? Modifier { get; set; }

        public Guid? Creator { get; set; }

        [Column(TypeName ="image")]
        public byte[] Avator { get; set; }

        public virtual ICollection<SysUserToken> SysUserTokens { get; set; }

        public virtual ICollection<SysUserLoginLog> SysUserLoginLogs { get; set; }
    }
}
