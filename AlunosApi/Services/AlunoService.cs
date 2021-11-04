using AlunosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AlunoService : IAlunoService // -forma mais correta de trabalhar com dependencias
    {
        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            throw new NotImplementedException();
        }

        public async Task<Aluno> GetAluno(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Aluno>> GetAlunosByName(string nome)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAluno(Aluno aluno)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAluno(Aluno aluno)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAluno(Aluno aluno)
        {
            throw new NotImplementedException();
        }

    }
}
