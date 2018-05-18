using MathNet.Numerics.LinearAlgebra;
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
    }
}
