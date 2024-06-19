using Microsoft.EntityFrameworkCore;
using DesafioMVC.Models;

namespace DesafioMVC.Context
{
    public class OrganizadorContext : DbContext
    {
        public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options) 
        { 
        
        }
        public DbSet<Tarefa> Tarefas { get; set; }

    }
    
}