using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001857 RID: 6231
	[StaticConstructorOnStartup]
	public class CompDarklightOverlay : CompFireOverlayBase
	{
		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x06009BF2 RID: 39922 RVA: 0x00382E45 File Offset: 0x00381045
		public new CompProperties_DarklightOverlay Props
		{
			get
			{
				return (CompProperties_DarklightOverlay)this.props;
			}
		}

		// Token: 0x06009BF3 RID: 39923 RVA: 0x00382E54 File Offset: 0x00381054
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.03658537f;
			CompDarklightOverlay.DarklightGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		// Token: 0x06009BF4 RID: 39924 RVA: 0x00382EB4 File Offset: 0x003810B4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x04005CB9 RID: 23737
		protected CompRefuelable refuelableComp;

		// Token: 0x04005CBA RID: 23738
		public static readonly Graphic DarklightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Darklight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
