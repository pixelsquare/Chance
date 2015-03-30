using UnityEngine;

public struct GameStory {
	private static Story[] storyText;
	public static Story[] StoryText {
		get { return storyText; }
	}

	public static void Initialize() {
		storyText = new Story[26];

		storyText[0] = new Story(4f, "When I look at the time, I saw every second wasted of me doing nothing but, wondering.");
		storyText[1] = new Story(1f, "Wondering of what?");
		storyText[2] = new Story(2f, "Of how I took things for granted ?");
		storyText[3] = new Story(0.5f, "Or ... ?");
		storyText[4] = new Story(2f, "Of how I made stupid decisions ?");
		storyText[5] = new Story(2f, "What is to become of me when I graduate ?");
		storyText[6] = new Story(1f, "I was supposed to love this.");
		storyText[7] = new Story(2f, "I know I loved this and I still love this now.");
		storyText[8] = new Story(0.5f, "But ...");
		storyText[9] = new Story(0.5f, "I’m losing hope.");
		storyText[10] = new Story(2f, "I can’t even draw brilliantly. I can’t even program logically.");
		storyText[11] = new Story(1.5f, "My skills can’t even reach heights.");
		storyText[12] = new Story(2f, "A post has been posted in the group a few minutes ago.");
		storyText[13] = new Story(0.5f, "What’s this?");
		storyText[14] = new Story(1f, "An event? A game jam?");
		storyText[15] = new Story(3f, "Is this a sign? ... Is fate giving me one last chance to prove myself?");
		storyText[16] = new Story(2f, "tick tick tick. Click. Tick tick tick. Click click.");
		storyText[17] = new Story(4f, "I don’t know if this is it, but, I can’t stop my hands from moving, from typing and from clicking.");
		storyText[18] = new Story(2f, "I can’t even see it move nor type nor click.");
		storyText[19] = new Story(1f, "Everything was a blur until-");
		storyText[20] = new Story(0.5f, "It stopped ...");
		storyText[21] = new Story(1f, "To click this one button without hesitation.");
		storyText[22] = new Story(1f, "This one button that could change my fate.");
		storyText[23] = new Story(3f, "This one button that could help me become something I wanted since a long time ago.");
		storyText[24] = new Story(0.5f, "I will be ...");
		storyText[25] = new Story(1f, "R e b o r n .");
	}
}

public struct Story {
	public Story(float dur, string txt) {
		duration = dur;
		text = txt;
	}

	[SerializeField]
	private string text;
	public string Text {
		get { return text; }
	}

	[SerializeField]
	private float duration;
	public float Duration {
		get { return duration; }
	}
}