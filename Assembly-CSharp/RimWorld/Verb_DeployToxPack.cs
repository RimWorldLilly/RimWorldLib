using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE4 RID: 8164
	public class Verb_DeployToxPack : Verb
	{
		// Token: 0x0600CF09 RID: 53001 RVA: 0x004C440D File Offset: 0x004C260D
		protected override bool TryCastShot()
		{
			return Verb_DeployToxPack.TryDeploy(base.EquipmentSource.TryGetComp<CompApparelReloadable>(), base.EquipmentSource.TryGetComp<CompReleaseGas>());
		}

		// Token: 0x0600CF0A RID: 53002 RVA: 0x004C442C File Offset: 0x004C262C
		public static bool TryDeploy(CompApparelReloadable reloadable, CompReleaseGas releaseGas)
		{
			if (!ModLister.CheckBiotech("Tox packs"))
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
