namespace StudentsHelper.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using StudentsHelper.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo
    {
        private string picturePath;

        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        /// <summary>
        /// Gets or Sets PicturePath.
        /// Important: On set IsPictureValidated value is set to false.
        /// </summary>
        public string PicturePath
        {
            get => this.picturePath;
            set
            {
                this.picturePath = value;
                this.IsPictureValidated = false;
            }
        }

        public bool IsPictureValidated { get; set; }

        [Required]
        public string Name { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
