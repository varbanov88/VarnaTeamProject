using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CodeIt.Models
{
    //DATABASE

    public class CodeItDbContext : IdentityDbContext<User>
    {
        public CodeItDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public virtual IDbSet<CommentOnGuest> CommentsOnGuest { get; set; }

        public virtual IDbSet<GuestCodeModel> GuestCodes { get; set; }

        public virtual IDbSet<CodeModel> Codes { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public static CodeItDbContext Create()
        {
            return new CodeItDbContext();
        }
    }
}