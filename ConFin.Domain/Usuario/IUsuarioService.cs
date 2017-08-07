namespace ConFin.Domain.Usuario
{
    public interface IUsuarioService
    {
        void PutSenha(int id, string token, string novaSenha);
    }
}
