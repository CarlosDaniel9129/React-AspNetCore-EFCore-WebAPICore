using AlunosApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public interface IAlunoService
    {
        // -não possui implementações, apenas contratos com assinaturas metodos que devem ser criados ao implementar a interface
        Task<IEnumerable<Aluno>> GetAlunos();
        Task<Aluno> GetAluno(int id);
        Task<IEnumerable<Aluno>> GetAlunosByName(string nome);
        Task CreateAluno(Aluno aluno);
        Task UpdateAluno(Aluno aluno);
        Task DeleteAluno(Aluno aluno);
    }
}
