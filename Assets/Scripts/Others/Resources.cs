using Item.Database;
using NPC.Database;
using UnityEngine;

public class Resources : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Texture2D blackTexture;
    [SerializeField]
    private Texture2D danceInstruction;
	[SerializeField]
	private Texture2D keypressInstruction;
	[SerializeField]
	private Texture2D hideNSeekInstruction;
	[SerializeField]
	private AvatarDatabase avatarDatabase;
	[SerializeField]
	private IconDatabase iconDatabase;

	# endregion Public Variables

	// Public Properties
	public static Texture2D BlackTexture {
		get { return current.blackTexture; }
	}

    public static Texture2D DanceInstruction {
        get { return current.danceInstruction; }
    }

	public static Texture2D KeypressInstruction {
		get { return current.keypressInstruction; }
	}

	public static Texture2D HideNSeekInstruction {
		get { return current.hideNSeekInstruction; }
	}

	public static AvatarDatabase AvatarDatabase {
		get { return current.avatarDatabase; }
	}

	public static IconDatabase IconDatabase {
		get { return current.iconDatabase; }
	}
	// --

	private static Resources current;

	private void Awake() {
		current = this;
		NPCDatabase.Initialize();
		ItemDatabase.Initialize();
	}
}

[System.Serializable]
public class AvatarDatabase {
	public AvatarDatabase() { }

	[SerializeField]
	private Texture2D placeholderAvatar;
	public Texture2D PlaceholderAvatar {
		get { return placeholderAvatar; }
	}

	[SerializeField]
	private Texture2D betaAvatar;
	public Texture2D BetaAvatar {
		get { return betaAvatar; }
	}

    [SerializeField]
    private Texture2D ninaAvatar;
    public Texture2D NinaAvatar
    {
        get { return ninaAvatar; }

    }

	[SerializeField]
	private Texture2D owenAvatar;
	public Texture2D OwenAvatar {
		get { return owenAvatar; }
	}

    #region Noelle

        [SerializeField]
        private Texture2D noelleAvatar;
        public Texture2D NoelleAvatar
        {
            get { return noelleAvatar; }
        }

        [SerializeField]
        private Texture2D noelleHappyAvatar;
        public Texture2D NoelleHappyAvatar
        {
            get { return noelleHappyAvatar; }
        }

        [SerializeField]
        private Texture2D noelleSadAvatar;
        public Texture2D NoelleSadAvatar
        {
            get { return noelleSadAvatar; }
        }

        [SerializeField]
        private Texture2D noelleMadAvatar;
        public Texture2D NoelleMadAvatar
        {
            get { return noelleMadAvatar; }
        }

        [SerializeField]
        private Texture2D noelleWeirdAvatar;
        public Texture2D NoelleWeirdAvatar
        {
            get { return noelleWeirdAvatar; }
        }

    #endregion Noelle

    #region Andy

        [SerializeField]
        private Texture2D andyAvatar;
        public Texture2D AndyAvatar
        {
            get { return andyAvatar; }
        }

        [SerializeField]
        private Texture2D andyHappyAvatar;
        public Texture2D AndyHappyAvatar
        {
            get { return andyHappyAvatar; }
        }

        [SerializeField]
        private Texture2D andySadAvatar;
        public Texture2D AndySadAvatar
        {
            get { return andySadAvatar; }
        }

        [SerializeField]
        private Texture2D andyMadAvatar;
        public Texture2D AndyMadAvatar
        {
            get { return andyMadAvatar; }
        }

        [SerializeField]
        private Texture2D andyWeirdAvatar;
        public Texture2D AndyWeirdAvatar
        {
            get { return andyWeirdAvatar; }
        }

        #endregion Andy

    #region Franz

        [SerializeField]
        private Texture2D franzAvatar;
        public Texture2D FranzAvatar
        {
            get { return franzAvatar; }
        }

        [SerializeField]
        private Texture2D franzHappyAvatar;
        public Texture2D FranzHappyAvatar
        {
            get { return franzHappyAvatar; }
        }

        [SerializeField]
        private Texture2D franzSadAvatar;
        public Texture2D FranzSadAvatar
        {
            get { return franzSadAvatar; }
        }

        [SerializeField]
        private Texture2D franzMadAvatar;
        public Texture2D FranzMadAvatar
        {
            get { return franzMadAvatar; }
        }

