using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // -aciona erros de validação para uma resposta HTTP 400. Possui (FromBody, FromForm etc...)
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    //[Produces("aplication/json")] // -define o formato do retorno da Api 
    public class AlunoController : ControllerBase
    {
        private IAlunoService _alunoService;

        public AlunoController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // -especifica os status de retorno 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Resquest Inválido");
                //return StatusCode(StatusCodes.Status500InternalServerError, "erro ao obter alunos");
            }
        }

        [HttpGet("byname")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByName([FromQuery] string nome)
        {
            try
            {
                var alunos = await _alunoService.GetAlunosByName(nome);

                if (alunos.Count() == 0)
                    return NotFound($"Não existe alunos com o nome: {nome} epecificado");

                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Resquest Inválido");
                //return StatusCode(StatusCodes.Status500InternalServerError, "erro ao obter alunos");
            }
        }
    
        [HttpGet("byid")]
        public async Task<ActionResult<Aluno>> GetAlunoById(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);

                if (aluno == null)
                    return NotFound($"Não existe aluno com o id: {id} epecificado");

                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Resquest Inválido");
                //return StatusCode(StatusCodes.Status500InternalServerError, "erro ao obter alunos");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Aluno aluno) 
        {
            try
            {
                await _alunoService.CreateAluno(aluno);

               // return CreatedAtRoute("byid", new { id = aluno.Id }, aluno); // -obtem o recurso recem criado, retorna 201, usado somente me Post
                return Ok($"Aluno criado com sucesso");
            }
            catch
            {
                return BadRequest("Resquest Inválido");
                //return StatusCode(StatusCodes.Status500InternalServerError, "erro ao obter alunos");
            }
        }

        [HttpPut("{id:int}")] // -altera todos os dados
        public async Task<ActionResult> Update(int id, [FromBody] Aluno aluno)
        {
            try
            {
                if (aluno.Id == id)
                {
                    await _alunoService.UpdateAluno(aluno);
                    return Ok($"Aluno com id: {id} foi atualizado com sucesso");
                }
                else
                {
                    return BadRequest("Dados inconsistentes");
                }

            }
            catch
            {
                return BadRequest("Resquest Inválido");
                //return StatusCode(StatusCodes.Status500InternalServerError, "erro ao obter alunos");
            }
        }

        [HttpDelete("{id:int}")] // -altera todos os dados
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno != null)
                {
                    await _alunoService.DeleteAluno(aluno);
                    return Ok($"Aluno de Id: {id} deletado com sucesso");
                }
                else
                {
                    return NotFound($"Aluno de Id: {id} não encontrado");
                }
            }
            catch
            {
                return BadRequest("Resquest Inválido");
                //return StatusCode(StatusCodes.Status500InternalServerError, "erro ao obter alunos");
            }
        }
    }
}
