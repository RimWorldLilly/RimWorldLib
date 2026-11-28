using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200223F RID: 8767
	public class QuestNode_SpawnMechCluster : QuestNode
	{
		// Token: 0x0600D8F1 RID: 55537 RVA: 0x00505C9C File Offset: 0x00503E9C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_MechCluster questPart_MechCluster = new QuestPart_MechCluster();
			questPart_MechCluster.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_MechCluster.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_MechCluster.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_MechCluster.sketch = this.GenerateSketch(slate);
			questPart_MechCluster.dropSpot = (this.dropSpot.GetValue(slate) ?? IntVec3.Invalid);
			QuestGen.quest.AddPart(questPart_MechCluster);
			string text = "";
			if (questPart_MechCluster.sketch.pawns != null)
			{
				text += PawnUtility.PawnKindsToLineList(from m in questPart_MechCluster.sketch.pawns
				select m.kindDef, "  - ", ColoredText.ThreatColor);
			}
			string[] array = (from t in questPart_MechCluster.sketch.buildingsSketch.Things
			where GenHostility.IsDefMechClusterThreat(t.def)
			group t by t.def.label).Select(delegate(IGrouping<string, SketchThing> grp)
			{
				int num = grp.Count<SketchThing>();
				return num.ToString() + " " + ((num > 1) ? Find.ActiveLanguageWorker.Pluralize(grp.Key, num) : grp.Key);
			}).ToArray<string>();
			if (array.Any<string>())
			{
				if (text != "")
				{
					text += "\n";
				}
				text += array.ToLineList(ColoredText.ThreatColor, "  - ");
			}
			if (text != "")
			{
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allThreats", text)
				});
			}
		}

		// Token: 0x0600D8F2 RID: 55538 RVA: 0x00505E90 File Offset: 0x00504090
		private MechClusterSketch GenerateSketch(Slate slate)
		{
			return MechClusterGenerator.GenerateClusterSketch(this.points.GetValue(slate) ?? slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), true, false);
		}

		// Token: 0x0600D8F3 RID: 55539 RVA: 0x00505EE1 File Offset: 0x005040E1
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && Faction.OfMechanoids != null && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x04008A97 RID: 35479
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A98 RID: 35480
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04008A99 RID: 35481
		public SlateRef<float?> points;

		// Token: 0x04008A9A RID: 35482
		public SlateRef<IntVec3?> dropSpot;
	}
}
