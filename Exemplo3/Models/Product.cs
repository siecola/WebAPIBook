using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exemplo3.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string nome { get; set; }

        public string descricao { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "O tamanho máximo do código é 8 caracteres")]
        public string codigo { get; set; }

        [Range(10, 999, ErrorMessage = "O preço deverá ser entre 10 e 999.")]
        public decimal preco { get; set; }

        [StringLength(80, ErrorMessage = "O tamanho máximo da url é 80 caracteres")]
        public string Url { get; set; }
    }
}