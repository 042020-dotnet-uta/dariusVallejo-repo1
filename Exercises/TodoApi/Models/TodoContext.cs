using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Models {
    public class TodoContext : DbContext {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) {
        }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoApi.Models.TodoClassification> TodoClassification { get; set; }
    }
}