[System.Serializable]
public class TestScore
{
    public string date;
    public int[] grade;
    public int[] rank;
    public TestScore()
    {
        grade = new int[5];
        rank = new int[5];
    }
}
