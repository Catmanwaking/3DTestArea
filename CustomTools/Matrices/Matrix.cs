using Space;
using System;

namespace Matrices
{
    /// <summary>
    /// Provides methods for creating and manipulating Matrices.
    /// </summary>
    public class Matrix
    {
        #region Variables

        /// <summary>
        /// A two-dimensional array of flaoting-point numbers.
        /// </summary>
        public double[,] Mtrx { get; set; }

        /// <summary>
        /// The amount of rows of the matrix.
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// The amount of columns of the matrix.
        /// </summary>
        public int Columns { get; }

        /// <summary>
        /// Matrix is in reduced row-echelon form.
        /// </summary>
        public bool IsRREF { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a matix of the specified size.
        /// </summary>
        /// <param name="rows">Amount of Rows.</param>
        /// <param name="columns">Amount of Columns.</param>
        public Matrix(int rows, int columns)
        {
            Mtrx = new double[rows, columns];
            Rows = rows;
            Columns = columns;
        }

        /// <summary>
        /// Creates a 3x1 matrix from a vector.
        /// </summary>
        /// <param name="v">A vector.</param>
        public Matrix(Vect3D v)
        {
            Mtrx = new double[3, 1];
            Rows = 3;
            Columns = 1;
            Mtrx[0, 0] = v.X;
            Mtrx[1, 0] = v.Y;
            Mtrx[2, 0] = v.Z;
        }

        /// <summary>
        /// Creates a 2x3 linear equation matrix from the given three dimensional lines.
        /// </summary>
        /// <param name="line1">First three dimensional line.</param>
        /// <param name="line2">Second three dimensional line.</param>
        /// <param name="useLower">Use X and Z coordinates for Matrix.</param>
        public Matrix(Line3D line1, Line3D line2, bool useLower = false)
        {
            Mtrx = new double[2, 3];
            Rows = 2;
            Columns = 3;
            Mtrx[0, 0] = line1.DirectionVector.X;
            Mtrx[0, 1] = -line2.DirectionVector.X;
            Mtrx[0, 2] = line2.SupportVector.X - line1.SupportVector.X;
            if (!useLower)
            {
                Mtrx[1, 0] = line1.DirectionVector.Y;
                Mtrx[1, 1] = -line2.DirectionVector.Y;
                Mtrx[1, 2] = line2.SupportVector.Y - line1.SupportVector.Y;
            }
            else
            {
                Mtrx[1, 0] = line1.DirectionVector.Z;
                Mtrx[1, 1] = -line2.DirectionVector.Z;
                Mtrx[1, 2] = line2.SupportVector.Z - line1.SupportVector.Z;
            }
        }

        /// <summary>
        /// Creates a 3x4 linear equation matrix from the given three dimensional plane and line.
        /// </summary>
        /// <param name="plane">Three dimensional plane.</param>
        /// <param name="line">Three dimensional line.</param>
        public Matrix(Plane3D plane, Line3D line)
        {
            Mtrx = new double[3, 4];
            Rows = 3;
            Columns = 4;
            Mtrx[0, 0] = plane.DirectionVectors[0].X;
            Mtrx[0, 1] = plane.DirectionVectors[1].X;
            Mtrx[0, 2] = -line.DirectionVector.X;
            Mtrx[0, 3] = line.SupportVector.X - plane.SupportVector.X;
            Mtrx[1, 0] = plane.DirectionVectors[0].Y;
            Mtrx[1, 1] = plane.DirectionVectors[1].Y;
            Mtrx[1, 2] = -line.DirectionVector.Y;
            Mtrx[1, 3] = line.SupportVector.Y - plane.SupportVector.Y;
            Mtrx[2, 0] = plane.DirectionVectors[0].Z;
            Mtrx[2, 1] = plane.DirectionVectors[1].Z;
            Mtrx[2, 2] = -line.DirectionVector.Z;
            Mtrx[2, 3] = line.SupportVector.Z - plane.SupportVector.Z;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Swaps one row of the matrix with another.
        /// </summary>
        /// <param name="row1">First row.</param>
        /// <param name="row2">Second row.</param>
        public void SwapRows(int row1,int row2)
        {
            if (row1 == row2) return;
            double tmp;           
            for(int i = 0; i < Columns; i++)
            {
                tmp = Mtrx[row1, i];
                Mtrx[row1, i] = Mtrx[row2, i];
                Mtrx[row2, i] = tmp;
            }
        }

        /// <summary>
        /// Multiplies a row of the matrix by a number.
        /// </summary>
        /// <param name="row">Row to be Multiplied.</param>
        /// <param name="num">Multiplier.</param>
        private void MultiplyRow(int row, double num)
        {
            for (int i = 0; i < Columns; i++)
            {
                Mtrx[row, i] *= num;
            }
        }

        /// <summary>
        /// Divides a row of the matrix by a number.
        /// </summary>
        /// <param name="row">Row to be divided.</param>
        /// <param name="num">Divisor.</param>
        private void DivideRow(int row, double num)
        {
            for(int i = 0; i < Columns; i++)
            {
                Mtrx[row, i] /= num;
            }
        }

        /// <summary>
        /// Subtracts one row of the matrix from another.
        /// </summary>
        /// <param name="fromRow">Row to be subtracted from.</param>
        /// <param name="subtractionRow">Row used for subtraction.</param>
        /// <param name="times">Multiplier of subtractionRow.</param>
        private void SubtractRowFromRow(int fromRow, int subtractionRow, double times)
        {
            for (int i = 0; i < Columns; i++)
            {
                Mtrx[fromRow, i] -= Mtrx[subtractionRow, i] * times;
            }
        }

        /// <summary>
        /// Turns the matrix into reduced row-echelon form.
        /// </summary>
        public void ToRREF()
        {
            IsRREF = true;
            int i;
            int lead = 0;
            for(int r = 0; r < Rows; r++)
            {
                if (Columns <= lead) break;
                i = r;
                while(Mtrx[i,lead] == 0.0)
                {
                    i++;
                    if(Rows == i)
                    {
                        i = r;
                        lead++;
                        if (Columns == lead) return;
                    }
                }
                SwapRows(i, r);
                if(Mtrx[r, lead] != 0.0) DivideRow(r, Mtrx[r, lead]);
                for(i = 0; i < Rows; i++)
                {
                    if(i != r)
                    {
                        SubtractRowFromRow(i, r, Mtrx[i, lead]);
                    }
                }
                lead++;
            }
        }

        /// <summary>
        /// Evaluates the linear equation matrix.
        /// </summary>
        /// <returns>Type of solution.</returns>
        public int LinearEquationSolutionType()
        {
            if (!IsRREF || Rows + 1 != Columns) return -2; //unknown
            else if (Mtrx[Rows - 1, Columns - 2] == 0.0 && Mtrx[Rows - 1, Columns - 1] == 0.0) return -1; //infinite solutions
            else if (Mtrx[Rows - 1, Columns - 2] == 0.0 && Mtrx[Rows - 1, Columns - 1] != 0.0) return -1; //no solutions
            else return 1; //exactly one solution
        }

        /// <summary>
        /// Turns a 3x1 matrix into a vector.
        /// </summary>
        /// <returns>Vector represented by the matrix.</returns>
        public Vect3D ToVector()
        {
            Vect3D v = new Vect3D();
            if (Rows == 3 && Columns == 1)
            {
                v.X = Mtrx[0, 0];
                v.Y = Mtrx[1, 0];
                v.Z = Mtrx[2, 0];
            }
            return v;
        }

        #endregion

        #region Static functions

        /// <summary>
        /// Creates a multiplicative identity matrix of the specified size.
        /// </summary>
        /// <param name="size">Size of the identity matrix.</param>
        /// <returns>The identity matrix.</returns>
        public static Matrix CreateMultiplicativeIdentityMatrix(int size)
        {
            Matrix identityMatrix = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                identityMatrix.Mtrx[i, i] = 1.0;
            }
            return identityMatrix;
        }

        /// <summary>
        /// Creates an additive identity matrix of the specified size.
        /// </summary>
        /// <param name="size">Size of the identity matrix.</param>
        /// <returns>The identity matrix.</returns>
        public static Matrix CreateAdditiveIdentityMatrix(int size)
        {
            return new Matrix(size, size);           
        }

        /// <summary>
        /// Creates a rotation matrix of the given parameters.
        /// </summary>
        /// <param name="unitVector">Rotation axis.</param>
        /// <param name="angle">Rotation angle in radians.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix CreateRotationMatrix(Vect3D unitVector, double angle)
        {          
            if (unitVector.GetLength() != 1) return null; //UNCHECKED this may cause problems
            if (angle == 0) return CreateMultiplicativeIdentityMatrix(3);
            Matrix m = new Matrix(3, 3);

            //https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
            m.Mtrx[0, 0] = Math.Cos(angle) + unitVector.X * unitVector.X * (1 - Math.Cos(angle));
            m.Mtrx[0, 1] = unitVector.X * unitVector.Y * (1 - Math.Cos(angle)) - unitVector.Z * Math.Sin(angle);
            m.Mtrx[0, 2] = unitVector.X * unitVector.Z * (1 - Math.Cos(angle)) + unitVector.Y * Math.Sin(angle);
            m.Mtrx[1, 0] = unitVector.Y * unitVector.X * (1 - Math.Cos(angle)) + unitVector.Z * Math.Sin(angle);
            m.Mtrx[1, 1] = Math.Cos(angle) + unitVector.Y * unitVector.Y * (1 - Math.Cos(angle));
            m.Mtrx[1, 2] = unitVector.Y * unitVector.Z * (1 - Math.Cos(angle)) - unitVector.X * Math.Sin(angle);
            m.Mtrx[2, 0] = unitVector.Z * unitVector.X * (1 - Math.Cos(angle)) - unitVector.Y * Math.Sin(angle);
            m.Mtrx[2, 1] = unitVector.Z * unitVector.Y * (1 - Math.Cos(angle)) + unitVector.X * Math.Sin(angle);
            m.Mtrx[2, 2] = Math.Cos(angle) + unitVector.Z * unitVector.Z * (1 - Math.Cos(angle));

            return m;
        }

        #endregion

        #region Overloaded operators

        //Matrix scalar multiplication
        public static Matrix operator *(Matrix matrix, double scalar)
        {
            for(int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Columns; col++)
                {
                    matrix.Mtrx[row, col] *= scalar;
                }
            }
            return matrix;
        }

