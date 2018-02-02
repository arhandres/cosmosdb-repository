using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain;

namespace ToDo.DataAccess
{
    public interface ITodoRepository
    {
        List<TodoItem> GetTodoItems();
        TodoItem GetTodoItemById(string id);
        void CreateTodoItem(TodoItem todoItem);
        void DeleteTodoItem(string id);
        void UpdateTodoItem(TodoItem todoItem);
    }
}
