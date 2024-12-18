namespace Day17;

public static class PuzzleInput
{
    public const string Input = "Register A: 28422061\nRegister B: 0\nRegister C: 0\n\nProgram: 2,4,1,1,7,5,1,5,4,2,5,5,0,3,3,0";
}

//B = A & 7     B => 2 //my choice
//B ^= 1        B => 3 //clean seperation of B and C
//C = A >> B    C => next 3 bits
//B ^= 5
//B ^= C
//Out B & 7 
//A >>= 3
//jnz 0


// out = B ^ 1 ^ 5 ^ C = B ^ 4 ^ C
// c shift = B ^ 1