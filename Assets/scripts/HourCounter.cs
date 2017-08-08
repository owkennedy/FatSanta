using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourCounter : MonoBehaviour {

	public Text 	timeText;
	public Text 	weightText;
	public Text 	narrationText;
	public Text 	fatManText;
	public Slider 	hungerSlider;

	public int gramLossSpeed = 1;

	int hours;
	int dayCount;
	int weight;
	int gramCounter;
	bool afternoon;
	bool nightTime;

	enum BodyStates {death, starvation_2, starvation_1, starvation_0, lean, looking_good, overweight, fatMan, obese_0, obese_1, morbid_0, morbid_1, explode};
	BodyStates fatManBodyState;

	void Start ()
	{
		hours = 1;
		dayCount = 1;
		weight = 100;
		gramCounter = 999;
		hungerSlider.value = 100;
		afternoon = false;
		nightTime = true;

		timeText.text = "Time: " + hours + "AM\n" +
						"Day: " + dayCount;

		StartCoroutine (Timer ());

		fatManBodyState = BodyStates.fatMan;
	}

	void Update ()
	{
		print (fatManBodyState);

		//This will choose the right methods based on the Fat Man's current weight
		if 		(fatManBodyState == BodyStates.fatMan) 			{FatMan ();} 
		else if (fatManBodyState == BodyStates.explode) 		{Explode ();}
		else if (fatManBodyState == BodyStates.morbid_1) 		{Morbid_1 ();}
		else if (fatManBodyState == BodyStates.morbid_0) 		{Morbid_0 ();}
		else if (fatManBodyState == BodyStates.obese_1) 		{Obese_1 ();}
		else if (fatManBodyState == BodyStates.obese_0) 		{Obese_0 ();}
		else if (fatManBodyState == BodyStates.overweight) 		{Overweight ();}
		else if (fatManBodyState == BodyStates.looking_good)	{LookingGood ();} 
		else if (fatManBodyState == BodyStates.lean) 			{Lean ();} 
		else if (fatManBodyState == BodyStates.starvation_0) 	{Starvation_0 ();} 
		else if (fatManBodyState == BodyStates.starvation_1) 	{Starvation_1 ();} 
		else if (fatManBodyState == BodyStates.starvation_2) 	{Starvation_2 ();} 
		else if (fatManBodyState == BodyStates.death) 			{Death ();}
	}


	//This is the internal clock 
	IEnumerator Timer ()
	{
		yield return new WaitForSeconds (1);
		hours++;
		Debug.Log ("day: " + dayCount);
		SetTime ();
		while (true) {
			yield return new WaitForSeconds (1);
			hours++;
			SetPM ();
			ResetTime ();
			SetNightTime ();
			LoseGram ();
			GetHungry ();
			SetTime ();
			Debug.Log ("Day: " + dayCount);
		}
	}

	//This sets the time in synch with the internal clock and also states the days.
	void SetTime ()
	{
		if (afternoon == false) {
			timeText.text = "Time: " + hours + "AM\n" +
							"Day: " + dayCount;
		} else {
			timeText.text = "Time: " + hours + "PM\n" +
							"Day: " + dayCount;
		}
	}

	//Checks to see if it is now PM or AM
	void SetPM ()
	{
		if (hours == 12 && afternoon == false) {
			afternoon = true;
		} else if (hours == 12 && afternoon == true) {
			afternoon = false;
			dayCount++;
		}
	}

	//Check to see if it is Night Time or not. Hunger decreased slower at night.
	void SetNightTime () {
		if (hours == 1 && afternoon == false) {
			nightTime = true;
		} else if (hours == 8 && afternoon == false) {
			nightTime = false;
		}
	}

		//Checks to see if it is now PM or AM
		void ResetTime ()
		{
			if (hours > 12) {
				hours = 1;
		} else {return;}
	}


	void LoseGram ()
	{
		gramCounter = gramCounter - gramLossSpeed;
		Debug.Log (gramCounter);

		if (gramCounter < 0) {
			gramCounter = 999;
			weight--;
			weightText.text = "Weight: " + weight + "kg";
		} else if (gramCounter > 999) {
			gramCounter = 0;
			weight++;
			weightText.text = "Weight: " + weight + "kg";
		}
	}

	void GetHungry ()
	{
		if (nightTime == false) {
			hungerSlider.value -= 10;
		} else if (nightTime == true) {
			hungerSlider.value -= 5;
		}
	}

	void Eat ()
	{
		if (Input.GetKeyDown (KeyCode.F)) {
			gramCounter += 1000;
			hungerSlider.value += 50;
		}
	}

	// Methods fired in update based on the current weight.

	void Explode ()
	{
		narrationText.text = "The fat man exploded! You monster!";
		Explode_Speak ();
	}

	void Morbid_1 ()
	{
		narrationText.text = "Watch out!!\n The fat man could explode!\n DON'T Press 'F' to feed the fat man!";
		Morbid_1Speak ();
		Eat ();

		if (weight < 180) {
			fatManBodyState = BodyStates.morbid_0;
		} else if (weight >= 200) {
			fatManBodyState = BodyStates.explode;}
	}

	void Morbid_0 ()
	{
		narrationText.text = "The fat man is morbidly obese!\n You should stop feeding him so much.\n Press 'F' if you want to feed the fat man.";
		Morbid_0Speak ();
		Eat ();

		if (weight < 160) {
			fatManBodyState = BodyStates.obese_1;
		} else if (weight >= 180) {
			fatManBodyState = BodyStates.morbid_1;}
	}

	void Obese_1 ()
	{
		narrationText.text = "The fat man is very obese. If he stays like this you will shorten his life expectancy!\n Press 'F' to feed the fat man.";
		Obese_1Speak ();
		Eat ();

		if (weight < 140) {
			fatManBodyState = BodyStates.obese_0;
		} else if (weight >= 160) {
			fatManBodyState = BodyStates.morbid_0;}
	}

	void Obese_0 ()
	{
		narrationText.text = "The fat man is obese. \n He has a higher chance of developing: cardiovascular\n diseases, diabetes, and depression.\n Press 'F' to feed the fat man.";
		Obese_0Speak ();
		Eat ();

		if (weight < 120) {
			fatManBodyState = BodyStates.fatMan;
		} else if (weight >= 140) {
			fatManBodyState = BodyStates.obese_1;}
	}

	void FatMan ()
	{
		narrationText.text = "This Fat Man leads a happy life on his couch!\n But now someone else is in control of his food!\n Press 'F' to feed the fat man.";
		FatManSpeak ();
		Eat ();

		if (weight < 90) {
			fatManBodyState = BodyStates.overweight;
		} else if (weight >= 120) {
			fatManBodyState = BodyStates.obese_0;}
	}

	void Overweight ()
	{
		narrationText.text = "The fat man has lost a little weight.\n Press 'F' to feed the fat man.";
		OverweightSpeak ();
		Eat ();

		if (weight < 80) {
			fatManBodyState = BodyStates.looking_good;
		} else if (weight >= 90) {
			fatManBodyState = BodyStates.fatMan;}
	}

	void LookingGood ()
	{
		narrationText.text = "The fat man is much healthier now. But is he happy?\n Press 'F' to feed the fat man.";
		LookingGoodSpeak ();
		Eat ();

		if (weight < 70) {
			fatManBodyState = BodyStates.lean;
		} else if (weight >= 80) {
			fatManBodyState = BodyStates.overweight;}
	}

	void Lean ()
	{
		narrationText.text = "The fat man is now a lean machine.\n Press 'F' to feed the fat man.";
		LeanSpeak ();
		Eat ();

		if (weight < 60) {
			fatManBodyState = BodyStates.starvation_0;
		} else if (weight >= 70) {
			fatManBodyState = BodyStates.looking_good;}
	}

	void Starvation_0 ()
	{
		narrationText.text = "The fat man has lost a lot of weight! Possibly too much...\n Press 'F' to feed the fat man.";
		Starvation_0Speak ();
		Eat ();

		if (weight < 50) {
			fatManBodyState = BodyStates.starvation_1;
		} else if (weight >= 60) {
			fatManBodyState = BodyStates.lean;}
	}

	void Starvation_1 ()
	{
		narrationText.text = "The fat man has lost too much weight! He  is starving.\n Press 'F' to feed the fat man.";
		Starvation_1Speak ();
		Eat ();

		if (weight < 40) {
			fatManBodyState = BodyStates.starvation_2;
		} else if (weight >= 50) {
			fatManBodyState = BodyStates.starvation_0;}
	}

	void Starvation_2 ()
	{
		narrationText.text = "The fat man is starving to death! Be careful! Don't kill the fat man!\n For the love of god, press'F' to feed the fat man!";
		Starvation_2Speak ();
		Eat ();

		if (weight < 30) {
			fatManBodyState = BodyStates.death;
		} else if (weight >= 40) {
			fatManBodyState = BodyStates.starvation_1;}
	}
		
	void Death ()
	{
		DeathSpeak ();
		narrationText.text = "The fat man has starved to death. Rest in peace, fat man.";
	}

	//Methods that control what the Fat Man says.
	void Explode_Speak ()
	{
		fatManText.text = "...";
	}

	void Morbid_1Speak ()
	{
		if (hungerSlider.value >= 80) {
			fatManText.text = "...please stop... no more... it’s too much... I don’t want the food...";
		} else if (hungerSlider.value > 50 && nightTime == false) {
			fatManText.text = "Stomach... slowly recovering...";
		} else if (hungerSlider.value > 50 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... leave me alone... roast chicken... zzzZZZzzz...";
		} else if (hungerSlider.value > 0) {
			fatManText.text = "NNNGGGG... some kale could clean me out...";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "BLERRRRRR...";
		}
	}
		
	void Morbid_0Speak ()
	{
		if (hungerSlider.value >= 80 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... BLERRRRRR... zzzZZZzzz...";
		} else if (hungerSlider.value >= 80 && nightTime == false) {
			fatManText.text = "... I THINK I ATE TOO MUCH...";
		} else if (hungerSlider.value > 50 && nightTime == false) {
			fatManText.text = "BLOOD SUGAR... WHERE IS BLOOD SUGAR?";
		} else if (hungerSlider.value > 50 && nightTime == true) {
			fatManText.text = "\"zzzZZZzzz... SUGAR...  zzzZZZzzz...";
		} else if (hungerSlider.value > 0) {
			fatManText.text = "NNNGGGG... STOMACHE NEEDS FOOD";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "GIVE ME PIZZA BEFORE I BELLY-CRUSH YOU";
		}
	}

	void Obese_1Speak ()
	{
		if (hungerSlider.value >= 80 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... STEEAK!... zzzZZZzzz...";
		} else if (hungerSlider.value >= 80 && nightTime == false) {
			fatManText.text = "I LOVE YOU SO MUCH... STEAK";
		} else if (hungerSlider.value > 50 && nightTime == false) {
			fatManText.text = "I NEED CHOCOCLATE!";
		} else if (hungerSlider.value > 50 && nightTime == true) {
			fatManText.text = "\"zzzZZZzzz... Hey kid... CHOCOLATE...  zzzZZZzzz...";
		} else if (hungerSlider.value > 0) {
			fatManText.text = "FOOOOD";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "FEEEEED MEEEE!";
		}
	}

	void Obese_0Speak ()
	{
		if (hungerSlider.value >= 80 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... Fly my pretties... Chicken wings!... zzzZZZzzz...";
		} else if (hungerSlider.value >= 80 && nightTime == false) {
			fatManText.text = "Yess... Food SO GOOD!! You're my BEST FRIEND!";
		} else if (hungerSlider.value > 50 && nightTime == false) {
			fatManText.text = "MORE FOOD! MORE! FOOD!";
		} else if (hungerSlider.value > 50 && nightTime == true) {
			fatManText.text = "\"zzzZZZzzz... Hey kid, give me that chocolate...  zzzZZZzzz...";
		} else if (hungerSlider.value > 0) {
			fatManText.text = "I HAVEN'T EATEN IN HOURS! GIVE ME A PIE!";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "Why would you feed me like this and then starve me?!?";
		}
	}

	void FatManSpeak ()
	{
		if (hungerSlider.value >= 80 && nightTime == true) {
			fatManText.text = "zzzzZZZZzzzz... Yummy food in my belly... zzzzZZZZzzzz...";
		} else if (hungerSlider.value >= 80 && nightTime == false) {
			fatManText.text = "mmmmMMMMmmmm... That tasted good! Thanks buddy. We should hang out more often!";
		} else if (hungerSlider.value > 60 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... I love you too, Mr. Ice Cream... zzzZZZzzz...";
		} else if (hungerSlider.value > 60 && nightTime == false) {
			fatManText.text = "Hey bud! Fancy a snack?";
		} else if (hungerSlider.value > 30 && nightTime == true) {
			fatManText.text = "Stomach is growling... Isn't there any food?";
		} else if (hungerSlider.value > 30 && nightTime == false) {
			fatManText.text = "I'm hungry! I could eat anything!";
		}else if (hungerSlider.value > 0 && nightTime == true) {
			fatManText.text = "I'm REALLY hungry, man. What's the deal? I can't sleep without food!";
		} else if (hungerSlider.value > 0 && nightTime == false) {
			fatManText.text = "Okay, there better be food soon, because I'm REALLY hungry and this is unacceptable!";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "I HAVEN'T EATEN IN AGES! DON'T YOU KNOW WHO I AM ?!?!?!? GIVE ME FOOOOOOOD!";
		}
	}

	void OverweightSpeak ()
	{
		if (hungerSlider.value > 70 && nightTime == true) {
			fatManText.text = "zzzZZZzzz...";
		} else if (hungerSlider.value > 70 && nightTime == false) {
			fatManText.text = "Thanks for the meal, bud! I think I'm starting to like you again.";
		} else if (hungerSlider.value > 30 && nightTime == true) {
			fatManText.text = "Come on, man... how about some more food... Need a bacon sandwich hit... Can't sleep without a bacon sandiwch";
		} else if (hungerSlider.value > 30 && nightTime == false) {
			fatManText.text = "Look, I've lost weight! So, just feed me already!";
		}else if (hungerSlider.value > 0 && nightTime == true) {
			fatManText.text = "My stomach is GROWLING too loudly too sleep!\n If you weren't all the way on the other side of that monitor I'd give you a slap!";
		} else if (hungerSlider.value > 0 && nightTime == false) {
			fatManText.text = "You filthy coward, hiding behind your keyboard. A real man OR WOMAN would give me a delicious burger.\n I'd be angry at you if I didn't already feel sorry for you.";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "If I ever get out of here I’ll strangle you into a pork sausage! GIVE ME FOOD! NOW! Before I beat the crap out of you!";
		}
	}

	void LookingGoodSpeak ()
	{
		if (hungerSlider.value > 65 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... \nOh hey there dream girl... \nYes, as a matter of fact I have lost some weight...\n zzzZZZzzz...";
		} else if (hungerSlider.value > 65 && nightTime == false) {
			fatManText.text = "Seriously buddy, what did you put in that meal? Paprika? Whatever it was, it tasted great!";
		} else if (hungerSlider.value > 30 && nightTime == true) {
			fatManText.text = "Could use a midnight snack around now, if that's cool? \n Not a big deal though... Happy to just stare at the ceiling feeling sad all night... again...";
		} else if (hungerSlider.value > 30 && nightTime == false) {
			fatManText.text = "Most people eat three meals a day. You know that, right?\n I'm just asking for one... or maybe two...";
		}else if (hungerSlider.value > 0 && nightTime == true) {
			fatManText.text = "Starving like this can turn me into an anorexic... Is that what you want?";
		} else if (hungerSlider.value > 0 && nightTime == false) {
			fatManText.text = "You're sick. I've lost over 30kg and you still want me to starve? You disgust me.";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "What have you done?!? My clothes don’t fit me anymore.\n I don’t recognise myself in the mirror. I’m so hungry my stomach is going to take over my mind… Stop yelling at me, stomach! It’s not my fault!";
		}
	}

	void LeanSpeak ()
	{
		if (hungerSlider.value > 60 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... \nOh hey there dream girls... I would love to sleep with all of you but there's someone watching me right now and I don't want you to be creeped out\n zzzZZZzzz...";
		} else if (hungerSlider.value > 60 && nightTime == false) {
			fatManText.text = "This is a great meal! For a while I thought you were trying to kill me, but this proves you have my best intentions at heart! I feel fantastic! Best I've felt in years.";
		} else if (hungerSlider.value > 30 && nightTime == true) {
			fatManText.text = "Another sleepless night, but hey! I'm losing weight aren't I?";
		} else if (hungerSlider.value > 30 && nightTime == false) {
			fatManText.text = "Oooh, just heard my stomach growl. No matter, I'll just try to ignore it.";
		}else if (hungerSlider.value > 0) {
			fatManText.text = "Why are you doing this? I'm already skinny! You should be feeding me!";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "Oh man, everything looks like food. My own body looks like food! My arm looks like a delicious baguette! Give me food before I eat my own delicious baguette arm!";
		}
	}

	void Starvation_0Speak ()
	{
		if (hungerSlider.value > 50 && nightTime == true) {
			fatManText.text = "zzzZZZzzz... Secretly planning my escape from this computer\n zzzZZZzzz...";
		} else if (hungerSlider.value > 50 && nightTime == false) {
			fatManText.text = "Thank you dear sir, for this precious meal.";
		} else if (hungerSlider.value > 30) {
			fatManText.text = "Shut up, stomach. No! SHHH!";
		} else if (hungerSlider.value > 0) {
			fatManText.text = "No no no no no no no no no no...";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "Come on, buddy. Please… I appreciate what you’ve done.\n I’ve lost a lot of weight. But look at me. It’s all gone now. There’s nothing much more I can lose… and I’m just so hungry…";
		}
	}

	void Starvation_1Speak ()
	{
		if (hungerSlider.value > 50) {
			fatManText.text = "Precious... precious food!";
		} else if (hungerSlider.value > 0) {
			fatManText.text = "Days turn to night... Night turns to day... It's all the same, can't keep track...";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "I’m withering away! Look at me! I’m not half the man I once was! I can’t even stand up… Feel so strange… The days just pass but I don’t know can’t see them… My body just eats itself.";
		}
	}

	void Starvation_2Speak ()
	{
		if (hungerSlider.value > 0) {
			fatManText.text = "You… you gave me food! It tastes so wonderful. You wonderful, wonderful person. I think I feel a little less dizzy. Maybe in a little while I’ll see if I can stand up…";
		} else if (hungerSlider.value <= 0) {
			fatManText.text = "You’re going to kill me… Please… I’ll do anything… You can’t just let me die! You’re not a monster are you? ARE YOU?!?";
		}
	}

	void DeathSpeak ()
	{
		fatManText.text = "frrreergggghhh...";
	}
}
