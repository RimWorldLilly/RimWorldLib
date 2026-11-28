using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004CE RID: 1230
	public class InfectionPathway : IExposable
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x000CB927 File Offset: 0x000C9B27
		public InfectionPathwayDef Def
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x000CB92F File Offset: 0x000C9B2F
		public bool SourceWasPawn
		{
			get
			{
				return this.sourcePawnKind != null;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x000CB93A File Offset: 0x000C9B3A
		public PawnKindDef SourcePawnKind
		{
			get
			{
				return this.sourcePawnKind;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x000CB942 File Offset: 0x000C9B42
		public Pawn OwnerPawn
		{
			get
			{
				return this.ownerPawn;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x000CB94A File Offset: 0x000C9B4A
		public int AgeTicks
		{
			get
			{
				return this.ageTicks;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x000CB952 File Offset: 0x000C9B52
		public bool Expired
		{
			get
			{
				return this.ageTicks > this.def.ExpiryTicks;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x000CB967 File Offset: 0x000C9B67
		public float AgePercent
		{
			get
			{
				return Mathf.Clamp01((float)this.ageTicks / (float)this.def.ExpiryTicks);
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000CB982 File Offset: 0x000C9B82
		public InfectionPathway()
		{
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000CB98A File Offset: 0x000C9B8A
		public InfectionPathway(InfectionPathwayDef def, Pawn ownerPawn, PawnKindDef sourcePawnKind = null)
		{
			this.def = def;
			this.sourcePawnKind = sourcePawnKind;
			this.ownerPawn = ownerPawn;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000CB9A8 File Offset: 0x000C9BA8
		public string GetExplanation(HediffDef hediff)
		{
			foreach (HediffInfectionPathway hediffInfectionPathway in hediff.possiblePathways)
			{
				if (hediffInfectionPathway.PathwayDef == this.def)
				{
					if (this.sourcePawnKind != null)
					{
						return hediffInfectionPathway.Explanation.Formatted(this.sourcePawnKind.Named("SOURCEKIND"));
					}
					return hediffInfectionPathway.Explanation.Formatted(Array.Empty<NamedArgument>());
				}
			}
			Log.ErrorOnce("Failed to get an explanation for infection vector " + this.def.defName + " for hediff def " + hediff.defName, 85784634);
			return null;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000CBA74 File Offset: 0x000C9C74
		public void TickInterval(int delta)
		{
			this.ageTicks += delta;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000CBA84 File Offset: 0x000C9C84
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Defs.Look<InfectionPathwayDef>(ref this.def, "def");
			Scribe_Defs.Look<PawnKindDef>(ref this.sourcePawnKind, "sourcePawnKind");
			Scribe_References.Look<Pawn>(ref this.ownerPawn, "ownerPawn", false);
		}

		// Token: 0x040019C4 RID: 6596
		private InfectionPathwayDef def;

		// Token: 0x040019C5 RID: 6597
		private int ageTicks;

		// Token: 0x040019C6 RID: 6598
		private PawnKindDef sourcePawnKind;

		// Token: 0x040019C7 RID: 6599
		private Pawn ownerPawn;
	}
}
