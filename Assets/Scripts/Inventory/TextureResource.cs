using Item.Database;
using NPC.Database;
using UnityEngine;

public class TextureResource : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private AvatarDatabase avatarDatabase;
	[SerializeField]
	private IconDatabase iconDatabase;

	# endregion Public Variables

	// Public Properties
	public static AvatarDatabase AvatarDatabase {
		get { return current.avatarDatabase; }
	}

	public static IconDatabase IconDatabase {
		get { return current.iconDatabase; }
	}
	// --

	private static TextureResource current;

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
	private Texture2D ninaAvatar;
	public Texture2D NinaAvatar {
		get { return ninaAvatar; }

	}

	[SerializeField]
	private Texture2D betaAvatar;
	public Texture2D BetaAvatar {
		get { return betaAvatar; }
	}
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
}