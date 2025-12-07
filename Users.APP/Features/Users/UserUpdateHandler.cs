using Users.APP.Domain;
using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Users.APP.Features.Users
{
    public class UserUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(10)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required, StringLength(15)]
        public string Password { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [Required]
        [DisplayName("Roles")]
        public List<int> RoleIds { get; set; }
    }

    public class UserUpdateHandler : Service<User>, IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        public UserUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(u => u.UserRoles);
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(u => u.Id != request.Id && u.UserName == request.UserName, cancellationToken))
                return Error("User with the same user name exists!");
            var entity = await Query(false).SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            entity.UserName = request.UserName;
            entity.Password = request.Password;
            entity.IsActive = request.IsActive;
            entity.RoleIds = request.RoleIds;
            await Update(entity, cancellationToken);
            return Success("User updated successfully.", entity.Id);
        }
    }
}
