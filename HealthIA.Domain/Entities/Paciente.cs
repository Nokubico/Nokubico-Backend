using System;
using HealthIA.Domain.Validation;

namespace HealthIA.Domain.Entities
{
    public class Paciente
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Sexo { get; private set; }
        public string Telefone { get; private set; }
        public int UsuarioId { get; private set; }

        public Paciente(string nome, DateTime dataNascimento, string sexo, string telefone, int usuarioId)
        {
            Validacao(nome, dataNascimento, sexo, telefone, usuarioId);
        }

        private void Validacao(string nome, DateTime dataNascimento, string sexo, string telefone, int usuarioId)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(nome), "Nome do paciente inválido.");
            DomainExceptionValidation.When(dataNascimento.Date > DateTime.Now.Date, "Data de nascimento inválida.");
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(sexo), "Sexo do paciente inválido.");
            DomainExceptionValidation.When(usuarioId < 0, "O id do usuário não pode ser negativo.");

            Nome = nome.Trim();
            DataNascimento = dataNascimento;
            Sexo = sexo;
            Telefone = telefone;
            UsuarioId = usuarioId;
        }
    }
}
