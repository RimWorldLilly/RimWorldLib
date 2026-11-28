using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001101 RID: 4353
	public class QuestPart_SubquestGenerator_Gravcores : QuestPart_SubquestGenerator
	{
		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06006CA2 RID: 27810 RVA: 0x002619B0 File Offset: 0x0025FBB0
		private int MTB
		{
			get
			{
				if (!this.givenFirstSubquest)
				{
					return 120000;
				}
				return 300000;
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06006CA3 RID: 27811 RVA: 0x002619C5 File Offset: 0x0025FBC5
		private int MinTime
		{
			get
			{
				if (!this.givenFirstSubquest)
				{
					return 300000;
				}
				return 900000;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06006CA4 RID: 27812 RVA: 0x002619DA File Offset: 0x0025FBDA
		private int MaxTime
		{
			get
			{
				if (!this.givenFirstSubquest)
				{
					return 480000;
				}
				return 1800000;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06006CA5 RID: 27813 RVA: 0x002619F0 File Offset: 0x0025FBF0
		protected override bool CanGenerateSubquest
		{
			get
			{
				if (!base.CanGenerateSubquest)
				{
					return false;
				}
				if ((float)Find.TickManager.TicksGame - this.lastSubquestTick < (float)this.MinTime)
				{
					return false;
				}
				using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GravEngine))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x00261A7C File Offset: 0x0025FC7C
		protected override QuestScriptDef GetNextSubquestDef()
		{
			this.tmpSubquestDefs.Clear();
			this.GetPossibleSubquests(this.tmpSubquestDefs);
			QuestScriptDef result;
			if (this.tmpSubquestDefs.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06006CA7 RID: 27815 RVA: 0x00261AB4 File Offset: 0x0025FCB4
		private void GetPossibleSubquests(List<QuestScriptDef> outList)
		{
			IEnumerable<Quest> subquests = this.quest.GetSubquests(null);
			IEnumerable<Quest> source = from x in subquests
			where x.State == QuestState.Ongoing
			select x;
			IEnumerable<Quest> source2 = from x in subquests
			where x.State == QuestState.EndedSuccess
			select x;
			int num = source.Count<Quest>() + source2.Count<Quest>();
			bool flag;
			if (!source.Any((Quest x) => x.root == QuestScriptDefOf.Gravcore_Mechhive))
			{
				flag = source2.Any((Quest x) => x.root == QuestScriptDefOf.Gravcore_Mechhive);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			bool flag3 = flag2 && num >= this.subquestDefs.Count;
			using (List<QuestScriptDef>.Enumerator enumerator = this.subquestDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestScriptDef def = enumerator.Current;
					QuestNode_Root_Gravcore questNode_Root_Gravcore = def.root as QuestNode_Root_Gravcore;
					if (questNode_Root_Gravcore != null && questNode_Root_Gravcore.requiredSubquestsGiven <= num && (flag3 || (!source2.Any((Quest x) => x.root == def) && !source.Any((Quest x) => x.root == def))) && (!flag2 || def != QuestScriptDefOf.Gravcore_Mechhive) && def.CanRun(this.InitSlate(), Find.World))
					{
						outList.Add(def);
					}
				}
			}
		}

		// Token: 0x06006CA8 RID: 27816 RVA: 0x00261C74 File Offset: 0x0025FE74
		protected override bool TryGenerateSubquest()
		{
			bool flag = base.TryGenerateSubquest();
			if (flag)
			{
				this.lastSubquestTick = (float)Find.TickManager.TicksGame;
				this.givenFirstSubquest = true;
				return flag;
			}
			Log.Warning("Failed to generate gravcore subquest, trying again in 6 hours");
			this.lastSubquestTick += 15000f;
			return flag;
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x00261CB4 File Offset: 0x0025FEB4
		protected override Slate InitSlate()
		{
			float var;
			if (Find.AnyPlayerHomeMap != null)
			{
				var = StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap);
			}
			else
			{
				var = StorytellerUtility.DefaultThreatPointsNow(Find.World);
			}
			Slate slate = new Slate();
			slate.Set<float>("points", var, false);
			return slate;
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x00261CF4 File Offset: 0x0025FEF4
		public override void QuestPartTick()
		{
			if (this.subquestDefs.Count == 0)
			{
				return;
			}
			if (base.Paused)
			{
				return;
			}
			if (this.CanGenerateSubquest && (Rand.MTBEventOccurs((float)this.MTB, 1f, 1f) || (float)Find.TickManager.TicksGame > this.lastSubquestTick + (float)this.MaxTime))
			{
				this.TryGenerateSubquest();
			}
		}

		// Token: 0x06006CAB RID: 27819 RVA: 0x00261D5A File Offset: 0x0025FF5A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.lastSubquestTick, "lastSubquestTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.givenFirstSubquest, "givenFirstSubquest", false, false);
		}

		// Token: 0x04004526 RID: 17702
		private float lastSubquestTick;

		// Token: 0x04004527 RID: 17703
		private bool givenFirstSubquest;

		// Token: 0x04004528 RID: 17704
		private const int MTBSubquestsTicks = 300000;

		// Token: 0x04004529 RID: 17705
		private const int MTBFirstSubquestTicks = 120000;

		// Token: 0x0400452A RID: 17706
		private const int MinTimeBetweenSubquests = 900000;

		// Token: 0x0400452B RID: 17707
		private const int MaxTimeBetweenSubquests = 1800000;

		// Token: 0x0400452C RID: 17708
		private const int MinTimeFirstSubquest = 300000;

		// Token: 0x0400452D RID: 17709
		private const int MaxTimeFirstSubquest = 480000;

		// Token: 0x0400452E RID: 17710
		private List<QuestScriptDef> tmpSubquestDefs = new List<QuestScriptDef>();
	}
}
