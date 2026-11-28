using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200037C RID: 892
	internal struct PawnStatusEffecters
	{
		// Token: 0x06001B34 RID: 6964 RVA: 0x000A7A44 File Offset: 0x000A5C44
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000A7A58 File Offset: 0x000A5C58
		public void EffectersTick(bool suspended)
		{
			if (!suspended)
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffComp_Effecter hediffComp_Effecter = hediffs[i].TryGetComp<HediffComp_Effecter>();
					if (hediffComp_Effecter != null)
					{
						EffecterDef effecterDef = hediffComp_Effecter.CurrentStateEffecter();
						if (effecterDef != null)
						{
							this.AddOrMaintain(effecterDef);
						}
					}
				}
				Pawn_MindState mindState = this.pawn.mindState;
				if (((mindState != null) ? mindState.mentalStateHandler.CurState : null) != null)
				{
					EffecterDef effecterDef2 = this.pawn.mindState.mentalStateHandler.CurState.CurrentStateEffecter();
					if (effecterDef2 != null)
					{
						this.AddOrMaintain(effecterDef2);
					}
				}
			}
			for (int j = this.pairs.Count - 1; j >= 0; j--)
			{
				if (this.pairs[j].Expired)
				{
					this.pairs[j].Cleanup();
					this.pairs.RemoveAt(j);
				}
				else
				{
					this.pairs[j].Tick(this.pawn, 1);
				}
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000A7B64 File Offset: 0x000A5D64
		private void AddOrMaintain(EffecterDef def)
		{
			for (int i = 0; i < this.pairs.Count; i++)
			{
				if (this.pairs[i].def == def)
				{
					this.pairs[i].Maintain();
					return;
				}
			}
			PawnStatusEffecters.LiveEffecter liveEffecter = FullPool<PawnStatusEffecters.LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}

		// Token: 0x0400165A RID: 5722
		public Pawn pawn;

		// Token: 0x0400165B RID: 5723
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x0200269E RID: 9886
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x17002AE2 RID: 10978
			// (get) Token: 0x0600F434 RID: 62516 RVA: 0x005932BD File Offset: 0x005914BD
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x0600F436 RID: 62518 RVA: 0x005932D9 File Offset: 0x005914D9
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x0600F437 RID: 62519 RVA: 0x005932F4 File Offset: 0x005914F4
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x0600F438 RID: 62520 RVA: 0x0059330B File Offset: 0x0059150B
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x0600F439 RID: 62521 RVA: 0x0059331D File Offset: 0x0059151D
			public void Tick(Pawn pawn, int delta)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.SpawnAttached(pawn, pawn.MapHeld, 1f);
				}
				this.effecter.EffectTick(pawn, pawn);
			}

			// Token: 0x04009E16 RID: 40470
			public EffecterDef def;

			// Token: 0x04009E17 RID: 40471
			public Effecter effecter;

			// Token: 0x04009E18 RID: 40472
			public int lastMaintainTick;
		}
	}
}
