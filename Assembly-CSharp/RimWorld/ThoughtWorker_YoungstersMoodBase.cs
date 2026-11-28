using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7D RID: 3709
	public abstract class ThoughtWorker_YoungstersMoodBase : ThoughtWorker
	{
		// Token: 0x06006230 RID: 25136
		protected abstract FloatRange MoodRange();

		// Token: 0x06006231 RID: 25137 RVA: 0x0023514C File Offset: 0x0023334C
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x0023518C File Offset: 0x0023338C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (!p.IsColonist || p.Suspended || !p.DevelopmentalStage.Adult())
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenWithMoodInColony(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x002351E0 File Offset: 0x002333E0
		private int ChildrenWithMoodInColony(Pawn pawn)
		{
			int num = 0;
			ThoughtWorker_YoungstersMoodBase.tmpPawns.Clear();
			if (pawn.Spawned)
			{
				ThoughtWorker_YoungstersMoodBase.tmpPawns.AddRange(pawn.Map.mapPawns.FreeColonistsSpawned);
			}
			else
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null)
				{
					ThoughtWorker_YoungstersMoodBase.tmpPawns.AddRange(from p in caravan.PawnsListForReading
					where p.IsColonist
					select p);
				}
			}
			for (int i = 0; i < ThoughtWorker_YoungstersMoodBase.tmpPawns.Count; i++)
			{
				if (ThoughtWorker_YoungstersMoodBase.tmpPawns[i].RaceProps.Humanlike && !ThoughtWorker_YoungstersMoodBase.tmpPawns[i].DevelopmentalStage.Adult() && !PawnRelationDefOf.Parent.Worker.InRelation(ThoughtWorker_YoungstersMoodBase.tmpPawns[i], pawn) && ThoughtWorker_RelatedChildMoodBase.IsChildWithMood(ThoughtWorker_YoungstersMoodBase.tmpPawns[i], this.MoodRange()))
				{
					num++;
				}
			}
			ThoughtWorker_YoungstersMoodBase.tmpPawns.Clear();
			return num;
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x002352E2 File Offset: 0x002334E2
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenWithMoodInColony(p));
		}

		// Token: 0x0400361F RID: 13855
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
