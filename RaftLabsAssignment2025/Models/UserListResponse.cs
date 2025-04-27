using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLabsAssignment2025.Models
{
    public class UserListResponse
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<User> Data { get; set; }
    }
}
