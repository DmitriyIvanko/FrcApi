using System;

namespace Data.Logic.FaceRecognitionSystem
{
    public class MnemonicDescriptionModel
    {
        public string originalDescription;
        public string databaseName;
        public int databaseTestImagesPercent;
        public int databaseTestUsersForOpenTaskPercent;
        public int databaseTrainUsersForThresholdPercent;
        public string preprocessingName;
        public string featureName;
        public string trainName; // to do: создать специальный класс для отображения мнемонического описания относящегося к обучению.
        public int trainMartixLeftDimension;
        public int trainMartixRightDimension;
        public string keepInDatabaseName;
        public string testName;

        private string databaseDescriptionBlock;
        private string preprocessingDescriptionBlock;
        private string featureDescriptionBlock;
        private string trainDescriptionBlock;
        private string keepInDatabaseDescriptionBlock;
        private string testDescriptionBlock;

        public MnemonicDescriptionModel(string _description)
        {
            originalDescription = _description;
            this.parseDescription(_description);
        }

        private void parseDescription(string description)
        {
            parseToDescriptionBlock(description);

            parseDatabaseDesciptionBlock(databaseDescriptionBlock);
            parsePreprocessingDesciptionBlock(preprocessingDescriptionBlock);
            parseFeatureDescriptionBlock(featureDescriptionBlock);
            parseTrainDesciptionBlock(trainDescriptionBlock);
            parseKeepInDatabaseDescriptionBlock(keepInDatabaseDescriptionBlock);
            parseTestDescriptionBlock(testDescriptionBlock);
        }

        // to do: разделить и использовать переиспользуемый метод
        private void parseToDescriptionBlock(string description)
        {
            var databaseIndexStart = description.IndexOf("(") + 1;
            var databaseIndexEnd = description.IndexOf(")");
            databaseDescriptionBlock = description.Substring(databaseIndexStart, databaseIndexEnd - databaseIndexStart);
            description = description.Substring(databaseIndexEnd + 1);

            var preprocessingIndexStart = description.IndexOf("{") + 1;
            var preprocessingIndexEnd = description.IndexOf("}");
            preprocessingDescriptionBlock = description.Substring(preprocessingIndexStart, preprocessingIndexEnd - preprocessingIndexStart);
            description = description.Substring(preprocessingIndexEnd + 1);

            var featureIndexStart = description.IndexOf("[") + 1;
            var featureIndexEnd = description.IndexOf("]");
            featureDescriptionBlock = description.Substring(featureIndexStart, featureIndexEnd - featureIndexStart);
            description = description.Substring(featureIndexEnd + 1);

            var trainIndexStart = description.IndexOf("(") + 1;
            var trainIndexEnd = description.IndexOf(")");
            trainDescriptionBlock = description.Substring(trainIndexStart, trainIndexEnd - trainIndexStart);
            description = description.Substring(trainIndexEnd + 1);

            var keepInDatabaseIndexStart = description.IndexOf("{") + 1;
            var keepInDatabaseIndexEnd = description.IndexOf("}");
            keepInDatabaseDescriptionBlock = description.Substring(keepInDatabaseIndexStart, keepInDatabaseIndexEnd - keepInDatabaseIndexStart);
            description = description.Substring(keepInDatabaseIndexEnd + 1);

            var testIndexStart = description.IndexOf("[") + 1;
            var testIndexEnd = description.IndexOf("]");
            testDescriptionBlock = description.Substring(testIndexStart, testIndexEnd - testIndexStart);
            description = description.Substring(testIndexEnd + 1);
        }

        // to do: сделать переиспользуемым. Вообще код нужно улучшить
        private void parseDatabaseDesciptionBlock(string block)
        {
            var colonIndex = block.IndexOf(":");
            var firstSlashIndex = block.IndexOf("/");
            var secondSlashIndex = block.Substring(firstSlashIndex + 1).IndexOf("/") + firstSlashIndex + 1;

            databaseName = block.Substring(0, colonIndex);
            var databaseTestImagesStr = block.Substring(colonIndex + 1, firstSlashIndex - colonIndex - 1);
            var databaseTestUsersForOpenTaskStr = block.Substring(firstSlashIndex + 1, secondSlashIndex - firstSlashIndex - 1);
            var databaseTrainUsersForThresholdStr = block.Substring(secondSlashIndex + 1);

            Int32.TryParse(databaseTestImagesStr, out databaseTestImagesPercent);
            Int32.TryParse(databaseTestUsersForOpenTaskStr, out databaseTestUsersForOpenTaskPercent);
            Int32.TryParse(databaseTrainUsersForThresholdStr, out databaseTrainUsersForThresholdPercent);
        }

        private void parsePreprocessingDesciptionBlock(string block)
        {
            if (block == "S") {
                preprocessingName = block;
                return;
            }

            throw new NotImplementedException();
        }

        private void parseFeatureDescriptionBlock(string block)
        {
            if (block == "S" || block == "Brightness")
            {
                featureName = "Brightness";
                return;
            }

            throw new NotImplementedException();
        }

        private void parseTrainDesciptionBlock(string block)
        {
            var colonIndex = block.IndexOf(":");
            var firstSlashIndex = block.IndexOf("/");

            trainName = block.Substring(0, colonIndex);
            var trainMartixLeftDimensionStr = block.Substring(colonIndex + 1, firstSlashIndex - colonIndex - 1);
            var trainMartixRightDimensionStr = block.Substring(firstSlashIndex + 1);

            Int32.TryParse(trainMartixLeftDimensionStr, out trainMartixLeftDimension);
            Int32.TryParse(trainMartixRightDimensionStr, out trainMartixRightDimension);
        }

        private void parseKeepInDatabaseDescriptionBlock(string block)
        {
            if (block == "S")
            {
                keepInDatabaseName = block;
                return;
            }

            throw new NotImplementedException();
        }

        private void parseTestDescriptionBlock(string block)
        {
            testName = block;
        }
    }
}
