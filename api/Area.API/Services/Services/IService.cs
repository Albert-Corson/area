using System;
using System.Threading.Tasks;
using Area.API.Models.Table.Owned;

namespace Area.API.Services.Services
{
    public interface IService
    {
        public int Id { get; }

        public Task<Uri> GetSignInUrlAsync(string state);

        public Task<string?> HandleSignInCallbackAsync(string code);

        public bool SignIn(UserServiceTokensModel tokens);
    }
}