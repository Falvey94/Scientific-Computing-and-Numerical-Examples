//Student Name: Patrick Falvey
//Student Number: 113384116

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Perceptron
{
    class Program
    {
        static void Main(string[] args)
        {
            Perceptron p = new Perceptron(); //Create new perceptron class p
            
            //Provide the 'ReadData' method with the choosen .csv file 'data.csv'
            p.ReadData("C:\\Users\\113384116\\Documents\\Patrick Falvey\\Semester 1\\AM6007\\Assignment 2\\data.csv");
            p.TrainData(); //which finds the optimal weights(decision boundary).

            p.Output();
            Console.ReadKey();
        }
    }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Perceptron
    {
        //Create an inital array for each column of the source data.
        public int[] id;
        public int[] rpm;
        public int[] vibration;
        public int[] status;

        //Create the variable Bias. Bias is always w0 and is constant.
        public double bias = 0.2;

        public Vector WeightVector;

        //Method to read in the choosen .csv file
        public void ReadData(string filepath)
        {
            List<string> lines = File.ReadAllLines(filepath).ToList(); //Implements a text reader for the specified file.
            int size = lines.Count - 1;                                //Create a interger 'size' that counts the number of lines in the file. Account for the column heading, hence the -1.
            lines.RemoveAt(0);                                         //Remove the first line of the of the string 'lines'. This is the column headings.
            id = new int[size];                                        //Give the int array 'id' a size of rows the size of the variable 'size'. i.e[56 1]
            rpm = new int[size];                                       //Give the int array 'rpm' a size of rows the size of the variable 'size'. i.e[56 1]
            vibration = new int[size];                                 //Give the int array 'vibration' a size of rows the size of the variable 'size'. i.e[56 1]
            status = new int[size];                                    //Give the int array 'status' a size of rows the size of the variable 'size'. i.e[56 1]
            string str = string.Empty;                                 //Create an empty string called 'str'
            string[] parts = null;                                     //Create an empty array called 'parts' (type 'string')
            int i = 0;                                                 //Create the initalisation value for the for loop
            for (i = 0; i < size; i++)                                 //For loop that runs each line of the file brought in. Baring the 0th line that was excluded above.
            {
                str = lines[i];                                        //String 'str' takes on the ith line in the List 'lines'
                parts = str.Split(",");                                //'parts' is an array [1 4] splits the string where is comes across the ',' character.
                id[i] = Convert.ToInt32(parts[0]);                     //Assigns the Oth value of the array parts into the ith position of the array 'id'
                rpm[i] = Convert.ToInt32(parts[1]);                    //Assigns the 1st value of the array parts into the ith position of the array 'rpm'
                vibration[i] = Convert.ToInt32(parts[2]);              //Assigns the 2nd value of the array parts into the ith position of the array 'vibration' 
                status[i] = Convert.ToInt32(parts[3]);                 //Assigns the 3rd value of the array parts into the ith position of the array 'status'
            }

        }

        //Method to train data
        public void TrainData()
        {
            Console.WriteLine("Data Training has Commenced");
            Random rand = new Random();                                //Create a new variable 'rand' type rand. Generates a random variable

            double learningRate = 0.1;                                 //Create a new variable 'learningrate' type double.

                                                                       // If the weight vector is correct, then we use this weight vector to classify future points
                                                                       // Randomise the w1 and w2 while w0 is bias
            WeightVector = new Vector(bias, rand.NextDouble(), rand.NextDouble());

            int error = 1;                                             // initially set error to 1 to start the while loop
           
            while (error > 0.1)                                        // train until all data points are within status
            { 
                error = 0;                                             //At the start of each iteration, set error to 0. If it makes it through the loop, 
                                                                       //we will have weights that fit the training set, then we can classify new values. 
                for (int j = 0; j < id.Length ; j++)
                {
                    // We get out output. 

                    // x0 is always 1
                    Vector InputVector = new Vector(1, rpm[j], vibration[j]);
                    double output = WeightVector * InputVector;
                    int classifiedOutput = Activation(output);

                    if (classifiedOutput != status[j])
                    {
                        // Our weights are wrong. Lets update the weights using the update formula, set error and break.
                        // w ← w + α(yi − yˆi)xi 
                        double newX = WeightVector.x + learningRate * (classifiedOutput - status[j]) * InputVector.x;
                        double newY = WeightVector.y + learningRate * (classifiedOutput - status[j]) * InputVector.y;
                        double newZ = WeightVector.z + learningRate * (classifiedOutput - status[j]) * InputVector.z;

                        WeightVector = new Vector(newX, newY, newZ);
                       
                        //Console.ReadKey();
                        error = 1;
                        break;                                          
                    }
                }
            }

            Console.WriteLine("      rpm       ,    Vibration     ,    status");
            WeightVector.print();

            Console.WriteLine("Data Training Complete - Press any key to continue");//Let the user know that Data Training is complete
            Console.ReadKey();

        }
 
        public int ClassifyPoint(double rpm, double vibration)
        {
            Vector InputVector = new Vector(1, rpm, vibration);         //feed the Vector Class the 3 variables.
            double output = WeightVector * InputVector;                 //Multiply the WeightVector (Got from Traindata) and your newly created InputVector.
            int classifiedOutput = Activation(output);                  // Run the result through the Activation Method to determin whether the ouput is 'good' (1) or 'faulty' (0)

            return classifiedOutput;                                    //returns either 1 or 0 as output.
        }

        public void Output()
        {
            Console.WriteLine("      rpm       ,    Vibration     ,    status");                // print the weights
            WeightVector.print();

            //Once the data is trained lets test two points using the classify
            Console.WriteLine("Test 2 points");

            //rpm: 600, vibration: 200 -> this should be below the line in the graph in the pdf. Therefore 0.
            Console.WriteLine("Classifying new point; RPM: 600, Vibration: 200. Expected output: 0");
            Console.WriteLine("Output:"+ ClassifyPoint(600, 200)); // gives 0, correct

            //rpm: 600, vibration: 600 -> this should be above the line in the graph in the pdf. Therefore 1.
            Console.WriteLine("Classifying new point; RPM: 600, Vibration: 600. Expected output: 1");
            Console.WriteLine("Output:" + ClassifyPoint(600, 600)); // gives 1, correct
        }

        int Activation(double x) //Activation Method - Takes input value and returns 0 if result is positive zero else returns 1.
        {
            if (x >= 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Vector
    {
        public double x;
        public double y;
        public double z;                                            //Create 3 variables x,y,z.

        public Vector(double x, double y, double z)
        {
            this.x = x;                                            //Use this to qualify the members of the class instead of the constructor parameters.
            this.y = y;
            this.z = z;
        }
        public static double operator *(Vector v1, Vector v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;        //Calculate the dot product of the incoming v1 and v2
        }

        public void print()
        {
            Console.WriteLine(x + ", " + y + ", " + z);            //Print function to print outputs of the vector class ina single Line.
        }
    }

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
}
