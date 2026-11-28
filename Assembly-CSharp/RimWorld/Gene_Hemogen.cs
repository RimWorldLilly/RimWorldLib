using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207E RID: 8318
	public class Gene_Hemogen : Gene_Resource, IGeneResourceDrain
	{
		// Token: 0x17002598 RID: 9624
		// (get) Token: 0x0600D1BB RID: 53691 RVA: 0x004D14CF File Offset: 0x004CF6CF
		public Gene_Resource Resource
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17002599 RID: 9625
		// (get) Token: 0x0600D1BC RID: 53692 RVA: 0x004D14D2 File Offset: 0x004CF6D2
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x1700259A RID: 9626
		// (get) Token: 0x0600D1BD RID: 53693 RVA: 0x004D14DA File Offset: 0x004CF6DA
		public bool CanOffset
		{
			get
			{
				return this.Active && !this.pawn.Deathresting;
			}
		}

		// Token: 0x1700259B RID: 9627
		// (get) Token: 0x0600D1BE RID: 53694 RVA: 0x004D14F4 File Offset: 0x004CF6F4
		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene".Translate() + ")";
			}
		}

		// Token: 0x1700259C RID: 9628
		// (get) Token: 0x0600D1BF RID: 53695 RVA: 0x004D1524 File Offset: 0x004CF724
		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		// Token: 0x1700259D RID: 9629
		// (get) Token: 0x0600D1C0 RID: 53696 RVA: 0x004D1531 File Offset: 0x004CF731
		public override float InitialResourceMax
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700259E RID: 9630
		// (get) Token: 0x0600D1C1 RID: 53697 RVA: 0x004D1538 File Offset: 0x004CF738
		public override float MinLevelForAlert
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x1700259F RID: 9631
		// (get) Token: 0x0600D1C2 RID: 53698 RVA: 0x004D153F File Offset: 0x004CF73F
		public override float MaxLevelOffset
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170025A0 RID: 9632
		// (get) Token: 0x0600D1C3 RID: 53699 RVA: 0x004D1548 File Offset: 0x004CF748
		protected override Color BarColor
		{
			get
			{
				return new ColorInt(138, 3, 3).ToColor;
			}
		}

		// Token: 0x170025A1 RID: 9633
		// (get) Token: 0x0600D1C4 RID: 53700 RVA: 0x004D156C File Offset: 0x004CF76C
		protected override Color BarHighlightColor
		{
			get
			{
				return new ColorInt(145, 42, 42).ToColor;
			}
		}

		// Token: 0x0600D1C5 RID: 53701 RVA: 0x004D158F File Offset: 0x004CF78F
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Hemogen"))
			{
				return;
			}
			base.PostAdd();
			this.Reset();
		}

		// Token: 0x0600D1C6 RID: 53702 RVA: 0x004D15AC File Offset: 0x004CF7AC
		public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (thing.def.IsMeat)
			{
				IngestibleProperties ingestible = thing.def.ingestible;
				bool flag;
				if (ingestible == null)
				{
					flag = false;
				}
				else
				{
					ThingDef sourceDef = ingestible.sourceDef;
					bool? flag2;
					if (sourceDef == null)
					{
						flag2 = null;
					}
					else
					{
						RaceProperties race = sourceDef.race;
						flag2 = ((race != null) ? new bool?(race.Humanlike) : null);
					}
					bool? flag3 = flag2;
					flag = flag3.GetValueOrDefault();
				}
				if (flag)
				{
					GeneUtility.OffsetHemogen(this.pawn, 0.0375f * thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)numTaken, true);
				}
			}
		}

		// Token: 0x0600D1C7 RID: 53703 RVA: 0x004D1636 File Offset: 0x004CF836
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			GeneResourceDrainUtility.TickResourceDrainInterval(this, delta);
		}

		// Token: 0x0600D1C8 RID: 53704 RVA: 0x004D1646 File Offset: 0x004CF846
		public override void SetTargetValuePct(float val)
		{
			this.targetValue = Mathf.Clamp(val * this.Max, 0f, this.Max - this.MaxLevelOffset);
		}

		// Token: 0x0600D1C9 RID: 53705 RVA: 0x004D166D File Offset: 0x004CF86D
		public bool ShouldConsumeHemogenNow()
		{
			return this.Value < this.targetValue;
		}

		// Token: 0x0600D1CA RID: 53706 RVA: 0x004D167D File Offset: 0x004CF87D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Gene_Hemogen.<GetGizmos>d__26 <GetGizmos>d__ = new Gene_Hemogen.<GetGizmos>d__26(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600D1CB RID: 53707 RVA: 0x004D168D File Offset: 0x004CF88D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.hemogenPacksAllowed, "hemogenPacksAllowed", true, false);
		}

		// Token: 0x04008507 RID: 34055
		public bool hemogenPacksAllowed = true;
	}
}
