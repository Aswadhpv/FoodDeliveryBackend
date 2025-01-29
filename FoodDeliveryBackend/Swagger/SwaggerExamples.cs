using Swashbuckle.AspNetCore.Filters;
using FoodDeliveryBackend.DTOs;

namespace FoodDeliveryBackend.Swagger
{
    public class RegisterDtoExample : IExamplesProvider<RegisterDto>
    {
        public RegisterDto GetExamples()
        {
            return new RegisterDto
            {
                FullName = "John Doe",
                Password = "SecurePassword123!",
                Email = "user@example.com",
                Address = "123 Main St, City, Country",
                BirthDate = DateTime.UtcNow,
                Gender = "Male",
                PhoneNumber = "+1234567890"
            };
        }
    }

    public class TokenResponseExample : IExamplesProvider<TokenResponse>
    {
        public TokenResponse GetExamples()
        {
            return new TokenResponse
            {
                Token = "example-token-12345"
            };
        }
    }

    public class ResponseExample : IExamplesProvider<Response>
    {
        public Response GetExamples()
        {
            return new Response
            {
                Status = "Error",
                Message = "An unexpected error occurred. Please try again later."
            };
        }
    }
}
