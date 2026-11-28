using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE5 RID: 8165
	public class Verb_DeployDeadlifePack : Verb
	{
		// Token: 0x0600CF0C RID: 53004 RVA: 0x004C446E File Offset: 0x004C266E
		protected override bool TryCastShot()
		{
			return Verb_DeployDeadlifePack.TryDeploy(base.EquipmentSource.TryGetComp<CompApparelReloadable>(), base.EquipmentSource.TryGetComp<CompReleaseGas>());
		}

		// Token: 0x0600CF0D RID: 53005 RVA: 0x004C448C File Offset: 0x004C268C
		public static bool TryDeploy(CompApparelReloadable reloadable, CompReleaseGas releaseGas)
		{
			if (!ModLister.CheckAnomaly("Deadlife packs"))
			{
				return false;
			}
			string text;
			if (reloadable == null || !reloadable.CanBeUsed(out text) || releaseGas == null)
			{
				return false;
			}
			reloadable.UsedOnce();
			releaseGas.StartRelease();
			return true;
		}
	}
}
