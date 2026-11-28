using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194A RID: 6474
	public class CompPushable : ThingComp
	{
		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x0600A267 RID: 41575 RVA: 0x003A0CA0 File Offset: 0x0039EEA0
		public CompProperties_Pushable Props
		{
			get
			{
				return (CompProperties_Pushable)this.props;
			}
		}

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x0600A268 RID: 41576 RVA: 0x003A0CAD File Offset: 0x0039EEAD
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x0600A269 RID: 41577 RVA: 0x003A0CBA File Offset: 0x0039EEBA
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x0600A26A RID: 41578 RVA: 0x003A0CBC File Offset: 0x0039EEBC
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompPushable.<CompFloatMenuOptions>d__8 <CompFloatMenuOptions>d__ = new CompPushable.<CompFloatMenuOptions>d__8(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A26B RID: 41579 RVA: 0x003A0CD4 File Offset: 0x0039EED4
		public void OnStartedCarrying(Pawn pawn)
		{
			Vector3 v = (pawn.Position - this.Pawn.Position).ToVector3();
			Vector2 vector = new Vector2(0f, -this.Props.offsetDistance).RotatedBy(v.AngleFlat());
			this.drawPos = new Vector3(vector.x, 0f, -vector.y);
			this.drawVel = Vector3.zero;
		}

		// Token: 0x0600A26C RID: 41580 RVA: 0x003A0D4A File Offset: 0x0039EF4A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			return new CompPushable.<CompGetGizmosExtra>d__10(-2);
		}

		// Token: 0x0600A26D RID: 41581 RVA: 0x003A0D54 File Offset: 0x0039EF54
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.canBePushed, "canBePushed", false, false);
			Scribe_Values.Look<Vector3>(ref this.drawPos, "drawPos", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.drawVel, "drawVel", default(Vector3), false);
		}

		// Token: 0x04005FF2 RID: 24562
		public bool canBePushed = true;

		// Token: 0x04005FF3 RID: 24563
		public Vector3 drawPos;

		// Token: 0x04005FF4 RID: 24564
		public Vector3 drawVel;
	}
}
