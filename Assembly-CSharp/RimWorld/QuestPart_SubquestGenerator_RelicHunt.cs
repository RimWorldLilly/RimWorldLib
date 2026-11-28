using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FF RID: 4351
	public class QuestPart_SubquestGenerator_RelicHunt : QuestPart_SubquestGenerator
	{
		// Token: 0x06006C97 RID: 27799 RVA: 0x0026155C File Offset: 0x0025F75C
		protected override Slate InitSlate()
		{
			float var = 0f;
			if (this.useMapParentThreatPoints != null)
			{
				if (this.useMapParentThreatPoints.HasMap)
				{
					var = StorytellerUtility.DefaultThreatPointsNow(this.useMapParentThreatPoints.Map);
				}
				else if (Find.AnyPlayerHomeMap != null)
				{
					var = StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap);
				}
				else
				{
					var = StorytellerUtility.DefaultThreatPointsNow(Find.World);
				}
			}
			Slate slate = new Slate();
			slate.Set<float>("points", var, false);
			slate.Set<Precept_Relic>(this.relicSlateName, this.relic, false);
			return slate;
		}

		// Token: 0x06006C98 RID: 27800 RVA: 0x002615DC File Offset: 0x0025F7DC
		protected override QuestScriptDef GetNextSubquestDef()
		{
			this.ShuffleQueue();
			QuestScriptDef questScriptDef = this.questQueue.First<QuestScriptDef>();
			MapParent mapParent = this.useMapParentThreatPoints;
			IIncidentTarget target;
			if (mapParent != null && mapParent.HasMap)
			{
				target = this.useMapParentThreatPoints.Map;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				target = Find.AnyPlayerHomeMap;
			}
			else
			{
				target = Find.World;
			}
			if (!questScriptDef.CanRun(this.InitSlate(), target))
			{
				return null;
			}
			this.questQueue.RemoveAt(0);
			return questScriptDef;
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x00261650 File Offset: 0x0025F850
		private void ShuffleQueue()
		{
			this.questQueue.Clear();
			if (this.subquestDefs.Count == 1)
			{
				this.questQueue.AddRange(this.subquestDefs);
				return;
			}
			Quest quest = (from q in this.quest.GetSubquests(null)
			orderby q.appearanceTick descending
			select q).FirstOrDefault<Quest>();
			QuestScriptDef questScriptDef = (quest != null) ? quest.root : null;
			int i = 100;
			while (i > 0)
			{
				i--;
				this.questQueue.Clear();
				this.questQueue.AddRange(this.subquestDefs.InRandomOrder(null));
				if (questScriptDef == null || this.questQueue.First<QuestScriptDef>() != questScriptDef)
				{
					break;
				}
			}
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x00261714 File Offset: 0x0025F914
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			base.DoDebugWindowContents(innerRect, ref curY);
			if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Shuffle subquest queue", true, true, true, null))
			{
				this.ShuffleQueue();
			}
			curY += 29f;
			if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Log subquest queue", true, true, true, null))
			{
				if (this.questQueue.Count == 0)
				{
					this.ShuffleQueue();
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.questQueue.Count; i++)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(" ->");
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear() + this.questQueue[i].defName);
				}
				Log.Message(stringBuilder.ToString());
			}
			curY += 29f;
		}

		// Token: 0x06006C9B RID: 27803 RVA: 0x00261820 File Offset: 0x0025FA20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_Values.Look<string>(ref this.relicSlateName, "relicSlateName", null, false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.questQueue, "questQueue", LookMode.Def, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.useMapParentThreatPoints, "useMapParentThreatPoints", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.questQueue == null)
			{
				this.questQueue = new List<QuestScriptDef>();
			}
		}

		// Token: 0x0400451F RID: 17695
		private List<QuestScriptDef> questQueue = new List<QuestScriptDef>();

		// Token: 0x04004520 RID: 17696
		public Precept_Relic relic;

		// Token: 0x04004521 RID: 17697
		public string relicSlateName;

		// Token: 0x04004522 RID: 17698
		public MapParent useMapParentThreatPoints;
	}
}
