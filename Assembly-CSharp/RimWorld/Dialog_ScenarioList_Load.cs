using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDA RID: 7386
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x0600BDD7 RID: 48599 RVA: 0x00443137 File Offset: 0x00441337
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x0600BDD8 RID: 48600 RVA: 0x0044315C File Offset: 0x0044135C
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForScenario(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, delegate
			{
				Scenario obj;
				if (GameDataSaveLoader.TryLoadScenario(filePath, ScenarioCategory.CustomLocal, out obj))
				{
					this.scenarioReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x04006CB4 RID: 27828
		private Action<Scenario> scenarioReturner;
	}
}
