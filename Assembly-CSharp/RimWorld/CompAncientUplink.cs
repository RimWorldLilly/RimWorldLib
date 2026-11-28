using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001809 RID: 6153
	public class CompAncientUplink : ThingComp
	{
		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x0600999F RID: 39327 RVA: 0x00378E90 File Offset: 0x00377090
		private List<QuestScriptDef> ScannerQuests
		{
			get
			{
				List<QuestScriptDef> result;
				if ((result = this.scannerQuests) == null)
				{
					result = (this.scannerQuests = QuestUtility.GetGiverQuests(QuestGiverTag.OrbitalScanner).ToList<QuestScriptDef>());
				}
				return result;
			}
		}

		// Token: 0x060099A0 RID: 39328 RVA: 0x00378EBC File Offset: 0x003770BC
		public override void Notify_Hacked(Pawn hacker = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float points = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map);
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromUplink".Translate(hacker.Named("HACKER")), false);
			QuestScriptDef questScriptDef = this.ScannerQuests.RandomElementByWeight((QuestScriptDef q) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(q, points, Find.World.StoryState));
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
			if (!quest.hidden && questScriptDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod", null, false));
			}
		}

		// Token: 0x04005BA2 RID: 23458
		private List<QuestScriptDef> scannerQuests;
	}
}
