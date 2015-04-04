using UnityEngine;
using System.Collections;
using GameUtilities.AnchorPoint;
using GameUtilities;

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
		Mission13
	}

	public delegate bool MissionCondition();

	namespace Database {
		public struct MissionDatabase {
			private static MissionData[] missionDataList = new MissionData[13];
			public static MissionData[] MissionDataList {
				get { return missionDataList; }
			}

			public static void Initialize() {
				missionDataList[0] = new MissionData(
					"Mission 1",
					"Meet the Characters",
					"Talk to every character at least once",
					-1,
					MissionNameID.Mission1,
					Statistics.zero
				);

				missionDataList[1] = new MissionData(
					"Mission 2", 
					"Learn more about Andy",
					"Talk to Andy at least five times.",
					180,
					MissionNameID.Mission2,
					Statistics.zero
				);

				missionDataList[2] = new MissionData(
					"Mission 3",
					"Get Andy what He needs",
					"Hide and Seek Mini Game Incoming!",
                    720,
					MissionNameID.Mission3,
					Statistics.zero
				);

				missionDataList[3] = new MissionData(
					"Mission 4",
					"Learn more about Noelle",
					"Talk to Noelle at least five times.",
					180,
					MissionNameID.Mission4,
					Statistics.zero
				);

				missionDataList[4] = new MissionData(
					"Mission 5",
					"Get Noelle what she needs",
                    "Hide and Seek Mini Game Incoming!",
                    720,
					MissionNameID.Mission5,
					Statistics.zero
				);

				missionDataList[5] = new MissionData(
					"Mission 6",
					"Learn more about Franz",
					"Talk to Franz at least five times.",
					180,
					MissionNameID.Mission6,
					Statistics.zero
				);

				missionDataList[6] = new MissionData(
					"Mission 7",
					"Get Franz what she needs",
                    "Hide and Seek Mini Game Incoming!",
                    720,
					MissionNameID.Mission7,
					Statistics.zero
				);

				missionDataList[7] = new MissionData(
					"Mission 8",
					"Learn more about Bart",
					"Talk to Bart at least five times.",
					180,
					MissionNameID.Mission8,
					Statistics.zero
				);

				missionDataList[8] = new MissionData(
					"Mission 9",
					"Get Bart what he needs",
                    "Hide and Seek Mini Game Incoming!",
                    720,
					MissionNameID.Mission9,
					Statistics.zero
				);

				missionDataList[9] = new MissionData(
					"Mission 10",
					"Learn more about Jenevieve",
					"Talk to Jenevieve at least five times.",
					180,
					MissionNameID.Mission10,
					Statistics.zero
				);

				missionDataList[10] = new MissionData(
					"Mission 11",
					"Get Jenevieve what she needs",
                    "Hide and Seek Mini Game Incoming!",
                    720,
					MissionNameID.Mission11,
					Statistics.zero
				);

				missionDataList[11] = new MissionData(
					"Mission 12",
					"Learn more about Maxine",
					"Talk to Maxine at least five times.",
					180,
					MissionNameID.Mission12,
					Statistics.zero
				);

				missionDataList[12] = new MissionData(
					"Mission 13",
					"Get Maxine what she needs",
                    "Hide and Seek Mini Game Incoming!",
					720,
					MissionNameID.Mission13,
					Statistics.zero
				);
			}

			public static MissionData GetMission(MissionNameID missionID) {
				for (int i = 0; i < missionDataList.Length; i++) {
					if (missionDataList[i] != null && missionDataList[i].MissionNameID == missionID) {
						return missionDataList[i];
					}
				}
				return null;
			}
		}
	}

	public class MissionData {
		public MissionData() {
			missionTitle = string.Empty;
			missionName = string.Empty;
			missionDesc = string.Empty;
			missionNameID = MissionNameID.None;
		}

		public MissionData(string title, string name, string desc, float duration, MissionNameID nameID, Statistics reward) {
			missionTitle = title;
			missionName = name;
			missionDesc = desc;
			missionDuration = duration;
			missionNameID = nameID;
			missionReward = reward;
		}

		private string missionTitle;
		public string MissionTitle {
			get { return missionTitle; }
		}

		private string missionName;
		public string MissionName {
			get { return missionName; }
		}

		private string missionDesc;
		public string MissionDesc {
			get { return missionDesc; }
		}

		private MissionNameID missionNameID;
		public MissionNameID MissionNameID {
			get { return missionNameID; }
		}

		private float missionDuration;
		public float MissionDuration {
			get { return missionDuration; }
		}

		private Statistics missionReward;
		public Statistics MissionReward {
			get { return missionReward; }
		}

		private MissionCondition missionCondition;
		public MissionCondition MissionCondition {
			get { return missionCondition; }
			set { missionCondition = value; }
		}

		public void RunMissionTitle(FadeTransition fadeT) {
			UserInterface.RunTitle(missionTitle, missionName, missionDesc, fadeT);
		}
	}
}