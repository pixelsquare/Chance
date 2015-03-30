using UnityEngine;
using NPC.Database;
using NPC;

public class NPCManager : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Transform[] mainNpc;

	[SerializeField]
	private Transform[] wandererNpc;

	# endregion Public Variables

	// Public Properties
	public Transform[] MainNpc {
		get { return mainNpc; }
	}

	public Transform[] WandererNpc {
		get { return wandererNpc; }
	}

	public void EnableNpcs(bool enable) {
		for (int i = 0; i < MainNpc.Length; i++) {
			NPCInformation npcInformation = MainNpc[i].GetComponent<NPCInformation>();
			npcInformation.NPCEnable = enable;
		}
	}
	// --

	public static NPCManager current;

	private void Awake() {
		current = this;
	}

	// Used to average like and dislike
	public Statistics GetMainNpcAverageStatistics() {
		int count = 0;
		Statistics[] tmpStat = new Statistics[mainNpc.Length];
		for (int i = 0; i < mainNpc.Length; i++) {
			if (mainNpc[i] != null) {
				NPCInformation npcInformation = mainNpc[i].GetComponent<NPCInformation>();
				// Add if npc has information
				if (npcInformation.NpcNameID != NPCNameID.None) {
					tmpStat[i] = NPCDatabase.GetNPC(npcInformation.NpcNameID).NpcStatistics;
					count++;
				}
			}
		}
		return Statistics.GetAverage(tmpStat, count);
	}

	public Transform GetMainNpc(NPCNameID nameID) {
		for (int i = 0; i < mainNpc.Length; i++) {
			NPCInformation npcInformation = mainNpc[i].GetComponent<NPCInformation>();
			if (npcInformation.NpcNameID == nameID) {
				return mainNpc[i];
			}
		}
		return null;
	}

	public NPCControl GetMainNpcControl(NPCNameID nameID) {
		for (int i = 0; i < mainNpc.Length; i++) {
			NPCControl npcControl = mainNpc[i].GetComponent<NPCControl>();
			NPCInformation npcInformation = mainNpc[i].GetComponent<NPCInformation>();
			if (npcControl != null && npcInformation.NpcNameID == nameID) {
				return npcControl;
			}
		}
		return null;
	}

	public NPCInformation GetMainNpcInformation(NPCNameID nameID) {
		for (int i = 0; i < mainNpc.Length; i++) {
			NPCInformation npcInformation = mainNpc[i].GetComponent<NPCInformation>();
			if (npcInformation != null && npcInformation.NpcNameID == nameID) {
				return npcInformation;
			}
		}
		return null;
	}
}