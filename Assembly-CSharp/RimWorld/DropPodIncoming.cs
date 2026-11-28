using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4D RID: 6989
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveTransporter, IThingHolder
	{
		// Token: 0x17002073 RID: 8307
		// (get) Token: 0x0600B368 RID: 45928 RVA: 0x003F3B57 File Offset: 0x003F1D57
		// (set) Token: 0x0600B369 RID: 45929 RVA: 0x003F3B6F File Offset: 0x003F1D6F
		public ActiveTransporterInfo Contents
		{
			get
			{
				return ((ActiveTransporter)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveTransporter)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600B36A RID: 45930 RVA: 0x003F3B88 File Offset: 0x003F1D88
		protected override void SpawnThings()
		{
			if (this.Contents.spawnWipeMode == null)
			{
				base.SpawnThings();
				return;
			}
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenSpawn.Spawn(this.innerContainer[i], base.Position, base.Map, this.Contents.spawnWipeMode.Value);
			}
		}

		// Token: 0x0600B36B RID: 45931 RVA: 0x003F3BF4 File Offset: 0x003F1DF4
		protected override void Impact()
		{
			for (int i = 0; i < 6; i++)
			{
				FleckMaker.ThrowDustPuff(base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f), base.Map, 1.2f);
			}
			FleckMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), base.Map, 2f);
			GenClamor.DoClamor(this, 15f, ClamorDefOf.Impact);
			base.Impact();
		}
	}
}
