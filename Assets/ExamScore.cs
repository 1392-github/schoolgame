[System.Serializable]
public class ExamScore
{
    public int[] average;
    public int[] deviation;
    public int[] rawScore;
    public int[] standardScore;
    public int[] percentile;
    public int[] grade;
    public int[] gradeCut;
    public int[] rank;
    public ExamScore()
    {
        average = new int[5];
        deviation = new int[5];
        rawScore = new int[5];
        standardScore = new int[5];
        percentile = new int[5];
        grade = new int[5];
        gradeCut = new int[40]; // 5개 과목×8개 구분점수(9등급 제외 8개 등급) = 40
        rank = new int[5];
    }
}
