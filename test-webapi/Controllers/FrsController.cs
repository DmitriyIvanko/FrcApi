using System;
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

        [HttpGet]
        [ActionName("get-frs-parameter")]
        public FrsParameterDto GetFrsParameter([FromUri] Guid frsId)
        {
            var repository = new FrsRepository();
            var frsList = repository.GetFrsParemeter(frsId);
            return new FrsParameterDto
            {
                AverageImageMatrix = new MatrixStringDto
                {
                    DimentionOne = frsList[0].DimentionOne,
                    DimentionTwo = frsList[0].DimentionTwo,
                    MatrixStringId = frsList[0].MatrixStringId,
                    Value = frsList[0].Value,
                },
                FrsId = frsId,
                LeftMatrix = new MatrixStringDto
                {
                    DimentionOne = frsList[1].DimentionOne,
                    DimentionTwo = frsList[1].DimentionTwo,
                    MatrixStringId = frsList[1].MatrixStringId,
                    Value = frsList[1].Value,
                },
                RightMatrix = new MatrixStringDto
                {
                    DimentionOne = frsList[2].DimentionOne,
                    DimentionTwo = frsList[2].DimentionTwo,
                    MatrixStringId = frsList[2].MatrixStringId,
                    Value = frsList[2].Value,
                }
            };
        }

        [HttpPost]
        [ActionName("recognize")]
        public UserDto Recognize([FromBody] RecognizeItemDto item)
        {
            var frs = new FaceRecognitionTester();
            var user = frs.TestFromImage(item.FrsId, item.ImageByteArray, item.SystemEtalonCount);
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
            };
        }

        [HttpPost]
        [ActionName("by-feature")]
        public UserDto ByFeature([FromBody] RecognizeFeatureDto item)
        {
            var frs = new FaceRecognitionTester();
            var matrixString = new MatrixString
            {
                DimentionOne = item.DimentionOne,
                DimentionTwo = item.DimentionTwo,
                Value = item.FeatureMatrixString,
            };
            var user = frs.TestFromFeature(item.FrsId, matrixString);
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
            };
        }
    }
}
