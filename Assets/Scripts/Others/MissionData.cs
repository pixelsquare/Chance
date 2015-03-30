using UnityEngine;
using System.Collections;

namespace Mission {
	public enum MissionNameID {
		None,
		Mission1,
		Mission2,
		Mission3,
		Mission4,
		Mission5,
		Mission6,
		Mission7,
		Mission8,
		Mission9,
		Mission10,
		Mission11,
		Mission12,
		Mission13,
		SideMission1,
		SideMission2
	}

	public delegate void MissionStart();
	public delegate void MissionMid();
	public delegate bool MissionEnded();
	public delegate void MissionSuccess();
	public delegate void MissionFailed();
	public delegate void MissionResut();

	namespace Database {
		public struct MissionDatabase {
			public static MissionData[] missionData = new MissionData[13];
			public static MissionData[] MissionData {
				get { return missionData; }
			}

			public static MissionData[] sideMissionData = new MissionData[3];
			public static MissionData[] SideMissionData {
				get { return sideMissionData; }
			}

			public static void Initialize() {
				missionData[0] = new MissionData(
					"Meet the Characters",
					"Talk to every character at least once",
					10,
					MissionNameID.Mission1,
					Statistics.zero,
					new Mission()
				);

				missionData[1] = new MissionData(
					"Learn more about Andy",
					"Talk to Andy at least five times.",
					120,
					MissionNameID.Mission2,
					Statistics.zero,
					new Mission()
				);

				missionData[2] = new MissionData(
					"Get Andy what He needs",
					"MINI GAME!!!",
					120,
					MissionNameID.Mission3,
					Statistics.zero,
					new Mission()
				);

				missionData[3] = new MissionData(
					"Learn more about Noelle",
					"Talk to Noelle at least five times.",
					120,
					MissionNameID.Mission4,
					Statistics.zero,
					new Mission()
				);

				missionData[4] = new MissionData(
					"Get Noelle what she needs",
					"MINI GAME!!!",
					120,
					MissionNameID.Mission5,
					Statistics.zero,
					new Mission()
				);

				missionData[5] = new MissionData(
					"Learn more about Franz",
					"Talk to Franz at least five times.",
					120,
					MissionNameID.Mission6,
					Statistics.zero,
					new Mission()
				);

				missionData[6] = new MissionData(
					"Get Franz what she needs",
					"MINI GAME!!!",
					120,
					MissionNameID.Mission7,
					Statistics.zero,
					new Mission()
				);

				missionData[7] = new MissionData(
					"Learn more about Bart",
					"Talk to Bart at least five times.",
					120,
					MissionNameID.Mission8,
					Statistics.zero,
					new Mission()
				);

				missionData[8] = new MissionData(
					"Get Bart what he needs",
					"MINI GAME!!!",
					120,
					MissionNameID.Mission9,
					Statistics.zero,
					new Mission()
				);

				missionData[9] = new MissionData(
					"Learn more about Jenevieve",
					"Talk to Jenevieve at least five times.",
					120,
					MissionNameID.Mission10,
					Statistics.zero,
					new Mission()
				);

				missionData[10] = new MissionData(
					"Get Jenevieve what she needs",
					"MINI GAME!!!",
					120,
					MissionNameID.Mission11,
					Statistics.zero,
					new Mission()
				);

				missionData[11] = new MissionData(
					"Learn more about Maxine",
					"Talk to Maxine at least five times.",
					120,
					MissionNameID.Mission12,
					Statistics.zero,
					new Mission()
				);

				missionData[12] = new MissionData(
					"Get Maxine what she needs",
					"MINI GAME!!!",
					120,
					MissionNameID.Mission13,
					Statistics.zero,
					new Mission()
				);

				// Side Mission

				sideMissionData[0] = new MissionData(
					"Hide 'n Seek",
					string.Empty,
					120,
					MissionNameID.SideMission1,
					Statistics.zero,
					new Mission()
				);

				sideMissionData[1] = new MissionData(
					"Easter Eggs",
					string.Empty,
					120,
					MissionNameID.SideMission2,
					Statistics.zero,
					new Mission()
				);
			}

			public static MissionData GetMission(MissionNameID missionID) {
				for (int i = 0; i < missionData.Length; i++) {
					if (missionData[i].MissionNameID == missionID) {
						return missionData[i];
					}
				}
				return null;
			}

			public static MissionData GetSideMission(MissionNameID missionID) {
				for (int i = 0; i < sideMissionData.Length; i++) {
					if (sideMissionData[i].MissionNameID == missionID) {
						return sideMissionData[i];
					}
				}
				return null;
			}

		}
	}

	public class MissionData {
		public MissionData() {
			missionName = string.Empty;
			missionDesc = string.Empty;
			missionDuration = 0f;
			missionNameID = MissionNameID.None;
			missionReward = Statistics.zero;
			mission = new Mission();
		}

		public MissionData(string name, string desc, float duration, MissionNameID nameID, Statistics reward, Mission condition) {
			missionName = name;
			missionDesc = desc;
			missionDuration = duration;
			missionNameID = nameID;
			missionReward = reward;
			mission = condition;
		}

		private string missionName;
		public string MissionName {
			get { return missionName; }
		}

		private string missionDesc;
		public string MissionDesc {
			get { return missionDesc; }
			set { missionDesc = value; }
		}

		private float missionDuration;
		public float MissionDuration {
			get { return missionDuration; }
		}

		private MissionNameID missionNameID;
		public MissionNameID MissionNameID {
			get { return missionNameID; }
		}

		private Statistics missionReward;
		public Statistics MissionReward {
			get { return missionReward; }
		}

		private Mission mission;
		public Mission Mission {
			get { return mission; }
			set { mission = value; }
		}
	}

	public struct Mission {
		public Mission(MissionStart start, MissionMid mid, MissionEnded end, MissionResut result, MissionSuccess success, MissionFailed failed) {
			missionStart = start;
			missionMid = mid;
			missionEnded = end;
			missionResult = result;
			missionSuccess = success;
			missionFailed = failed;
		}

		private MissionStart missionStart;
		public MissionStart MissionStart {
			get { return missionStart; }
		}

		private MissionMid missionMid;
		public MissionMid MissionMid {
			get { return missionMid; }
		}

		private MissionEnded missionEnded;
		public MissionEnded MissionEnded {
			get { return missionEnded; }
		}

		private MissionResut missionResult;
		public MissionResut MissionResult {
			get { return missionResult; }
		}

		private MissionSuccess missionSuccess;
		public MissionSuccess MissionSuccess {
			get { return missionSuccess; }
		}

		private MissionFailed missionFailed;
		public MissionFailed MissionFailed {
			get { return missionFailed; }
		}
	}
}