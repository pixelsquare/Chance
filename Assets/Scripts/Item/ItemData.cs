using UnityEngine;

namespace Item {
    public enum ItemNameID {
        None, Chrome, Donut, FanfictionNotebook, StrawberryMilkshake, USB, GrassJellyMilkTea, Guitar, HotCoffee, Markers, OrangeJuice, WhiteEarphones, IcedTea, Headset, Milk
    };

	namespace Database {
		public struct ItemDatabase {
			private static ItemData[] itemDataInfoList = new ItemData[14];
			public static ItemData[] ItemDataInfoList {
				get { return itemDataInfoList; }
			}

			public static void Initialize() {
                itemDataInfoList[0] = new ItemData(
                    "Chrome of Enlightenment",
                    Resources.IconDatabase.ChromeIcon,
                    ItemNameID.Chrome,
                    "The Item that will enlighten your way.",
                    new Statistics(),
                    new Statistics()
                );

                itemDataInfoList[1] = new ItemData(
                    "Donut of OZ",
                    Resources.IconDatabase.DonutIcon,
                    ItemNameID.Donut,
                    "The donut that came from the world of OZ.",
                    new Statistics(),
                    new Statistics()
                );

                itemDataInfoList[2] = new ItemData(
                    "Fanfiction Notebook",
                    Resources.IconDatabase.FanfictionNotebookIcon,
                    ItemNameID.FanfictionNotebook,
                    "Where a certain person's imaginative secret lies...",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[3] = new ItemData(
                    "Strawberry Milkshake",
                    Resources.IconDatabase.StrawberryMilkshakeIcon,
                    ItemNameID.StrawberryMilkshake,
                    "Favored drink for a sweet tooth",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[4] = new ItemData(
                    "USB",
                    Resources.IconDatabase.USBIcon,
                    ItemNameID.USB,
                    "Someone's Life",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[5] = new ItemData(
                    "Grass Jelly Milk Tea",
                    Resources.IconDatabase.GrassJellyMilkteaIcon,
                    ItemNameID.GrassJellyMilkTea,
                    "Tea made from Grass",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[6] = new ItemData(
                    "Guitar",
                    Resources.IconDatabase.GuitarPickIcon,
                    ItemNameID.Guitar,
                    "The Sound of Muuuuusic~",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[7] = new ItemData(
                    "Hot Coffee",
                    Resources.IconDatabase.HotCoffeeIcon,
                    ItemNameID.HotCoffee,
                    "If it's not Starbucks, then it's either Coffee Bean, Seattle's Best or Bo's Coffee. If it's not one of those, it's not coffee.",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[8] = new ItemData(
                    "Markers",
                    Resources.IconDatabase.MarkersIcon,
                    ItemNameID.Markers,
                    "Something that can make your world colorful",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[9] = new ItemData(
                    "Orange Juice",
                    Resources.IconDatabase.OrangeJuiceIcon,
                    ItemNameID.OrangeJuice,
                    "Drink perfect for meals",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[10] = new ItemData(
                    "White Earphones",
                    Resources.IconDatabase.WhiteEarphonesIcon,
                    ItemNameID.WhiteEarphones,
                    "It's either this or the headset. Take your pick.",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[11] = new ItemData(
                    "Iced Tea",
                    Resources.IconDatabase.IcedTeaIcon,
                    ItemNameID.IcedTea,
                    "Drink perfect during summer",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[12] = new ItemData(
                    "Headset",
                    Resources.IconDatabase.HeadphonesIcon,
                    ItemNameID.Headset,
                    "It's either this or the earphones. Take your pick.",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );

                itemDataInfoList[13] = new ItemData(
                    "Milk",
                    Resources.IconDatabase.MilkIcon,
                    ItemNameID.Milk,
                    "Not for Lactose Intolerant",
                    new Statistics(15, 0),
                    new Statistics(0, 10)
                );
			}

			public static ItemData GetItem(ItemNameID itemID) {
				ItemData tmpItemData = new ItemData();
				for (int i = 0; i < itemDataInfoList.Length; i++) {
					if (itemDataInfoList[i].ItemNameID != ItemNameID.None && itemDataInfoList[i].ItemNameID == itemID) {
						tmpItemData = itemDataInfoList[i];
					}
				}
				return tmpItemData;
			}
		}
	}

	public class ItemData {
		public ItemData() {
			itemName = string.Empty;
			itemIcon = null;
			itemNameID = ItemNameID.None;
			itemDesc = string.Empty;
			itemAddedStat = Statistics.zero;
			itemDebuffStat = Statistics.zero;
		}

		public ItemData(string name, Texture2D icon, ItemNameID nameID, string desc, Statistics addStat, Statistics debuffStat) {
			itemName = name;
			itemIcon = icon;
			itemNameID = nameID;
			itemDesc = desc;
			itemAddedStat = addStat;
			itemDebuffStat = debuffStat;
		}

		private string itemName;
		public string ItemName {
			get { return itemName; }
		}

		private Texture2D itemIcon;
		public Texture2D ItemIcon {
			get { return itemIcon; }
		}

		private ItemNameID itemNameID;
		public ItemNameID ItemNameID {
			get { return itemNameID; }
		}

		private string itemDesc;
		public string ItemDesc {
			get { return itemDesc; }
		}

		private Statistics itemAddedStat;
		public Statistics ItemAddedStat {
			get { return itemAddedStat; }
		}

		private Statistics itemDebuffStat;
		public Statistics ItemDebuffStat {
			get { return itemDebuffStat; }
		}
	}
}