        [SerializeField]
        private Texture2D franzWeirdAvatar;
        public Texture2D FranzWeirdAvatar
        {
            get { return franzWeirdAvatar; }
        }


        #endregion Franz

    #region Jenevieve

        [SerializeField]
        private Texture2D jenevieveAvatar;
        public Texture2D JenevieveAvatar
        {
            get { return jenevieveAvatar; }
        }

        [SerializeField]
        private Texture2D jenevieveHappyAvatar;
        public Texture2D JenevieveHappyAvatar
        {
            get { return jenevieveHappyAvatar; }
        }

        [SerializeField]
        private Texture2D jenevieveSadAvatar;
        public Texture2D JenevieveSadAvatar
        {
            get { return jenevieveSadAvatar; }
        }

        [SerializeField]
        private Texture2D jenevieveMadAvatar;
        public Texture2D JenevieveMadAvatar
        {
            get { return jenevieveMadAvatar; }
        }

        [SerializeField]
        private Texture2D jenevieveWeirdAvatar;
        public Texture2D JenevieveWeirdAvatar
        {
            get { return jenevieveWeirdAvatar; }
        }

        #endregion Jenevieve

    #region Bart

        [SerializeField]
        private Texture2D bartAvatar;
        public Texture2D BartAvatar
        {
            get { return bartAvatar; }
        }

        [SerializeField]
        private Texture2D bartHappyAvatar;
        public Texture2D BartHappyAvatar
        {
            get { return bartHappyAvatar; }
        }

        [SerializeField]
        private Texture2D bartSadAvatar;
        public Texture2D BartSadAvatar
        {
            get { return bartSadAvatar; }
        }

        [SerializeField]
        private Texture2D bartMadAvatar;
        public Texture2D BartMadAvatar
        {
            get { return bartMadAvatar; }
        }

        [SerializeField]
        private Texture2D bartWeirdAvatar;
        public Texture2D BartWeirdAvatar
        {
            get { return bartWeirdAvatar; }
        }

        #endregion Jenevieve

    #region Maxine

        [SerializeField]
        private Texture2D maxineAvatar;
        public Texture2D MaxineAvatar
        {
            get { return maxineAvatar; }
        }

        [SerializeField]
        private Texture2D maxineHappyAvatar;
        public Texture2D MaxineHappyAvatar
        {
            get { return maxineHappyAvatar; }
        }

        [SerializeField]
        private Texture2D maxineSadAvatar;
        public Texture2D MaxineSadAvatar
        {
            get { return maxineSadAvatar; }
        }

        [SerializeField]
        private Texture2D maxineMadAvatar;
        public Texture2D MaxineMadAvatar
        {
            get { return maxineMadAvatar; }
        }

        [SerializeField]
        private Texture2D maxineWeirdAvatar;
        public Texture2D MaxineWeirdAvatar
        {
            get { return maxineWeirdAvatar; }
        }

        #endregion Jenevieve
}

[System.Serializable]
public class IconDatabase {
	public IconDatabase() { }

	[SerializeField]
	private Texture2D iconPlaceholder;
	public Texture2D IconPlaceholder {
		get { return iconPlaceholder; }
	}

	[SerializeField]
	private Texture2D chromeIcon;
	public Texture2D ChromeIcon {
		get { return chromeIcon; }
	}

	[SerializeField]
	private Texture2D donutIcon;
	public Texture2D DonutIcon {
		get { return donutIcon; }
	}

    #region Noelle

    [SerializeField]
    private Texture2D strawberryMilkshakeIcon;
    public Texture2D StrawberryMilkshakeIcon
    {
        get { return strawberryMilkshakeIcon; }
    }

    [SerializeField]
    private Texture2D fanfictionNotebookIcon;
    public Texture2D FanfictionNotebookIcon
    {
        get { return fanfictionNotebookIcon; }
    }

    #endregion Noelle

    #region Andy

    [SerializeField]
    private Texture2D grassjellyMilkteaIcon;
    public Texture2D GrassJellyMilkteaIcon
    {
        get { return grassjellyMilkteaIcon; }
    }

    [SerializeField]
    private Texture2D usbIcon;
    public Texture2D USBIcon
    {
        get { return usbIcon; }
    }

