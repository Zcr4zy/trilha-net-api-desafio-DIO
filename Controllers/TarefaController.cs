using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("ObterTodasAsTarefas")]
        public IActionResult ObterTodos() => Ok(_context.Tarefas.ToList()); 

        [HttpGet("ObterTarefa/{id}")]
        public IActionResult ObterPorId(int id)
        {
            Tarefa tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound(new { Erro = "Tarefa não encontrada!" });
            
            return Ok(tarefa);
        }

        [HttpGet("ObterPorTitulo/{titulo}")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            List<Tarefa> tarefas = _context.Tarefas.Where(w => w.Titulo.Contains(titulo)).ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            List<Tarefa> tarefas = _context.Tarefas.Where(w => w.Status == status).ToList();
            return Ok(tarefas);
        }

        [HttpPost("CriarTarefa")]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            if (tarefa == null || String.IsNullOrEmpty(tarefa.Descricao) || String.IsNullOrEmpty(tarefa.Titulo) || tarefa.Status == 0)
                return BadRequest(new { Erro = "Erro! Por favor verifique se os campos foram preenchidos!"});

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("AtualizarTarefa/{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            if (tarefa == null || String.IsNullOrEmpty(tarefa.Descricao) || String.IsNullOrEmpty(tarefa.Titulo) || tarefa.Status == 0)
                return BadRequest(new { Erro = "Erro! Por favor verifique se os campos foram preenchidos!"});

            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Titulo = tarefa.Titulo;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok(tarefaBanco);
        }

        [HttpDelete("DeletarTarefa/{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
