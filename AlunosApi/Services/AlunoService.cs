using AlunosApi.Context;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AlunoService : IAlunoService // -forma mais correta dee trabalhar com depenndencias
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            try
            {
                return await _context.Alunos.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Aluno> GetAluno(int id)
        {
            try
            {
                var aluno = await _context.Alunos.FindAsync(id); // -primeiro consulta na memória depis no banco
                return aluno;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Aluno>> GetAlunosByName(string nome)
        {
            IEnumerable<Aluno> alunos;

            if (!string.IsNullOrEmpty(nome))
            {
                alunos = await _context.Alunos.Where(n => n.Nome.Contains(nome)).ToListAsync();
            }
            else
            {
                alunos = await GetAlunos();
            }

            return alunos;
        }

        public async Task CreateAluno(Aluno aluno)
        {
            _context.Alunos.Add(aluno); // -ate aqui o objeto esta na memoria
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAluno(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified; // -estado modificado
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAluno(Aluno aluno)
        {
            _context.Alunos.Remove(aluno); // -ate aqui o objeto esta na memoria
            await _context.SaveChangesAsync();
        }

    }
}