    #endregion Andy

    #region Franz

    [SerializeField]
    private Texture2D orangeJuiceIcon;
    public Texture2D OrangeJuiceIcon
    {
        get { return orangeJuiceIcon; }
    }

    [SerializeField]
    private Texture2D markersIcon;
    public Texture2D MarkersIcon
    {
        get { return markersIcon; }
    }

    #endregion Franz

    #region Jenevieve

    [SerializeField]
    private Texture2D icedTeaIcon;
    public Texture2D IcedTeaIcon
    {
        get { return icedTeaIcon; }
    }

    [SerializeField]
    private Texture2D whiteEarphonesIcon;
    public Texture2D WhiteEarphonesIcon
    {
        get { return whiteEarphonesIcon; }
    }

    #endregion Jenevieve

    #region Bart

    [SerializeField]
    private Texture2D hotCoffeeIcon;
    public Texture2D HotCoffeeIcon
    {
        get { return hotCoffeeIcon; }
    }

    [SerializeField]
    private Texture2D guitarPickIcon;
    public Texture2D GuitarPickIcon
    {
        get { return guitarPickIcon; }
    }

    #endregion Bart

    #region Maxine

    [SerializeField]
    private Texture2D milkIcon;
    public Texture2D MilkIcon
    {
        get { return milkIcon; }
    }

    [SerializeField]
    private Texture2D headphonesIcon;
    public Texture2D HeadphonesIcon
    {
        get { return headphonesIcon; }
    }

    #endregion Maxine
}

//[System.Serializable]
//public class AudioDatabase {
//    public AudioDatabase() { }

//    [SerializeField]
//    private AudioClip mainMenuClip;
//    public AudioClip MainMenuClip {
//        get { return mainMenuClip; }
//    }

//    [SerializeField]
//    private AudioClip gameStoryClip;
//    public AudioClip GameStoryClip {
//        get { return gameStoryClip; }
//    }

//    [SerializeField]
//    private AudioClip mainGameClip;
//    public AudioClip MainGameClip {
//        get { return mainGameClip; }
//    }

//    [SerializeField]
//    private AudioClip gameEndClip;
//    public AudioClip GameEndClip {
//        get { return gameEndClip; }
//    }

//    [SerializeField]
//    private AudioClip menuButtonsClip;
//    public AudioClip MenuButtonsClip {
//        get { return menuButtonsClip; }
//    }

//    [SerializeField]
//    private AudioClip keypressClip;
//    public AudioClip KeypressClip {
//        get { return keypressClip; }
//    }

//    [SerializeField]
//    private AudioClip danceOffClip;
//    public AudioClip DanceOffClip {
//        get { return danceOffClip; }
//    }

//    [SerializeField]
//    private AudioClip hideNSeekClip;
//    public AudioClip HideNSeekClip {
//        get { return hideNSeekClip; }
//    }

//    [SerializeField]
//    private AudioClip keypressKeyClip;
//    public AudioClip KeypressKeyClip {
//        get { return keypressKeyClip; }
//    }

//    [SerializeField]
//    private AudioClip keypressEndClip;
//    public AudioClip KeypressEndClip {
//        get { return keypressEndClip; }
//    }

//    [SerializeField]
//    private AudioClip danceOffKeyClip;
//    public AudioClip DanceOffKeyClip {
//        get { return danceOffKeyClip; }
//    }

//    [SerializeField]
//    private AudioClip danceOffEndClip;
//    public AudioClip DanceOffEndClip {
//        get { return danceOffEndClip; }
//    }

//    [SerializeField]
//    private AudioClip hideNSeekFoundClip;
//    public AudioClip HideNSeekFoundClip {
//        get { return hideNSeekFoundClip; }
//    }

//    [SerializeField]
//    private AudioClip hideNSeekEndClip;
//    public AudioClip HideNSeekEndClip {
//        get { return hideNSeekEndClip; }
//    }

//    [SerializeField]
//    private AudioClip missionAccomplishedClip;
//    public AudioClip MissionAccomplishedClip {
//        get { return missionAccomplishedClip; }
//    }

//    [SerializeField]
//    private AudioClip missionFailedClip;
//    public AudioClip MissionFailedClip {
//        get { return missionFailedClip; }
//    }
//}