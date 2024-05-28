//Student Number: 113384116
//Student Name: Patrick Falvey


using System;
using System.Linq; //added to complete the task of outputting rule to the screen

namespace AM6007_Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            uint rule = 0; //initalise rule
            uint step = 0; //initalise step
            uint initial = 0; //initalise initialisation
            uint mask = 0;

            //Declare your variables for collecting information from the user
            string i; //string for reading in value for rule
            string j; //string for reading in value for step
            string k; //string for reading in value for initialisation

            bool success = false; //create boolean variable so that users inputs can be checked so that they are uint values.

            //create a CellularAutomata object
            CellularAutomata ca = new CellularAutomata();


            //Input Section
            //Step 1: Ask user for 'the rule'
            Console.WriteLine("Please enter the rule: any number between 0 and 255");
            i = Console.ReadLine();
            success = uint.TryParse(i, out rule);
            if (!success)//IF success is not a value fit of the traits of uint the program will end. ELSE it will continue.
            {
                Console.WriteLine("Invaild Input:program end");
                return; //ends the program
            }
            //pass the input to the object
            ca.Rule = rule;

            

            //Step 2: Ask user for 'the number of steps'
            Console.WriteLine("Please enter the number of steps: any number between 0 and 200");
            j = Console.ReadLine();
            success = uint.TryParse(j, out step);
            if (!success)//if success is not a value fit of the traits of unit the program will end else it will continue
            {
                Console.WriteLine("Invaild Input:program end");
                return; //ends the program
            }
            //pass the input to the object
            ca.Step = step;

 

            //Step 3: Ask user for 'the type of initialisation'
            Console.WriteLine("Please enter the type of initialisation: 0 for random, 1 for a single non zero entry in the middle");
            k = Console.ReadLine();
            success = uint.TryParse(k, out initial);
            if (!success)//if success is not a value fit of the traits of unit the program will end else it will continue
            {
                Console.WriteLine("Invaild Input:program end");
                return; //ends the program
            }
            //pass the input to the object
            ca.Initial = initial;



            //Ouput Section
            Console.WriteLine("\nYou have Entered Rule:");

            int p = 0;
            uint res = 0;
            uint[] arr = GenerateInitialArray(initial); //Make an save the initial array, taking in the value given by user
            uint[] ruleArr = new uint[8]; // Make and save rule array

            for (p = 0; p < 8; p++)
            {
                mask = 1;
                res = (byte)(rule & mask);

                int p1 = (byte)p;

                String s = Convert.ToString(p, 2); //Converts the iter into it's basform

                //Tool to print bits and result for this rule (for rule to work we add 'using System.Linq' to line 2)
                int[] bits = s.PadLeft(3, '0') //make sure we only use 3 bits
                     .Select(c => int.Parse(c.ToString())) //convert each char typr to int type
                     .ToArray(); //Convert it all to an array

                Console.WriteLine("({0},{1},{2})->{3}", bits[0], bits[1], bits[2], res); //Output the 3 bits of each number i (0->7) any the the value (0 or 1) at the ith position of the bitwise version of the selceted rule.

                ruleArr[p] = res; //add the res the array ruleArr in the position i

                rule = (byte)(rule >> 1);
            }

            // Print the first Array
            String line = ""; //Create an empty string called Line 
            for (int n = 0; n < arr.Length; n++) //loop complies the nth elements of the array arr in the string 'line'.
            {
                line += arr[n]; //output the nth input od the array array as a string
            }
            Console.WriteLine("\nThe initialisation is:");
            Console.WriteLine(line); //Print the initialisation string 'Line'
            Console.WriteLine("The CA is:");
            Console.WriteLine(line); //reprint the initialisation for the run.


            // For the run
            for (int f = 0; f < step; f++) //Generate a loop that will run for the ammount of steps specified by the user to procduce a run that is 'steps' long.
            {
                line = ""; //reuses the string line and sets it to empty
                arr = GenerateNewArray(arr, ruleArr); //arr becomes the next line in the sequence through the function.

                for (int n = 0; n < arr.Length; n++) //loop complies the nth elements of the array arr in the string 'line'.
                {
                    line += arr[n]; //output the nth input od the array array as a string
                }
                Console.WriteLine(line); //Print the string 'Line'
            }

            Console.ReadKey();
        }

        //Function 1: To generate the first array, whether it is 0 (random) or 1(single non zero)
        static uint[] GenerateInitialArray(uint input)
        {
            Random rnd = new Random(); 

            uint[] initilisationArray = new uint[32]; //Create an array that has 1 row and 32 columns

            
            if (input == 0) // IF input for initialisation from user is 0, generate random initialisation
            {
                for (uint i = 0; i < initilisationArray.Length; i++)
                {
                    initilisationArray[i] = (uint)rnd.Next(0, 2); //Input a random number between 0 and 2( not includig 2, so 0 and 1) for each ith position in the loop
                }

                return initilisationArray; //Return out of the function, the complete random initialisation
            }
            
            else //ELSE initialisation is 1, create an initialisation with a single non zero entry in the middle.
            {
                for (uint i = 0; i < initilisationArray.Length; i++)
                {
                    initilisationArray[i] = 0; //for each ith position in the loop generate a zero, generating an array of zeros. 
                }

                initilisationArray[15] = 1; //Change the value at the 15th position in the array from 0 to 1

                return initilisationArray; //Return out of the function, the complete 1 for a single non zeroentry in the middle initialisation
            }

        }
        //Function 2: Generate the cellular automata
        static uint[] GenerateNewArray(uint[] oldArray, uint[] ruleSet)
        {
            uint[] newArray = new uint[oldArray.Length]; //Create an array that is of the same length of old array, arr (the initial array)

            //arr is the line. We need a new line which uses the rule to change each number. 
            // rule for number 5.
            for (int n = 0; n < oldArray.Length; n++)
            {
                int m, o; // m = n - 1, o = n + 1

                m = n - 1; // m is the value to the left of n
                o = n + 1; // o is the value to the right of n

                if (n == 0) m = oldArray.Length - 1; //if n is at the 0th position, m (at position left of n) is equal to the value at the 32th position.
                if (n == oldArray.Length - 1) o = 0; //if n is at the 32th position, o (at position right of n) is equal to the value at the 0th position.

                String rule1 = "" + oldArray[m] + oldArray[n] + oldArray[o]; // Get a string for the rule (e.g. 1, 0, 1)

                uint ruleNum = (uint)Convert.ToInt32(rule1, 2); //Convert the string rule1 into a uint(number) called ruleNum (but using base 2 so 010 would become 2)

                // the value of ruleSet at ruleNum is the new value we are looking for
                newArray[n] = ruleSet[ruleNum]; //The value of  of ruleSet (Which is ruleArr) for the corresponding ruleNum Value, i.e. 010 is 1 (for the given rule of 30)
            }

            return newArray; //Return out of the function, the complete following row.
        }
    }


class CellularAutomata
{
    private uint rule;//specify the rule
    public uint Rule
    {
        get { return rule; }
        set
        {
            if (value >= 0 && value <= 255)
                rule = value;
            else
            {
                Console.WriteLine("Invalid Input:Program End");
                System.Environment.Exit(1); //Terminates this process and gives the underlying operating system the specifies exit code.
            }
        }
    }

    private uint step;//specifie how many steps to take
    public uint Step
    {
        get { return step; }
        set
        {
            if (value >= 0 && value <= 200)
                step = value;
            else
            {
                Console.WriteLine("Invalid Value:Program End");
                System.Environment.Exit(1); //Terminates this process and gives the underlying operating system the specifies exit code.
            }
        }
    }

    private uint initial; //what type of initialisation 0 means one and zeroes, 1 mean random
                          //Constructors
    public uint Initial
    {
        get { return initial; }
        set
        {
            if (value >= 0 && value <= 1)
                initial = value;
            else
            {
                Console.WriteLine("Invalid Value:Program End");
                initial = 0;
                System.Environment.Exit(1);//Terminates this process and gives the underlying operating system the specifies exit code.
            }
        }
    }
}
}


