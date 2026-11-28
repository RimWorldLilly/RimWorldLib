using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDE RID: 3806
	public class CompNoctolEyes : ThingComp
	{
		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x060063A5 RID: 25509 RVA: 0x0023C04D File Offset: 0x0023A24D
		public CompProperties_NoctolEyes Props
		{
			get
			{
				return (CompProperties_NoctolEyes)this.props;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x060063A6 RID: 25510 RVA: 0x0023C05A File Offset: 0x0023A25A
		public Pawn Parent
		{
			get
			{
				if (this.parentPawn == null)
				{
					this.parentPawn = (this.parent as Pawn);
				}
				return this.parentPawn;
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x060063A7 RID: 25511 RVA: 0x0023C07B File Offset: 0x0023A27B
		public float EyeBrightness
		{
			get
			{
				if (!this.Parent.Downed)
				{
					return 1f - this.hediff.Severity;
				}
				return 0f;
			}
		}

		// Token: 0x060063A8 RID: 25512 RVA: 0x0023C0A4 File Offset: 0x0023A2A4
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				if (this.mote == null || this.mote.Destroyed)
				{
					this.mote = MoteMaker.MakeAttachedOverlay(this.parent, this.Props.moteDef, Vector3.zero, 1f, -1f);
					this.mote.link1.rotateWithTarget = true;
				}
				if (this.hediff == null)
				{
					this.Parent.health.hediffSet.TryGetHediff(HediffDefOf.LightExposure, out this.hediff);
				}
				this.mote.instanceColor = new Color(1f, 1f, 1f, this.EyeBrightness);
				this.mote.Maintain();
			}
		}

		// Token: 0x04003801 RID: 14337
		private Mote mote;

		// Token: 0x04003802 RID: 14338
		private Hediff hediff;

		// Token: 0x04003803 RID: 14339
		private Pawn parentPawn;
	}
}
