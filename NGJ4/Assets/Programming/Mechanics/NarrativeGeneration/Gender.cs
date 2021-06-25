
public enum Gender {
	MALE = 0,
	FEMALE = 1,
	AMBI = 2
}

public static class Extensions {
	public static string SPronoun(this Gender gender) => 
		(new string[]{"he", "she", "they"})[(int) gender];
	public static string OPronoun(this Gender gender) => 
		(new string[]{"him", "her", "them"})[(int) gender];
	public static string PPronoun(this Gender gender) => 
		(new string[]{"his", "her", "their"})[(int) gender];
	public static string GenFirstName(this Gender gender) => 
		(new string[]{"#mcharName", "#fcharName", "#acharName"})[(int) gender];
	public static string Name(this Gender gender) => 
		(new string[]{"Male", "Female", "Ambiguous"})[(int) gender];
}