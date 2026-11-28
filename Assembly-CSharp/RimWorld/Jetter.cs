using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B27 RID: 6951
	public class Jetter : Thing
	{
		// Token: 0x0600B22A RID: 45610 RVA: 0x003EBB6C File Offset: 0x003E9D6C
		protected override void Tick()
		{
			if (this.JState == Jetter.JetterState.WickBurning)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
				this.WickTicksLeft--;
				if (this.WickTicksLeft == 0)
				{
					this.StartJetting();
					return;
				}
			}
			else if (this.JState == Jetter.JetterState.Jetting)
			{
				this.TicksUntilMove--;
				if (this.TicksUntilMove <= 0)
				{
					this.MoveJetter();
					this.TicksUntilMove = 3;
				}
			}
		}

		// Token: 0x0600B22B RID: 45611 RVA: 0x003EBBDE File Offset: 0x003E9DDE
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x0600B22C RID: 45612 RVA: 0x003EBC0C File Offset: 0x003E9E0C
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x0600B22D RID: 45613 RVA: 0x003EBC43 File Offset: 0x003E9E43
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x0600B22E RID: 45614 RVA: 0x003EBC7C File Offset: 0x003E9E7C
		protected void MoveJetter()
		{
			IntVec3 intVec = base.Position + base.Rotation.FacingCell;
			if (!intVec.Walkable(base.Map) || base.Map.thingGrid.CellContains(intVec, ThingCategory.Pawn) || intVec.GetEdifice(base.Map) != null)
			{
				this.Destroy(DestroyMode.Vanish);
				GenExplosion.DoExplosion(base.Position, base.Map, 2.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				return;
			}
			base.Position = intVec;
		}

		// Token: 0x0600B22F RID: 45615 RVA: 0x003EBD61 File Offset: 0x003E9F61
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
			if (this.jetSoundSustainer != null)
			{
				this.jetSoundSustainer.End();
				this.jetSoundSustainer = null;
			}
		}

		// Token: 0x040066C7 RID: 26311
		private Jetter.JetterState JState;

		// Token: 0x040066C8 RID: 26312
		private int WickTicksLeft;

		// Token: 0x040066C9 RID: 26313
		private int TicksUntilMove;

		// Token: 0x040066CA RID: 26314
		protected Sustainer wickSoundSustainer;

		// Token: 0x040066CB RID: 26315
		protected Sustainer jetSoundSustainer;

		// Token: 0x040066CC RID: 26316
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x040066CD RID: 26317
		private const int TicksBetweenMoves = 3;

		// Token: 0x0200383D RID: 14397
		private enum JetterState
		{
			// Token: 0x0400E093 RID: 57491
			Resting,
			// Token: 0x0400E094 RID: 57492
			WickBurning,
			// Token: 0x0400E095 RID: 57493
			Jetting
		}
	}
}
