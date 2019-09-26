using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Testing.TestUsers.Controllers
{
    [Route("_api/TestUser")]
    [AllowAnonymous]
    public class TestUserController : Controller
    {
        [HttpGet]
        [Route("{username}")]
        public IActionResult Get(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            int index = IdentityServer4.Quickstart.UI.TestUsers.Users.FindIndex(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            if (index < 0) return NotFound();

            return Ok(Map(IdentityServer4.Quickstart.UI.TestUsers.Users[index]));
        }
        private TestUserModel Map(IdentityServer4.Test.TestUser testUser)
        {

            TestUserModel testUserModel = new TestUserModel
            {
                SubjectId = testUser.SubjectId,
                Username = testUser.Username,
                Password = testUser.Password,
                Roles = new List<string>()
            };
            foreach (Claim claim in testUser.Claims)
            {
                if (claim.Type == JwtClaimTypes.Name) testUserModel.Name = claim.Value;
                else if (claim.Type == JwtClaimTypes.GivenName) testUserModel.GivenName = claim.Value;
                else if (claim.Type == JwtClaimTypes.FamilyName) testUserModel.FamilyName = claim.Value;
                else if (claim.Type == JwtClaimTypes.Email) testUserModel.Email = claim.Value;
                else if (claim.Type == JwtClaimTypes.Role) testUserModel.Roles.Add(claim.Value);
            }
            return testUserModel;
        }
        [HttpPost]
        public IActionResult Add([FromBody] TestUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityServer4.Test.TestUser testUser = Map(model);

            IdentityServer4.Quickstart.UI.TestUsers.Users.Add(testUser);

            return Ok();
        }
        private IdentityServer4.Test.TestUser Map(TestUserModel model)
        {

            IdentityServer4.Test.TestUser testUser = new IdentityServer4.Test.TestUser
            {
                SubjectId = model.SubjectId,
                Username = model.Username,
                Password = model.Password,
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, model.Name),
                    new Claim(JwtClaimTypes.GivenName, model.GivenName),
                    new Claim(JwtClaimTypes.FamilyName, model.FamilyName),
                    new Claim(JwtClaimTypes.Email, model.Email)
                }
            };
            if (model.Roles != null)
            {
                foreach (string role in model.Roles)
                {
                    testUser.Claims.Add(new Claim(JwtClaimTypes.Role, role));
                }
            }
            return testUser;
        }
        [HttpDelete]
        [Route("{username}")]
        public IActionResult Delete(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            int index = IdentityServer4.Quickstart.UI.TestUsers.Users.FindIndex(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            if (index < 0) return NotFound();

            IdentityServer4.Quickstart.UI.TestUsers.Users.RemoveAt(index);

            return Ok();
        }

    }
}
