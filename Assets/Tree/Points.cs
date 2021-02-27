using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Points {

  public int[] pointsGenre;
  
  public Points() {
    pointsGenre = new int[10];
  }

  public void Add(int genre, int point) {
    pointsGenre[genre] += point;
  }

  public int Sum() {
    return pointsGenre.Sum();
  }

  public float[] Ratio() {
    float[] result = new float[10];
    float sum = (float)Sum();
    int i = 0;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    i ++;
    result[i] = pointsGenre[i] / sum;
    return result;
  }
}