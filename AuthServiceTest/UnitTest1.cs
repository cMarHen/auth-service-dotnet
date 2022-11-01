using auth_account.Models;
using Microsoft.AspNetCore.Hosting;

namespace AuthServiceTest;

public class UnitTest1
{
    public UnitTest1 ()
    {
        SetupClient();
    }

    private void SetupClient()
    {
        // var server = new TestServer(new WebHosBuilder().UseStartup<Program>);
        var account = new Account();
    }

    [Fact]
    public void Test1()
    {
        Account acc = new Account();
        acc.username = "344sff";
        
        Assert.Equal(acc.username, "344sff");
    }
}