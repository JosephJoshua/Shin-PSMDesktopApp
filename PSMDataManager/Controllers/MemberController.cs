using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class MemberController : ApiController
    {
        [HttpGet]
        public List<MemberModel> Get()
        {
            MemberData data = new MemberData();
            return data.GetMembers();
        }

        [HttpPost]
        public IHttpActionResult Post(MemberModel member)
        {
            if (string.IsNullOrWhiteSpace(member.Nama))
            {
                return BadRequest("The field 'Nama' cannot be null or empty.");
            }

            MemberData data = new MemberData();
            data.InsertMember(member);

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Put(MemberModel member)
        {
            if (member.Id < 0)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(member.Nama))
            {
                return BadRequest("The field 'Nama' cannot be null or empty");
            }

            MemberData data = new MemberData();
            data.UpdateMember(member);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            MemberData data = new MemberData();
            data.DeleteMember(id);

            return Ok();
        }
    }
}
