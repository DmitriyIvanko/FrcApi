using System.Collections.Generic;
using System.Web.Http;

using Data.Entities;
using Data.Repositories;
using Data.Logic.FaceRecognitionSystem;
using test_webapi.Dto;

namespace test_webapi.Controllers
{
    public class FrsController : ApiController
    {
        [HttpGet]
        [ActionName("get-frs-list")]
        public List<FaceRecognitionSystem> GetFrsList()
        {
            var repository = new FrsRepository();
            var frsList =  repository.GetFrsList();
            return frsList;
        }

        [HttpPost]
        [ActionName("recognize")]
        public UserDto Recognize([FromBody] RecognizeItemDto item)
        {
            var frs = new FaceRecognitionTester();
            var user = frs.TestFromImage(item.FrsId, item.ImageByteArray);
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
            };
        }
    }
}