        //Matrix addition
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if (!(matrix1.Rows == matrix2.Rows && matrix1.Columns == matrix2.Columns))
                throw new MatrixDimensionMismatchException("The matrices cannot be added due to unfit dimension sizes");

            for (int row = 0; row < matrix1.Rows; row++)
            {
                for (int col = 0; col < matrix1.Columns; col++)
                {
                    matrix1.Mtrx[row, col] += matrix2.Mtrx[row, col];
                }
            }
            return matrix1;
        }

        //Matrix multiplication
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Columns!= matrix2.Rows)
                throw new MatrixDimensionMismatchException("The matrices cannot be multiplied due to unfit dimension sizes");

            Matrix matrix3 = new Matrix(matrix1.Rows, matrix2.Columns);
            double tmp;

            for (int m1row = 0; m1row < matrix1.Rows; m1row++)
            {
                for (int m2col = 0; m2col < matrix2.Columns; m2col++)
                {
                    tmp = 0;
                    for (int colRow = 0; colRow < matrix1.Columns; colRow++)
                    {
                        tmp += matrix1.Mtrx[m1row, colRow] * matrix2.Mtrx[colRow, m2col];
                    }
                    matrix3.Mtrx[m1row, m2col] = tmp;
                }
            }
            return matrix3;
        }
        #endregion
    }
}
