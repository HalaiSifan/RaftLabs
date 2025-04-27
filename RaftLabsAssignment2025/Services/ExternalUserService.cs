using RaftLabsAssignment2025.Clients;
using RaftLabsAssignment2025.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLabsAssignment2025.Services
{
    public class ExternalUserService
    {
        private readonly IReqResClient _client;

        public ExternalUserService(IReqResClient client)
        {
            _client = client;
        }

        public Task<User> GetUserByIdAsync(int userId) => _client.GetUserByIdAsync(userId);

        public Task<IEnumerable<User>> GetAllUsersAsync() => _client.GetAllUsersAsync();
    }
}