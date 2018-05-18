using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;

using Data.Entities;

namespace Data.Logic
{
    public static class MatrixHelper
    {
        public static MatrixString convertToMatrixString(Matrix<double> matrix)
        {
            return new MatrixString
            {
                MatrixStringId = Guid.NewGuid(),
                DimentionOne = matrix.RowCount,
                DimentionTwo = matrix.ColumnCount,
                Value = string.Join(Constants.MATRIX_SEPARATOR.ToString(), matrix.ToColumnMajorArray().Select(x => x.ToString())),
            };
        }

        public static DenseMatrix MatrixString2Matrix(Entities.MatrixString matrixString)
        {
            var valStrArray = matrixString.Value.Split(Constants.MATRIX_SEPARATOR);

            double[,] result = new double[matrixString.DimentionOne, matrixString.DimentionTwo];
            for (int i = 0; i < matrixString.DimentionTwo; i++)
            {
                for (int j = 0; j < matrixString.DimentionOne; j++)
                {
                    result[j, i] = Convert.ToDouble(valStrArray[i * matrixString.DimentionOne + j]);
                }
            }

            return DenseMatrix.OfArray(result);
        }
    }
}
