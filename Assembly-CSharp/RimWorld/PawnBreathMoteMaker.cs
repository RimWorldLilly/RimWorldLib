using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001494 RID: 5268
	public class PawnBreathMoteMaker
	{
		// Token: 0x06008123 RID: 33059 RVA: 0x002E1789 File Offset: 0x002DF989
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008124 RID: 33060 RVA: 0x002E1798 File Offset: 0x002DF998
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.RaceProps.Humanlike || this.pawn.RaceProps.IsMechanoid || this.pawn.IsShambler)
			{
				return;
			}
			if (ModsConfig.OdysseyActive && this.pawn.Position.GetVacuum(this.pawn.MapHeld) > 0f)
			{
				return;
			}
			int num = Mathf.Abs(Find.TickManager.TicksGame + this.pawn.HashOffset()) % 320;
			if (num < ticksPassed)
			{
				this.doThisBreath = (this.pawn.AmbientTemperature < 0f && this.pawn.GetPosture() == PawnPosture.Standing);
			}
			if (this.doThisBreath && num < 80 && num % 8 < ticksPassed)
			{
				this.TryMakeBreathMote();
			}
		}

		// Token: 0x06008125 RID: 33061 RVA: 0x002E186C File Offset: 0x002DFA6C
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			FleckMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x0400504D RID: 20557
		private Pawn pawn;

		// Token: 0x0400504E RID: 20558
		private bool doThisBreath;

		// Token: 0x0400504F RID: 20559
		private const int BreathDuration = 80;

		// Token: 0x04005050 RID: 20560
		private const int BreathInterval = 320;

		// Token: 0x04005051 RID: 20561
		private const int MoteInterval = 8;

		// Token: 0x04005052 RID: 20562
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04005053 RID: 20563
		private const float BreathRotationOffsetDist = 0.21f;

		// Token: 0x04005054 RID: 20564
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);
	}
}
