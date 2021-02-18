using System;
using System.Net.Http;
using System.Threading.Tasks;
using Area.AcceptanceTests.Constants;
using Area.AcceptanceTests.Models.Requests;
using Area.AcceptanceTests.Models.Responses;
using Area.AcceptanceTests.Utilities;

namespace Area.AcceptanceTests
{
    public class AreaApiClient
    {
        private readonly ApiClient _apiClient;

        private TokensModel? _tokens;

        public TokensModel? Tokens {
            get => _tokens;
            set {
                _tokens = value;
                _apiClient.SetBearer(_tokens?.AccessToken);
            }
        }

        public AreaApiClient()
        {
            var httpClient = new HttpClient {
                BaseAddress = new Uri("http://localhost:8080")
            };
            _apiClient = new ApiClient(httpClient);
        }

        public async Task<ApiResponse<StatusModel>> Register(RegisterModel form) =>
            await _apiClient.PostAsync<StatusModel, RegisterModel>(RouteConstants.Users.Register, form);

        public async Task<ApiResponse<ResponseModel<TokensModel>>> SignIn(SignInModel form) =>
            await _apiClient.PostAsync<ResponseModel<TokensModel>, SignInModel>(RouteConstants.Auth.SignIn, form);

        public async Task<ApiResponse<StatusModel>> DeleteMyUser() =>
            await _apiClient.DeleteAsync<StatusModel>(RouteConstants.Users.DeleteMyUser);

        public async Task<ApiResponse<ResponseModel<UserInformationModel>>> GetMyUser() =>
            await _apiClient.GetAsync<ResponseModel<UserInformationModel>>(RouteConstants.Users.GetMyUser);
    }
}