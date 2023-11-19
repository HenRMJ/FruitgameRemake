public class GameAttempt
{
    public int score;
    public int fruitsCombined;
    public Fruit largestFruit;
    public GameMode mode;

    public GameAttempt(int score, int fruitsCombined, Fruit largestFruit, GameMode mode)
    {
        this.score = score;
        this.fruitsCombined = fruitsCombined;
        this.largestFruit = largestFruit;
        this.mode = mode;
    }

    public int GetScore() => score;
    public int GetFruitsCombined() => fruitsCombined;
    public Fruit GetLargestFruit() => largestFruit;
}
