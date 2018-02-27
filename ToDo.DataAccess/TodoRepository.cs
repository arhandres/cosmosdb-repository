using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain;

namespace ToDo.DataAccess
{
    public class TodoRepository : BaseRepositoryCosmos<TodoItem>, ITodoRepository
    {
        public void CreateTodoItem(TodoItem todoItem)
        {
            this.Upsert(todoItem);
        }

        public void DeleteTodoItem(string id)
        {
            this.Delete(id);
        }

        public TodoItem GetTodoItemById(string id)
        {
            var item = this.GetFirstOrDefault(t => t.Id == id);
            return item;
        }

        public List<TodoItem> GetTodoItems()
        {
            var items = this.GetAll();
            return items;
        }

        public void UpdateTodoItem(TodoItem todoItem)
        {
            this.Upsert(todoItem);
        }
    }
}
