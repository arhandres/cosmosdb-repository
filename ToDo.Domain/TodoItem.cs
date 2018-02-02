using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain
{
    public class TodoItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
