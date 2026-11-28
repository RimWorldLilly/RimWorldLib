using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001787 RID: 6023
	public static class ScenarioLister
	{
		// Token: 0x060095FC RID: 38396 RVA: 0x00367FFE File Offset: 0x003661FE
		public static IEnumerable<Scenario> AllScenarios()
		{
			return new ScenarioLister.<AllScenarios>d__1(-2);
		}

		// Token: 0x060095FD RID: 38397 RVA: 0x00368007 File Offset: 0x00366207
		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.<ScenariosInCategory>d__2 <ScenariosInCategory>d__ = new ScenarioLister.<ScenariosInCategory>d__2(-2);
			<ScenariosInCategory>d__.<>3__cat = cat;
			return <ScenariosInCategory>d__;
		}

		// Token: 0x060095FE RID: 38398 RVA: 0x00368018 File Offset: 0x00366218
		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			using (IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.scenario == scen)
					{
						return true;
					}
				}
			}
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == scenario)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060095FF RID: 38399 RVA: 0x003680AC File Offset: 0x003662AC
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x06009600 RID: 38400 RVA: 0x003680B4 File Offset: 0x003662B4
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x06009601 RID: 38401 RVA: 0x003680C4 File Offset: 0x003662C4
		private static void RecacheData()
		{
			ScenarioLister.dirty = false;
			int num = ScenarioLister.ScenarioListHash();
			ScenarioFiles.RecacheData();
			if (ScenarioLister.ScenarioListHash() != num && !LongEventHandler.ShouldWaitForEvent)
			{
				Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
				if (page_SelectScenario != null)
				{
					page_SelectScenario.Notify_ScenarioListChanged();
				}
			}
		}

		// Token: 0x06009602 RID: 38402 RVA: 0x00368108 File Offset: 0x00366308
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Token: 0x04005A34 RID: 23092
		private static bool dirty = true;
	}
}
