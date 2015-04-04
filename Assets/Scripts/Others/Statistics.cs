using UnityEngine;

public struct Statistics {
	public const int statMax = 100;

	public Statistics(int artStat, int progStat, int designStat, int soundStat, int likeStat, int dislikeStat) {
		art = artStat;
		programming = progStat;
		design = designStat;
		sound = soundStat;
		like = likeStat;
		dislike = dislikeStat;
	}

	public Statistics(int artStat, int progStat, int designStat, int soundStat) {
		art = artStat;
		programming = progStat;
		design = designStat;
		sound = soundStat;
		like = 50;
		dislike = 50;
	}

	public Statistics(int likeStat, int dislikeStat) {
		art = 0;
		programming = 0;
		design = 0;
		sound = 0;
		like = likeStat;
		dislike = dislikeStat;
	}

	private int art;
	public int Art {
		get { return art; }
		set { art = Mathf.Clamp(value, 0, statMax); }
	}

	private int programming;
	public int Programming {
		get { return programming; }
		set { programming = Mathf.Clamp(value, 0, statMax); }
	}

	private int design;
	public int Design {
		get { return design; }
		set { design = Mathf.Clamp(value, 0, statMax); }
	}

	private int sound;
	public int Sound {
		get { return sound; }
		set { sound = Mathf.Clamp(value, 0, statMax); }
	}

	private int like;
	public int Like{
		get { return like; }
		set { like = Mathf.Clamp(value, 0, statMax); }
	}

	private int dislike;
	public int Dislike {
		get { return dislike; }
		set { dislike = Mathf.Clamp(value, 0, statMax); }
	}

	public static Statistics operator +(Statistics a, Statistics b) {
		return new Statistics(a.art + b.art, a.programming + b.programming, a.design + b.design, a.sound + b.sound, a.like + b.like, a.dislike + b.dislike);
	}

	public static Statistics operator +(Statistics a, int rhs) {
		return new Statistics(a.art + rhs, a.programming + rhs, a.design + rhs, a.sound + rhs, a.like + rhs, a.dislike + rhs);
	}

	public static Statistics operator -(Statistics a) {
		return new Statistics(-a.art, -a.programming, -a.design, -a.sound, -a.like, -a.dislike);
	}

	public static Statistics operator -(Statistics a, Statistics b) {
		return new Statistics(a.art - b.art, a.programming - b.programming, a.design - b.design, a.sound - b.sound, a.like - b.like, a.dislike - b.dislike);
	}

	public static Statistics operator -(Statistics a, int rhs) {
		return new Statistics(a.art - rhs, a.programming - rhs, a.design - rhs, a.sound - rhs, a.like - rhs, a.dislike - rhs);
	}

	public static Statistics operator /(Statistics a, Statistics b) {
		return new Statistics(a.art / b.art, a.programming / b.programming, a.design / b.design, a.sound / b.sound, a.like / b.like, a.dislike / b.dislike);
	}

	public static Statistics operator /(Statistics a, int rhs) {
		return new Statistics(a.art / rhs, a.programming / rhs, a.design / rhs, a.sound / rhs, a.like / rhs, a.dislike / rhs);
	}

	public static Statistics operator *(Statistics a, Statistics b) {
		return new Statistics(a.art * b.art, a.programming * b.programming, a.design * b.design, a.sound * b.sound, a.like * b.like, a.dislike * b.dislike);
	}

	public static Statistics operator *(Statistics a, int rhs) {
		return new Statistics(a.art * rhs, a.programming * rhs, a.design * rhs, a.sound * rhs, a.like * rhs, a.dislike * rhs);
	}

	public static bool operator !=(Statistics lhs, Statistics rhs) {
		return !Statistics.Equals(lhs, rhs);
	}

	public static bool operator ==(Statistics lhs, Statistics rhs) {
		return Statistics.Equals(lhs, rhs);
	}

	public static Statistics GetAverage(Statistics[] stats, int length) {
		Statistics totalStat = new Statistics();
		Statistics result = new Statistics();
		int statLength = length;
		for (int i = 0; i < stats.Length; i++) {
			totalStat = totalStat + stats[i];
		}
		result = totalStat / statLength;
		return result;
	}

	public static Statistics zero {
		get { return new Statistics(0, 0, 0, 0, 0, 0); }
	}

	public static Statistics one {
		get { return new Statistics(1, 1, 1, 1, 1, 1); }
	}

	public static Statistics dislikeStat {
		get { return new Statistics(0, 0, 0, 0, 0, 1); }
	}

	public static Statistics likeStat {
		get { return new Statistics(0, 0, 0, 0, 1, 0); }
	}

	public void ClampAllValues() {
		art = Mathf.Clamp(art, 0, statMax);
		programming = Mathf.Clamp(programming, 0, statMax);
		design = Mathf.Clamp(design, 0, statMax);
		sound = Mathf.Clamp(sound, 0, statMax);
		like = Mathf.Clamp(like, 0, statMax);
		dislike = Mathf.Clamp(dislike, 0, statMax);
	}

	public override bool Equals(object obj) {
		return base.Equals(obj);
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}

	public override string ToString() {
		return "(" + like + ", " + dislike + ")";
		//return "(" + art + ", " + programming + ", " + design + ", " + sound + ", " + like + ", " + dislike + ")";
	}
}