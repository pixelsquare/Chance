using UnityEngine;
using System.Collections;

namespace GameEnd {
	public enum EndingType {
		None,
		Ending1,
		Ending2,
		Ending3,
		Ending4,
		TrueEnding
	}

	namespace Database {
		public struct GameEndDatabase {
			private static GameEndData[] gameEndDataList = new GameEndData[5];
			public static GameEndData[] GameEndData {
				get { return gameEndDataList; }
			}

			public static void Initialize() {
				gameEndDataList[0] = new GameEndData(
					"The Ending 1",
					EndingType.Ending1,
					"This Ending 1 Short Summary"
				);

				gameEndDataList[1] = new GameEndData(
					"The Ending 2",
					EndingType.Ending2,
					"This Ending 2 Short Summary"
				);

				gameEndDataList[2] = new GameEndData(
					"The Ending 3",
					EndingType.Ending3,
					"This Ending 3 Short Summary"
				);

				gameEndDataList[3] = new GameEndData(
					"The Ending 4",
					EndingType.Ending4,
					"This Ending 4 Short Summary"
				);

				gameEndDataList[4] = new GameEndData(
					"True Ending",
					EndingType.TrueEnding,
					"True Ending Short Summary"
				);
			}

			public static GameEndData GetGameEnd(EndingType gameEndType) {
				GameEndData gameEndData = new GameEndData(); 
				for (int i = 0; i < gameEndDataList.Length; i++) {
					if (gameEndDataList[i].EndingType == gameEndType) {
						gameEndData = gameEndDataList[i];
					}
				}
				return gameEndData;
			}
		}
	}

	public struct GameEndData {
		public GameEndData(string name, EndingType type, string desc) {
			endingTitle = name;
			endingType = type;
			endingDesc = desc;
		}

		private string endingTitle;
		public string EndingTitle {
			get { return endingTitle; }
		}

		private EndingType endingType;
		public EndingType EndingType {
			get { return endingType; }
		}

		private string endingDesc;
		public string EndingDesc {
			get { return endingDesc; }
		}
	}
}