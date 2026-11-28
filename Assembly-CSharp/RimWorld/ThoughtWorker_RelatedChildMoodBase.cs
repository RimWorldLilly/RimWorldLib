using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E78 RID: 3704
	public abstract class ThoughtWorker_RelatedChildMoodBase : ThoughtWorker
	{
		// Token: 0x06006218 RID: 25112 RVA: 0x00234EEA File Offset: 0x002330EA
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Suspended)
			{
				return ThoughtState.Inactive;
			}
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x00234F1C File Offset: 0x0023311C
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn pawn2 = p.relations.Children.FirstOrDefault((Pawn pawn) => ThoughtWorker_RelatedChildMoodBase.IsChildWithMood(pawn, this.MoodRange()));
			if (pawn2 != null)
			{
				return label.Formatted(p.Named("PAWN"), pawn2.Named("CHILD"));
			}
			return this.SingleChildLabel().Translate().CapitalizeFirst();
		}

		// Token: 0x0600621A RID: 25114
		protected abstract bool Active(Pawn p);

		// Token: 0x0600621B RID: 25115
		protected abstract string SingleChildLabel();

		// Token: 0x0600621C RID: 25116
		protected abstract FloatRange MoodRange();

		// Token: 0x0600621D RID: 25117 RVA: 0x00234F84 File Offset: 0x00233184
		protected int ChildrenWithMoodCount(Pawn parent)
		{
			int num = 0;
			if (parent.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn in parent.relations.Children)
				{
					if (ThoughtWorker_RelatedChildMoodBase.InSameMapOrCaravan(parent, pawn) && ThoughtWorker_RelatedChildMoodBase.IsChildWithMood(pawn, this.MoodRange()))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x00234FFC File Offset: 0x002331FC
		public static bool IsChildWithMood(Pawn child, FloatRange moodRange)
		{
			Pawn_NeedsTracker needs = child.needs;
			return ((needs != null) ? needs.mood : null) != null && child.DevelopmentalStage.Juvenile() && moodRange.Includes(child.needs.mood.CurLevelPercentage) && !(child.ParentHolder is Building_GrowthVat) && !(child.ParentHolder is Building_CryptosleepCasket);
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00235064 File Offset: 0x00233264
		public static bool InSameMapOrCaravan(Pawn pawn, Pawn other)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld != null && mapHeld == other.MapHeld)
			{
				return true;
			}
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan == other.GetCaravan();
		}
	}
}
