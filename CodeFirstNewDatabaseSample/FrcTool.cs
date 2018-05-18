using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Data.Entities;
using Data.Logic.AddImageDatabase;
using Data.Logic.FaceRecognitionSystem;

namespace Data
{
    class FrcTool
    {
        static void Main(string[] args)
        {
            // string imageDatabaseName = "ORL";
            // string inputDatabaseDir = @"C:\Projects\frc\AT&T_ORL_database_JPEG\";
            // 
            // var dbWriter = new AddImageDatabase(imageDatabaseName, inputDatabaseDir);
            // dbWriter.startAddingDatabase();

            // string description = @"(ORL:30/25/50){S}[Brightness](LDA:11/11){S}[IDTrue:plot(E)]";
            // var md = new MnemonicDescriptionModel(description);
            // var frBuilder = new FaceRecognitionSystemBuilder(md);
            // var frsId = frBuilder.Create();

            var frsId = Guid.Parse("564F7374-8F5A-E811-9CA1-4CCC6A0F6DCB");
            var tester = new FaceRecognitionTester();
            tester.TestFromDatabase(frsId);

        }
    }
}
