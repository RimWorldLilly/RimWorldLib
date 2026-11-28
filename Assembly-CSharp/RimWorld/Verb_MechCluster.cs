using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001FEB RID: 8171
	public class Verb_MechCluster : Verb_CastBase
	{
		// Token: 0x0600CF2B RID: 53035 RVA: 0x004C4A38 File Offset: 0x004C2C38
		protected override bool TryCastShot()
		{
			if (Faction.OfMechanoids == null)
			{
				Messages.Message("MessageNoFactionForVerbMechCluster".Translate(), this.caster, MessageTypeDefOf.RejectInput, null, false);
				return false;
			}
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			MechClusterUtility.SpawnCluster(this.currentTarget.Cell, this.caster.Map, MechClusterGenerator.GenerateClusterSketch(2500f, this.caster.Map, true, true), true, false, null);
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600CF2C RID: 53036 RVA: 0x004C4AE8 File Offset: 0x004C2CE8
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x040083C1 RID: 33729
		public const float Points = 2500f;
	}
}
