using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001786 RID: 6022
	public static class ScenarioFiles
	{
		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x060095F8 RID: 38392 RVA: 0x00367F18 File Offset: 0x00366118
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x060095F9 RID: 38393 RVA: 0x00367F1F File Offset: 0x0036611F
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x060095FA RID: 38394 RVA: 0x00367F28 File Offset: 0x00366128
		public static void RecacheData()
		{
			ScenarioFiles.scenariosLocal.Clear();
			using (IEnumerator<FileInfo> enumerator = GenFilePaths.AllCustomScenarioFiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scenario item;
					if (GameDataSaveLoader.TryLoadScenario(enumerator.Current.FullName, ScenarioCategory.CustomLocal, out item))
					{
						ScenarioFiles.scenariosLocal.Add(item);
					}
				}
			}
			ScenarioFiles.scenariosWorkshop.Clear();
			foreach (WorkshopItem workshopItem in WorkshopItems.AllSubscribedItems)
			{
				WorkshopItem_Scenario workshopItem_Scenario = workshopItem as WorkshopItem_Scenario;
				if (workshopItem_Scenario != null)
				{
					ScenarioFiles.scenariosWorkshop.Add(workshopItem_Scenario.GetScenario());
				}
			}
		}

		// Token: 0x04005A32 RID: 23090
		private static readonly List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x04005A33 RID: 23091
		private static readonly List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
