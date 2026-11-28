using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200158B RID: 5515
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x0600871D RID: 34589 RVA: 0x0030D374 File Offset: 0x0030B574
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x0600871E RID: 34590 RVA: 0x0030D387 File Offset: 0x0030B587
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x0600871F RID: 34591 RVA: 0x0030D39A File Offset: 0x0030B59A
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008720 RID: 34592 RVA: 0x0030D3BF File Offset: 0x0030B5BF
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x06008721 RID: 34593 RVA: 0x0030D3F0 File Offset: 0x0030B5F0
		public void RecentMemoryInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.Map.glowGrid.PsychGlowAt(this.pawn.Position) != PsychGlow.Dark)
			{
				this.lastLightTick = Find.TickManager.TicksGame;
			}
			if (this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06008722 RID: 34594 RVA: 0x0030D458 File Offset: 0x0030B658
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x06008723 RID: 34595 RVA: 0x0030D47E File Offset: 0x0030B67E
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04005388 RID: 21384
		private Pawn pawn;

		// Token: 0x04005389 RID: 21385
		private int lastLightTick = 999999;

		// Token: 0x0400538A RID: 21386
		private int lastOutdoorTick = 999999;
	}
}
