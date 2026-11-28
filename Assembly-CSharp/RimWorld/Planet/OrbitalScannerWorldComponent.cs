using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F9 RID: 8953
	public class OrbitalScannerWorldComponent : WorldComponent
	{
		// Token: 0x170026A4 RID: 9892
		// (get) Token: 0x0600DD48 RID: 56648 RVA: 0x00526350 File Offset: 0x00524550
		private bool OnCooldown
		{
			get
			{
				return this.lastFoundSignal > 0 && Find.TickManager.TicksGame < this.lastFoundSignal + 1080000;
			}
		}

		// Token: 0x0600DD49 RID: 56649 RVA: 0x00526375 File Offset: 0x00524575
		public OrbitalScannerWorldComponent(World world) : base(world)
		{
		}

		// Token: 0x0600DD4A RID: 56650 RVA: 0x00526390 File Offset: 0x00524590
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastFoundSignal, "lastFoundSignal", 0, false);
		}

		// Token: 0x0600DD4B RID: 56651 RVA: 0x005263A4 File Offset: 0x005245A4
		public override void WorldComponentTick()
		{
			if (this.OnCooldown)
			{
				return;
			}
			if (this.workingScanners.Empty<CompOrbitalScanner>())
			{
				return;
			}
			if (Rand.MTBEventOccurs(60000f, 1f, 1f))
			{
				this.workingScanners.RandomElement<CompOrbitalScanner>().ReceiveSignal();
				this.lastFoundSignal = Find.TickManager.TicksGame;
			}
			this.workingScanners.Clear();
		}

		// Token: 0x0600DD4C RID: 56652 RVA: 0x00526409 File Offset: 0x00524609
		public void Notify_ScannerWorking(CompOrbitalScanner scanner)
		{
			if (this.OnCooldown)
			{
				return;
			}
			this.workingScanners.Add(scanner);
		}

		// Token: 0x04008DD6 RID: 36310
		private int lastFoundSignal = -1;

		// Token: 0x04008DD7 RID: 36311
		private List<CompOrbitalScanner> workingScanners = new List<CompOrbitalScanner>();

		// Token: 0x04008DD8 RID: 36312
		private const int FindSignalMTBTicks = 60000;

		// Token: 0x04008DD9 RID: 36313
		private const int FindSignalCooldownTicks = 1080000;
	}
}
