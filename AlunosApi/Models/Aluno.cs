using System.ComponentModel.DataAnnotations;

namespace AlunosApi.Models
{
    public class Aluno
    {
        // -poderia ser AlunoId que o entity consegue mapear normalmente, porem nada alem disso
        public int Id { get; set; }

        [Required] // -o entity entende com o required que a coluna na base nao pode ser nula
        [StringLength(80)] // -pode-se definir a mesagem de erro
        public string Nome { get; set; }

        [Required]
        [EmailAddress] // -verifica o email
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public int Idade { get; set; }
    }
}
