using System.Linq;
using UnityEngine;

public class Points {
  public static Color[] GENRE_TO_COLOR = new Color[10]{
    new Color(0 / 256f, 171 / 256f, 214 / 256f),
    new Color(0 / 256f, 216 / 256f, 133 / 256f),
    new Color(137 / 256f, 91 / 256f, 0 / 256f),
    new Color(173 / 256f, 166 / 256f, 145 / 256f),
    new Color(177 / 256f, 249 / 256f, 114 / 256f),
    new Color(150 / 256f, 200 / 256f, 255 / 256f),
    new Color(219 / 256f, 43 / 256f, 0 / 256f),
    new Color(198 / 256f, 198 / 256f, 198 / 256f),
    new Color(125 / 256f, 0 / 256f, 188 / 256f),
    new Color(0 / 256f, 38 / 256f, 255 / 256f),
  };

  public int[] pointsGenre;
  public int sum;
  public float[] cumulativeParcentage;
  
  public Points() {
    pointsGenre = new int[10]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    cumulativeParcentage = new float[10]{0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
  }

  public void Add(int genre, int point) {
    pointsGenre[genre] += point;
    sum = pointsGenre.Sum();
    int temp = 0;
    for(int i=0;i<10;i++) {
      temp += pointsGenre[i];
      cumulativeParcentage[i] = temp / (float)sum;
    }
  }
}