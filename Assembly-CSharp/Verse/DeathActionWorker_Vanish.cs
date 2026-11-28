using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020000CC RID: 204
	public class DeathActionWorker_Vanish : DeathActionWorker
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0002C950 File Offset: 0x0002AB50
		public DeathActionProperties_Vanish Props
		{
			get
			{
				return (DeathActionProperties_Vanish)this.props;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002C960 File Offset: 0x0002AB60
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (this.Props.fleck != null)
			{
				FleckMaker.Static(corpse.PositionHeld, corpse.MapHeld, this.Props.fleck, 1f);
			}
			if (this.Props.filth != null)
			{
				int randomInRange = this.Props.filthCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					FilthMaker.TryMakeFilth(corpse.PositionHeld, corpse.MapHeld, this.Props.filth, 1, FilthSourceFlags.None, true);
				}
			}
			if (this.Props.meatExplosionSize != null && ModsConfig.AnomalyActive)
			{
				FleshbeastUtility.MeatSplatter(0, corpse.PositionHeld, corpse.MapHeld, this.Props.meatExplosionSize.Value);
			}
			corpse.Destroy(DestroyMode.Vanish);
		}
	}
}
