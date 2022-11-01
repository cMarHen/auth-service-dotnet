using auth_account.Models;
using auth_account.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace AuthServiceTest;

public class JwtHandlerTest
{
  private readonly JwtHandler jwtHandler;

  public JwtHandlerTest()
  {
    // Setup config. TODO: Make this instancation only once
    var builder = WebApplication.CreateBuilder();
    ConfigurationManager config = builder.Configuration;

    this.jwtHandler = new JwtHandler(config);
  }

  [Fact]
  public void getTokenNotNull()
  {   
    var account = new Account("test", "test");

    var token = jwtHandler.getToken(account);
    Assert.NotNull(token);
  }

  [Fact]
  public void getTokenIsString()
  {   
    var account = new Account("test", "test");

    var token = jwtHandler.getToken(account);
    Assert.IsType<String>(token);
  }

  [Theory]
  [InlineData(300)]
  public void getTokenIsOfLength(int length)
  {   
    var account = new Account("test", "test");

    var token = jwtHandler.getToken(account);
    Assert.True(token.Length > length);
  }

  [Fact]
  public void getTokenShouldThrow()
  {
    var account = new Account();

    Assert.Throws<Exception>(() => jwtHandler.getToken(account));
  }
}