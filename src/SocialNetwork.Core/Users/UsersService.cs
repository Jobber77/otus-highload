using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Core.Common;

namespace SocialNetwork.Core.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUsersGateway _gateway;
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(UserManager<User> userManager, 
            IUsersGateway gateway, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _gateway = gateway;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Result<User>> CreateUser(User user, string password, CancellationToken token)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            if (identityResult.Succeeded)
            {
                await _unitOfWork.Commit(token);
                return Result.Ok(user);
            }
            
            var failedResult = Result.Ok();
            foreach (var error in identityResult.Errors)
            {
                failedResult.WithError(error.Description);
            }

            return failedResult;
        }
        
        public async Task<User?> GetUser(long id, CancellationToken token)
        {
            return await _gateway.GetUser(id, token);
        }

        public async Task<IReadOnlyCollection<User>> SelectUsers(CancellationToken token, params long[] ids)
        {
            return await _gateway.SelectUsers(token, ids);
        }
        
        
    }
}