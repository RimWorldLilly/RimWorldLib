using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001884 RID: 6276
	[StaticConstructorOnStartup]
	public class CompFireOverlay : CompFireOverlayBase
	{
		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x06009D1E RID: 40222 RVA: 0x0038817B File Offset: 0x0038637B
		public new CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x06009D1F RID: 40223 RVA: 0x00388188 File Offset: 0x00386388
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.03658537f;
			CompFireOverlay.FireGraphic.Draw(drawPos, this.parent.Rotation, this.parent, 0f);
		}

		// Token: 0x06009D20 RID: 40224 RVA: 0x003881EE File Offset: 0x003863EE
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x06009D21 RID: 40225 RVA: 0x00388208 File Offset: 0x00386408
		public override void CompTick()
		{
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			if (this.startedGrowingAtTick < 0)
			{
				this.startedGrowingAtTick = GenTicks.TicksAbs;
			}
		}

		// Token: 0x04005D20 RID: 23840
		protected CompRefuelable refuelableComp;

		// Token: 0x04005D21 RID: 23841
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
