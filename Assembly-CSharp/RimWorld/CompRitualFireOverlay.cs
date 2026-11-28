using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001885 RID: 6277
	[StaticConstructorOnStartup]
	public class CompRitualFireOverlay : CompFireOverlayBase
	{
		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x06009D24 RID: 40228 RVA: 0x0038825C File Offset: 0x0038645C
		public new CompProperties_FireOverlayRitual Props
		{
			get
			{
				return (CompProperties_FireOverlayRitual)this.props;
			}
		}

		// Token: 0x06009D25 RID: 40229 RVA: 0x0038826C File Offset: 0x0038646C
		public override void PostDraw()
		{
			base.PostDraw();
			LordJob_Ritual lordJob_Ritual = this.parent.TargetOfRitual();
			if (lordJob_Ritual == null)
			{
				return;
			}
			if (lordJob_Ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			Vector3 loc = this.parent.TrueCenter();
			loc.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			CompFireOverlay.FireGraphic.Draw(loc, Rot4.North, this.parent, 0f);
		}

		// Token: 0x06009D26 RID: 40230 RVA: 0x003882D8 File Offset: 0x003864D8
		public override void CompTick()
		{
			LordJob_Ritual lordJob_Ritual = this.parent.TargetOfRitual();
			if (lordJob_Ritual == null)
			{
				return;
			}
			if (lordJob_Ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			if (this.startedGrowingAtTick < 0)
			{
				this.startedGrowingAtTick = GenTicks.TicksAbs;
			}
			if (GenTicks.TicksAbs % 30 == 0)
			{
				FleckMaker.ThrowFireGlow(this.parent.TrueCenter(), this.parent.Map, 1f);
			}
		}

		// Token: 0x04005D22 RID: 23842
		public const int FireGlowIntervalTicks = 30;
	}
}
