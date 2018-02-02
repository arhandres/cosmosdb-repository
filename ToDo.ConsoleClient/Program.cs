using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.DataAccess;
using ToDo.Domain;

namespace ToDo.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ITodoRepository todoRepository = new TodoRepository();

            var todoItem = new TodoItem()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Create git repository",
                Description = "Just a description",
                CreatedAt = DateTime.Now,
                IsDone = false,
                Comments = new List<Comment>()
                {
                    new Comment()
                    {
                        Email = "test@mail.com",
                        Message = "PR"
                    }
                }
            };

            todoRepository.CreateTodoItem(todoItem);

            todoItem.IsDone = true;

            todoRepository.UpdateTodoItem(todoItem);

            var reply = todoRepository.GetTodoItems();

            //todoRepository.DeleteTodoItem(todoItem.Id);

            Console.ReadKey();
        }
    }
}
