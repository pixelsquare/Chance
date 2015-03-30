using UnityEngine;

namespace Player {
	public class PlayerData {
		public PlayerData() {
			playerName = string.Empty;
			playerAvatar = null;
			playerStatistics = Statistics.zero;
			playerT = null;
			playerInformation = null;
			playerControl = null;
		}

		public PlayerData(string name, Texture2D avatar, Statistics stat) {
			playerName = name;
			playerAvatar = avatar;
			playerStatistics = stat;
			playerT = null;
			playerInformation = null;
			playerControl = null;
		}

		public PlayerData(string name, Texture2D avatar, Transform player, Statistics stat) {
			playerName = name;
			playerAvatar = avatar;
			playerT = player;
			playerStatistics = stat;
			playerInformation = playerT.GetComponent<PlayerInformation>();
			playerControl = playerT.GetComponent<PlayerControl>();
		}

		private string playerName;
		public string PlayerName {
			get { return playerName; }
			set { playerName = value; }
		}

		private Texture2D playerAvatar;
		public Texture2D PlayerAvatar {
			get { return playerAvatar; }
			set { playerAvatar = value; }
		}

		private Transform playerT;
		public Transform PlayerT {
			get { return playerT; }
			set {
				playerT = value;
				playerControl = playerT.GetComponent<PlayerControl>();
				playerInformation = playerT.GetComponent<PlayerInformation>();
			}
		}

		private PlayerControl playerControl;
		public PlayerControl PlayerControl {
			get { return playerControl; }
		}

		private PlayerInformation playerInformation;
		public PlayerInformation PlayerInformation {
			get { return playerInformation; }
		}

		private Statistics playerStatistics;
		public Statistics PlayerStatistics {
			get { return playerStatistics; }
			set { playerStatistics = value; }
		}
	}
}