using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DesafioMVC.Context;
using DesafioMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioMVC.Controllers
{
	public class TarefaController : Controller
	{
		private readonly OrganizadorContext _context;
		
		public TarefaController(OrganizadorContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var tarefas = _context.Tarefas.ToList();
			return View(tarefas);
		}
		[HttpGet]
        public IActionResult Index(string searchAttribute, string searchString, string dateSearchString)
        {
            var tarefas = from t in _context.Tarefas
                          select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchAttribute)
                {
                    case "Id":
                        if (int.TryParse(searchString, out int id))
                        {
                            tarefas = tarefas.Where(t => t.Id == id);
                        }
                        break;
                    case "Titulo":
                        tarefas = tarefas.Where(t => t.Titulo.Contains(searchString));
                        break;
                    case "Descricao":
                        tarefas = tarefas.Where(t => t.Descricao.Contains(searchString));
                        break;
                   
                    case "Status":
                        if (Enum.TryParse<EnumStatusTarefa>(searchString, true, out EnumStatusTarefa status))
                        {
                            tarefas = tarefas.Where(t => t.Status == status);
                        }
                        break;
                }
            }

            if (!String.IsNullOrEmpty(dateSearchString))
            {
                if (DateTime.TryParseExact(dateSearchString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateCriacao))
                {
                    tarefas = tarefas.Where(t => t.Data.Date == dateCriacao.Date);
                }
            }

            return View(tarefas.ToList());
        }

        public IActionResult Criar()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Criar(Tarefa tarefa)
		{
			if (ModelState.IsValid)
			{
				tarefa.Status = EnumStatusTarefa.Pendente;			
				_context.Tarefas.Add(tarefa);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			return View(tarefa);
		}
		public IActionResult Editar(int id)
		{
			var tarefa = _context.Tarefas.Find(id);
			if (tarefa == null)
				return RedirectToAction(nameof(Index));
			return View(tarefa);
		}
		[HttpPost]
		public IActionResult Editar(Tarefa tarefa)
		{

			var tarefaBanco = _context.Tarefas.Find(tarefa.Id);

			tarefaBanco.Titulo = tarefa.Titulo;
			tarefaBanco.Descricao = tarefa.Descricao;
			tarefaBanco.Status = tarefa.Status;
			tarefaBanco.Data= tarefa.Data;
			

			_context.Tarefas.Update(tarefaBanco);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
		public IActionResult Detalhes(int id)
		{
			var tarefa = _context.Tarefas.Find(id);

			if (tarefa == null)
				return RedirectToAction(nameof(Index));
						
			return View(tarefa);
		}
        public IActionResult Deletar(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return RedirectToAction(nameof(Index));

            return View(tarefa);
        }
        [HttpPost]
		public IActionResult Deletar(Tarefa tarefa)
		{
			var tarefaBanco = _context.Tarefas.Find(tarefa.Id);

			_context.Tarefas.Remove(tarefaBanco);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
