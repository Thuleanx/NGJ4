using UnityEngine;
using Thuleanx;

public class CharacterInfo {
	public string firstName, lastName, trait, job, equipment;
	public Gender gender;

	public string fullName => firstName + " " + lastName;

	CharacterInfo(
		string firstName, 
		string lastName, 
		string trait, 
		string job, 
		string equipment, 
		Gender gender) {


		this.firstName = firstName;
		this.lastName = lastName;
		this.trait = trait;
		this.job = job;
		this.equipment = equipment;
		this.gender = gender;
	}

	public override string ToString() =>
		$"{fullName}\ngender: {gender.Name()}\n{trait} {job}\nBrought {equipment}";

	public static CharacterInfo GenerateCharacter(CharacterClass charClass) {
		Gender gender = (Gender) mt19937.RangeWeighted(new int[]{10, 10, 1});
		string firstName = App.Instance._NarrativeGenerator.parse(gender.GenFirstName());
		string lastName = App.Instance._NarrativeGenerator.parse("#familyName");
		string trait = App.Instance._NarrativeGenerator.parse("#traits");
		string job = App.Instance._NarrativeGenerator.parse("#jobs");
		string equipment = App.Instance._NarrativeGenerator.parse("#wpn");

		return new CharacterInfo(
			firstName,
			lastName,
			trait,
			job,
			equipment,
			gender
		);
	}
}