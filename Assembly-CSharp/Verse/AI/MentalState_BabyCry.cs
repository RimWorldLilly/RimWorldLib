using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200087B RID: 2171
	public class MentalState_BabyCry : MentalState_BabyFit
	{
		// Token: 0x060043CC RID: 17356 RVA: 0x001AA408 File Offset: 0x001A8608
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			float num = this.pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None);
			Pawn pawn = this.pawn.SpawnedParentOrMe as Pawn;
			if (pawn != null && !pawn.Position.Fogged(pawn.Map))
			{
				int num2 = this.ticksUntilLeftTear - 1;
				this.ticksUntilLeftTear = num2;
				if (num2 <= 0)
				{
					pawn.Map.flecks.CreateFleck(new FleckCreationData
					{
						spawnPosition = this.pawn.DrawPosHeld.Value + new Vector3(-0.15f, 0f, 0.066f).RotatedBy(num),
						velocitySpeed = -0.66f,
						velocityAngle = 90f + num - MentalState_BabyCry.randAngle.RandomInRange,
						def = FleckDefOf.FleckBabyCrying,
						scale = MentalState_BabyCry.randScale.RandomInRange
					});
					this.ticksUntilLeftTear = MentalState_BabyCry.TicksBetweenTears.RandomInRange;
				}
				num2 = this.ticksUntilRightTear - 1;
				this.ticksUntilRightTear = num2;
				if (num2 <= 0)
				{
					pawn.Map.flecks.CreateFleck(new FleckCreationData
					{
						spawnPosition = this.pawn.DrawPosHeld.Value + new Vector3(0.15f, 0f, 0.066f).RotatedBy(num),
						velocitySpeed = 0.66f,
						velocityAngle = 90f + num + MentalState_BabyCry.randAngle.RandomInRange,
						def = FleckDefOf.FleckBabyCrying,
						scale = MentalState_BabyCry.randScale.RandomInRange,
						exactScale = new Vector3?(new Vector3(-1f, 1f, 1f))
					});
					this.ticksUntilRightTear = MentalState_BabyCry.TicksBetweenTears.RandomInRange;
				}
				if (this.pawn.IsHashIntervalTick(35, delta))
				{
					MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_BabyCryingDots, new Vector3(0.27f, 0f, 0.066f).RotatedBy(num), 1f, -1f).exactRotation = Rand.Value * 180f;
					MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_BabyCryingDots, new Vector3(-0.27f, 0f, 0.066f).RotatedBy(num), 1f, -1f).exactRotation = Rand.Value * 180f;
				}
			}
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x001AA6A0 File Offset: 0x001A88A0
		protected override void AuraEffect(Thing source, Pawn hearer)
		{
			hearer.HearClamor(source, ClamorDefOf.BabyCry);
			Pawn pawn = source as Pawn;
			if (pawn != null && hearer.needs.mood != null)
			{
				if (hearer == pawn.GetMother() || hearer == pawn.GetFather())
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyCryingBaby, pawn, null);
				}
				else
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CryingBaby, pawn, null);
				}
				hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BabyCriedSocial, pawn, null);
			}
		}

		// Token: 0x04002C19 RID: 11289
		private int ticksUntilLeftTear;

		// Token: 0x04002C1A RID: 11290
		private int ticksUntilRightTear;

		// Token: 0x04002C1B RID: 11291
		private const int TicksBetweenTearDots = 35;

		// Token: 0x04002C1C RID: 11292
		private static readonly IntRange TicksBetweenTears = new IntRange(25, 40);

		// Token: 0x04002C1D RID: 11293
		private const float speed = 0.66f;

		// Token: 0x04002C1E RID: 11294
		private static readonly FloatRange randAngle = new FloatRange(10f, 30f);

		// Token: 0x04002C1F RID: 11295
		private static readonly FloatRange randScale = new FloatRange(0.6f, 1f);
	}
}
