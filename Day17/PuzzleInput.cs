namespace Day17;

public static class PuzzleInput
{
    public const string Input = "Register A: 28422061\nRegister B: 0\nRegister C: 0\n\nProgram: 2,4,1,1,7,5,1,5,4,2,5,5,0,3,3,0";
}

//B = A & 7     B => 2 //my choice
//B ^= 1        B => 3 //clean seperation of B and C
//C = A >> B    C => next 3 bits
//A >>= B       shift 3 more bits
//B ^= C        3^2=C, C=1
//Out B & 7         2
//A >>= 3       6 bits total
//jnz 0

//B always 2
//C always output XOR 3
//both 3 bit, shift in 3 bits at a time

//each counter
//A |= 2
//A << 3
//A |= program[counter] ^ 3
//A << 3
