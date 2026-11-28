using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DA RID: 474
	public class WorkTypeDef : Def
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00043F76 File Offset: 0x00042176
		public bool VisibleCurrently
		{
			get
			{
				if (this.cachedFrameVisibleCurrently == -1 || this.cachedFrameVisibleCurrently < Time.frameCount - 30)
				{
					this.cachedVisibleCurrently = this.VisibleNow(null, null);
					this.cachedFrameVisibleCurrently = Time.frameCount;
				}
				return this.cachedVisibleCurrently;
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00043FB0 File Offset: 0x000421B0
		public bool VisibleNow(Pawn ignorePawn = null, Pawn alsoCheckPawn = null)
		{
			if (!this.visible)
			{
				return false;
			}
			if (this.visibleOnlyWithChildrenInColony)
			{
				bool flag = false;
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && pawn != ignorePawn && pawn.DevelopmentalStage.Juvenile())
					{
						flag = true;
						break;
					}
				}
				if (alsoCheckPawn != null && alsoCheckPawn.DevelopmentalStage.Juvenile())
				{
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0004404C File Offset: 0x0004224C
		public override IEnumerable<string> ConfigErrors()
		{
			WorkTypeDef.<ConfigErrors>d__18 <ConfigErrors>d__ = new WorkTypeDef.<ConfigErrors>d__18(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0004405C File Offset: 0x0004225C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000440E8 File Offset: 0x000422E8
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x04000D6B RID: 3435
		public WorkTags workTags;

		// Token: 0x04000D6C RID: 3436
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000D6D RID: 3437
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x04000D6E RID: 3438
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04000D6F RID: 3439
		[MustTranslate]
		public string verb;

		// Token: 0x04000D70 RID: 3440
		public bool visible = true;

		// Token: 0x04000D71 RID: 3441
		public bool visibleOnlyWithChildrenInColony;

		// Token: 0x04000D72 RID: 3442
		public int naturalPriority;

		// Token: 0x04000D73 RID: 3443
		public bool alwaysStartActive;

		// Token: 0x04000D74 RID: 3444
		public bool requireCapableColonist;

		// Token: 0x04000D75 RID: 3445
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x04000D76 RID: 3446
		public bool disabledForSlaves;

		// Token: 0x04000D77 RID: 3447
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

		// Token: 0x04000D78 RID: 3448
		[Unsaved(false)]
		private bool cachedVisibleCurrently;

		// Token: 0x04000D79 RID: 3449
		[Unsaved(false)]
		private int cachedFrameVisibleCurrently = -1;
	}
}
