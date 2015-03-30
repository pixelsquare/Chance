using Item;
using Mission;
using Mission.Database;
using NPC;
using NPC.Database;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	# region Public Variables

	[SerializeField]
	private MissionNameID initialMission;

	# endregion Public Variables

	# region Private Variables

	// Mission 1
	private NPCData[] npcTalkedTo;

	private NPCData data;
	private System.Random rng;
	private int indx;

	private int missionIndx;
	private bool missionActive;
	private MissionData baseMissionData;

	private GameManager gameManager;
	private PlayerInformation playerInformation;
	private UserInterface userInterface;

	private MissionUi missionUi;

	# endregion Private Variables

	# region Reset Variables

	private MissionNameID _initialMission;

	# endregion Reset Variables

	public MissionData BaseMissionData {
		get { return baseMissionData; }
		set { baseMissionData = value; }
	}

	public static MissionManager current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		gameManager = GameManager.current;
		missionUi = MissionUi.current;
		userInterface = UserInterface.current;
		Reset();
	}

	private void Update() {
		if (playerInformation == null && gameManager.BasePlayerData != null) {
			playerInformation = gameManager.BasePlayerData.PlayerInformation;
		}

		if (baseMissionData != null && missionUi.HasAcceptedMission) {
			if (baseMissionData.Mission.MissionMid != null) {
				baseMissionData.Mission.MissionMid();
			}

			if (baseMissionData.Mission.MissionEnded != null) {
				if (baseMissionData.Mission.MissionEnded()) {
					if (baseMissionData.Mission.MissionSuccess != null) {
						baseMissionData.Mission.MissionSuccess();
					}

					if (baseMissionData.Mission.MissionResult != null) {
						baseMissionData.Mission.MissionResult();
					}
				}
			}
		}
	}

	public void RunMission() {
		if (missionIndx < MissionDatabase.missionData.Length) {
			missionIndx++;
			MissionNameID nameID = (MissionNameID)missionIndx;
			baseMissionData = MissionDatabase.GetMission(nameID);

			if (nameID == MissionNameID.Mission1) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission1End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission2) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission2End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission3) {
				baseMissionData.Mission = new Mission.Mission(Mission3Start, null, Mission3End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission4) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission4End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission5) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission5End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission6) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission6End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission7) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission7End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission8) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission8End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission9) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission9End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission10) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission10End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission11) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission11End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission12) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission12End, null, MissionSuccess, MissionFailed);
			}
			else if (nameID == MissionNameID.Mission13) {
				baseMissionData.Mission = new Mission.Mission(null, null, Mission13End, null, MissionSuccess, MissionFailed);
			}

			if (baseMissionData.Mission.MissionStart != null) {
				baseMissionData.Mission.MissionStart();
			}

			missionUi.RunMissionButton(1f);
			missionActive = true;
		}
		else {
			Debug.Log("[MISSION] ENDED!");
		}
	}

	# region Mission 1

	private bool Mission1End() {
		return (GetTalkedToNpcCount() >= 6);
	}

	public void AddTalkedToNpc(NPCNameID nameID) {
		int count = 0; int indx = 0;
		for (int i = 0; i < npcTalkedTo.Length; i++) {
			if (npcTalkedTo[i] != null) {
				if (npcTalkedTo[i].NpcNameID == nameID) {
					count++;
					break;
				}
				indx++;
			}
		}

		if (count < 1) {
			npcTalkedTo[indx] = NPCDatabase.GetNPC(nameID);
			Debug.Log("[MISSION] " + nameID.ToString() + " has been added to your talked npc");
		}
	}

	private int GetTalkedToNpcCount() {
		int count = 0;
		for (int i = 0; i < npcTalkedTo.Length; i++) {
			if (npcTalkedTo[i] != null) {
				count++;
			}
		}
		return count;
	}

	# endregion Mission 1

	# region Mission 2

	private bool Mission2End() {
		return (NPCDatabase.GetNPC(NPCNameID.Andy).NpcTalkedCount >= 5);
	}

	# endregion Mission 2

	# region Mission 3

	private void Mission3Start() {
		data = NPCDatabase.GetNPC(NPCNameID.Andy);
		rng = new System.Random();
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission3End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Andy, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 3

	# region Mission 4

	private bool Mission4End() {
		return (NPCDatabase.GetNPC(NPCNameID.Noelle).NpcTalkedCount >= 5);
	}

	# endregion Mission 4

	# region Mission 5

	private bool Mission5End() {
		return true;
	}

	# endregion Mission 5

	# region Mission 6

	private bool Mission6End() {
		return (NPCDatabase.GetNPC(NPCNameID.Franz).NpcTalkedCount >= 5);
	}

	# endregion Mission 6

	# region Mission 7

	private bool Mission7End() {
		return true;
	}

	# endregion Mission 7

	# region Mission 8

	private bool Mission8End() {
		return (NPCDatabase.GetNPC(NPCNameID.Bart).NpcTalkedCount >= 5);
	}

	# endregion Mission 8

	# region Mission 9

	private bool Mission9End() {
		return true;
	}

	# endregion Mission 9

	# region Mission 10

	private bool Mission10End() {
		return (NPCDatabase.GetNPC(NPCNameID.Jenevieve).NpcTalkedCount >= 5);
	}

	# endregion Mission 10

	# region Mission 11

	private bool Mission11End() {
		return true;
	}

	# endregion Mission 11

	# region Mission 12

	private bool Mission12End() {
		return (NPCDatabase.GetNPC(NPCNameID.Maxine).NpcTalkedCount >= 5);
	}

	# endregion Mission 12

	# region Mission 13

	private bool Mission13End() {
		return true;
	}

	# endregion Mission 13

	# region Side Mission 1

	private bool SideMission1() {
		return true;
	}

	# endregion Side Mission 1

	# region Side Mission 2

	private bool SideMission2() {
		return true;
	}

	# endregion Side Mission 2

	# region Side Mission 3

	private bool SideMission3() {
		return true;
	}

	# endregion Side Mission 3

	private void MissionSuccess() {
		if (missionUi.HasAcceptedMission && missionActive && userInterface.InactiveUI()) {
			missionUi.RunMissionTitle("MISSION ACCOMPLISHED", "REWARD: +5 Likes to all", new FadeTransition(MissionTitleStart, null, MissionTitleEnd));
			missionActive = false;
		}
	}

	private void MissionFailed() {
		if (missionUi.HasAcceptedMission && missionActive && userInterface.InactiveUI()) {
			missionUi.RunMissionTitle("MISSION FAILED", string.Empty, new FadeTransition(MissionTitleStart, null, MissionTitleEnd));
			missionActive = false;
		}
	}

	private void MissionTitleStart() {
		missionUi.ShowMissionInfo = false;
	}

	private void MissionTitleEnd() {
		missionUi.HasAcceptedMission = false;
		baseMissionData = null;
		RunMission();
	}

	private void Reset() {
		_initialMission = initialMission;
		missionIndx = (int)_initialMission;
		npcTalkedTo = new NPCData[6];
	}
}