using NPC;
using NPC.Database;
using UnityEngine;

public class NPCManager : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Transform[] npc;
	[SerializeField]
	private BoxCollider firstFloor;
	[SerializeField]
	private BoxCollider secondFloor;

	# endregion Public Variables

	// Public Properties
	public Transform[] Npc{
		get { return npc; }
	}

	public BoxCollider FirstFloor {
		get { return firstFloor; }
	}

	public BoxCollider SecondFloor {
		get { return secondFloor; }
	}
	// --

	public static NPCManager current;

	private void Awake() {
		current = this;
	}

	public void EnableNpcs(bool enable) {
		for (int i = 0; i < npc.Length; i++) {
			NPCInformation npcInformation = npc[i].GetComponent<NPCInformation>();
			npcInformation.ObjectEnabled = enable;
		}
	}

	public Statistics GetNpcAverageStatistics() {
		int count = 0;
		Statistics[] tmpStat = new Statistics[npc.Length];
		for (int i = 0; i < npc.Length; i++) {
			if (npc[i] != null) {
				NPCInformation npcInformation = npc[i].GetComponent<NPCInformation>();
				// Add if npc has information
				if (npcInformation.BaseNpcData.NpcNameID != NPCNameID.None) {
					tmpStat[i] = NPCDatabase.GetNpc(npcInformation.BaseNpcData.NpcNameID).NpcStatistics;
					count++;
				}
			}
		}
		return Statistics.GetAverage(tmpStat, count);
	}

	public Transform GetNpc(NPCNameID nameID) {
		for (int i = 0; i < npc.Length; i++) {
			NPCInformation npcInformation = npc[i].GetComponent<NPCInformation>();
			if (npcInformation.BaseNpcData.NpcNameID == nameID) {
				return npc[i];
			}
		}
		return null;
	}

	public NPCControl GetNpcControl(NPCNameID nameID) {
		for (int i = 0; i < npc.Length; i++) {
			NPCControl npcControl = npc[i].GetComponent<NPCControl>();
			NPCInformation npcInformation = npc[i].GetComponent<NPCInformation>();
			if (npcControl != null && npcInformation.BaseNpcData.NpcNameID == nameID) {
				return npcControl;
			}
		}
		return null;
	}

	public NPCInformation GetNpcInformation(NPCNameID nameID) {
		for (int i = 0; i < npc.Length; i++) {
			NPCInformation npcInformation = npc[i].GetComponent<NPCInformation>();
			if (npcInformation != null && npcInformation.BaseNpcData.NpcNameID == nameID) {
				return npcInformation;
			}
		}
		return null;
	}
}