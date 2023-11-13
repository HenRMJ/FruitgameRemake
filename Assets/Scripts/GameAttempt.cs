public class GameAttempt
{
    public int score;
    public int fruitsCombined;
    public Fruit largestFruit;

    public GameAttempt(int score, int fruitsCombined, Fruit largestFruit)
    {
        this.score = score;
        this.fruitsCombined = fruitsCombined;
        this.largestFruit = largestFruit;
    }

    public int GetScore() => score;
    public int GetFruitsCombined() => fruitsCombined;
    public Fruit GetLargestFruit() => largestFruit;
}
