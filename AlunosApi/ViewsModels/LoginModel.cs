using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.ViewsModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatório")]
       // [StringLength(5, ErrorMessage = "A senha deve ter 5 caraters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    } 
}
