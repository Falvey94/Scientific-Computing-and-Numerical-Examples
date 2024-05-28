//Student Name: Patrick Falvey
//Student Number: 113384116

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;

namespace Gauss_Jacobi_method
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                Matrix m = new Matrix(4);                               //Create matrix m. Size 4x4
                Vector b = new Vector(4);                               //Create matrix b. Size 4

                m[0, 0] = 9; m[0, 1] = -2; m[0, 2] = 3; m[0, 3] = 2;    //Feed in values into m at each position in m
                m[1, 0] = 2; m[1, 1] = 8; m[1, 2] = -2; m[1, 3] = 3;
                m[2, 0] = -3; m[2, 1] = 2; m[2, 2] = 11; m[2, 3] = -4;
                m[3, 0] = -2; m[3, 1] = 3; m[3, 2] = 2; m[3, 3] = 10;

                b[0] = 54.5; b[1] = -14; b[2] = 12.5; b[3] = -21;       //Feed in values into b at each position in b

                Console.WriteLine("The matrix is {0}", m);              //Output to the console the matrix m
                Console.WriteLine("The Vector is {0}", b);              //Output to the console the matrix b

                LinSolve l = new LinSolve();                            //Create a new linear solver function l
                Vector ans = l.Solve(m, b);                             //Input both m, b into the linear solver function l

                Console.WriteLine("The Solution to m = b is {0}", ans); //Output the resulting vectore to the console
                
            }
            catch(Exception e)                                          //Exception Handler
            {
                Console.WriteLine("Error encountered:{0}", e.Message);  //If exception id caught in the try element it will output to the console 
            }
            Console.ReadKey();
        }
    }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Matrix
    {
        private double[,] data = new double[3, 3];                      //part (3a) create a new two-dimensional array (matrix) and initialise it with the valuesize of 3 x 3

        public int NumCols                                              //NumCols stores the value of the number of columns in the matrix
        { get { return data.GetLength(1); } }                           //get the value of the number of columns (index 1) from the array

        public int NumRows                                              //NumrRows stores the value of the number of rows in the matrix
        { get { return data.GetLength(0); } }                           //get the value of the number of rows (index 0) from the array

        public Matrix()                                                 //part (3b) default constructor which sizes the matrix to 3 by 3 and sets all values to zero
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    this[i, j] = 0;                                     //set all values to zero
        }

        public Matrix(int size)                                         //part (3c) constructor public Matrix int(size)                                
        {
            this.data = new double[size, size];
            if (size > 1)                                               //if statement used to check that matrix size is > 1
            {
                for (int i = 0; i < NumRows; i++)
                    for (int j = 0; j < NumCols; j++)
                        this[i, j] = 0;                                 //set all values to zero
            }
            else
            {
                throw new ArgumentException("Make Sure size of matrix is > 1"); 
            }
        }

        public static Matrix operator *(Matrix left, Matrix right)      //part (3d) Implements matrix multiplication
        {
            if ((left.NumCols == right.NumCols))                        //if statement used to check that matrix sizes are identical
            {
                Matrix tmp = new Matrix(left.NumRows);                  //Create a new Output Matrix of size same as the number of Rows in the left Matrix
                for (int i = 0; i < left.NumRows; i++)
                    for (int j = 0; j < left.NumCols; j++)
                        for (int k = 0; j < left.NumRows; j++)
                            tmp[i, j] = left[i, k] * right[k, j];
                return tmp;                                             //return a matrix of values
            }
            else
            {
                throw new ArgumentException("matrices are not the same size");
            }
        }

        public static Vector operator *(Matrix left, Vector right)      //part (3e) Implements multiplication of a matrix by a vector
        {
            if (left.NumCols == right.Size)                             //if statement used to check that matrix sizes are identical       
            {
                Vector tmp = new Vector(right.Size);                    //Create a new Output Vector of size same as input Vector
                double sum = 0;                                         //Initialise your value for sum
                for (int i = 0; i < left.NumRows; i++)
                {
                    sum = 0;
                    for (int j = 0; j < left.NumCols; j++)
                        sum += right[j] * left[i, j];
                    tmp[i] = sum;
                }
                return tmp;                                             //return a vector of values
            }
            else
            {
                throw new ArgumentException("matrix and vector are not the same size");
            }

        }

        public double this[int row, int col]                            //part (3f) Indexer function to allow users access or set elements of the matrix
        {
            get {
                if (row >=0 && col>=0)                                  //check indices are valid
                {
                    return data[row, col];
                }
                else { throw new ArgumentException("Indices are not valid"); }
                   
            }
            set { data[row, col] = value; }
        }

        public override string ToString()                               //part (3g) Returns a string representation of the matrix for use in Console.WriteLine.
        {
            string tmp = "\n";                                          //Create String tmp and give it an initial value
            for (int i = 0; i < NumRows; i++)
            {
                tmp += "{";
                for (int j = 0; j < NumCols; j++)
                {
                    tmp += data[i, j].ToString();                       //Convert the value at the position i,j to a string
                    if (j!=NumCols-1)                                   //If iteration gets to last value do not add a comma
                        tmp += ", ";
                }
                tmp += "}\n";                                           //Add a close bracket to the string
            }
            return tmp;
        }

        public static Matrix operator +(Matrix left, Matrix right)      //Function Implements addition of a two matrices
        {
            Matrix tmp = new Matrix(left.NumRows);                      //Create a new Output Matrix of size same as the left input Matrix
            for (int i = 0; i < left.NumRows; i++)
                for (int j = 0; j < left.NumCols; j++)
                    tmp[i, j] = left[i, j] + right[i, j];
            return tmp;                                                 //return a matrix of values
        }

        public Matrix GetInvD()                                         //Function Implements the creation of a transpose matrix D of the input matix
        {
            Matrix tmp = new Matrix(NumRows);                           //Create a new Output Matrix of size same as the input Matrix
            for (int i = 0; i < NumRows; i++)
                tmp[i, i] = 1 / this[i, i];                             //At the position were i=j
            return tmp;                                                 //return a matrix of values
        }
        public Matrix GetLpU()                                          //Function Implements the creation of a Lower and Upper triangles matrix of the input matix
        {
            Matrix tmp = new Matrix(NumCols);                           //Create a new Output Matrix of size same as the input Matrix
            for (int i = 0; i < NumRows; i++)
                for (int j = 0; j < NumCols; j++)
                    if (i != j)                                         //If the i position and j position are not equal (i.e. the transpose)
                        tmp[i, j] = -this[i, j];
            return tmp;                                                 //return a matrix of values
        }
    }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Vector
    {
        private double[] data = new double[3];                          //part (5a) create a new one-dimensional array (matrix) and initialise it with the valuesize of 3
        public int Size { get { return data.Length; } }                 //get the value of the number of values in the array

        public Vector()                                                 //part (5b) default constructor which sizes the vector to 3 sets all values to zero
        {
            for (int i = 0; i < Size; i++)
                data[i] = 0;                                            //set all values to zero
        }

        public Vector(int size)                                         //part (5c) constructor which sizes the vector to 3 sets all values to zero
        {
            this.data = new double[size];
            if (size > 1)                                               //if statement used to check that matrix size is > 1
            {
                for (int i = 0; i < size; i++)
                    data[i] = 0;                                        //set all values to zero
            }
            else
            {
                throw new ArgumentException("Make Sure size of Vector is > 1");
            }
        }

        public static Vector operator +(Vector left, Vector right)      //part (5d) Implements vector addition
        {
            if (left.Size == right.Size)                                //if statement used to check that the vectors are of the same size
            {
                Vector tmp = new Vector(left.Size);                     //Create a new Output Vector of size same as the left input Vector
                for (int i = 0; i < left.Size; i++)
                    tmp[i] = left[i] + right[i];
                return tmp;                                             //return a vector of values
            }
            else
            {
                throw new ArgumentException("Vectors are not the same size");
            }
        }

        public static Vector operator -(Vector left, Vector right)      //part (5e) Implements vector subtraction
        {
            if (left.Size == right.Size)                                //if statement used to check that the vectors are of the same size
            {
                Vector tmp = new Vector(left.Size);                     //Create a new Output Vector of size same as the left input Vector
                for (int i = 0; i < left.Size; i++)
                    tmp[i] = left[i] - right[i];
                return tmp;                                             //return a vector of values
            }
            else
            {
                throw new ArgumentException("Vectors are not the same size");
            }
        }

        public double this[int index]                                   //part (5f) Indexer function to allow users access or set elements of the vector
        {
            get
            {
                return data[index];                                     //returns the value at the value indexed to the user     
            }
            set
            {
                data[index] = value;                                    //Allows the user to set the value at the value indexed
            }
        }

        public double Norm()                                            //part (5g) Implements the norm (abolute value) of a vector.
        {
            double tmp = 0;                                             //Initialise your value for tmp
            for (int i = 0; i < Size; i++)
                tmp += data[i] * data[i];                               //tmp is the square of the value at i (square always returns a +ve value)
            return Math.Sqrt(tmp);                                      //return the root of temp to provide a positive value of the value at i
        }

        public override string ToString()                               //part (5h) Returns a string representation of the vector for use in Console.WriteLine.
        {
            int i;                                                      //Initialise your value for i
            string tmp = "{";                                           //Create String tmp and give it an initial value, open the brakcet
            for (i = 0; i < Size; i++)
            {
                tmp += data[i].ToString("F3");                              //Convert the value at the position i to a string, Format it to 2 decimal places
                tmp += " ";
            }
            tmp += "}";
            return tmp;                                                 //Return the string of all values in the vector
        }
        
    }

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
    class LinSolve
    {
        public Vector Solve(Matrix A, Vector b)                     //Part 7b: Solve to produce a matrix. Input values are a matrix and a vector
        {
            Vector solnp1 = new Vector(b.Size);                     //Intialise 2 new vestors: One to assign the the solution value to
            Vector soln = new Vector(b.Size);                       //the new iteration (solnp1) and the previous iteration (soln)
            Matrix Dinv = A.GetInvD();                              //Dinv is assigned the value of the transpose Matrix A
            Matrix LpU = A.GetLpU();                                //LpU is assigned the value of the Lower and Upper triangles matrix of Matrix A

            int maxiters = 0;                                       //Part 7a: Intialise the interger max iters               
            do
            {               
                solnp1 = Dinv * (LpU * soln) + Dinv * b;            //Assign solnp1 the value of the linear solver  
                if (((solnp1 - soln).Norm()) / (soln.Norm()) < 0.00001) // If the absolute value of the the new vector solution minus the previous vector solution divided tby the previous vector solution is almost zero
                                                                    //we can say that the solution has converged 
                {
                    break;
                }
                if (maxiters > 100)                                 //If maxiters iterates 100 times break out of the loop and suggest that the function failed to converge
                {
                    Console.WriteLine("Failed to converge");
                    break;
                }
                soln = solnp1;
                maxiters++;

            } while (true);
            return solnp1;                                         //return the resulting vector
        }

    }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
}
