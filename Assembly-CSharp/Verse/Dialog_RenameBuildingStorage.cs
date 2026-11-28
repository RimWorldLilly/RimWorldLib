using System;

namespace Verse
{
	// Token: 0x02000671 RID: 1649
	public class Dialog_RenameBuildingStorage : Dialog_Rename<IRenameable>
	{
		// Token: 0x0600341D RID: 13341 RVA: 0x00150291 File Offset: 0x0014E491
		public Dialog_RenameBuildingStorage(IRenameable storage) : base(storage)
		{
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x0015029C File Offset: 0x0014E49C
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (Current.Game.CurrentMap.zoneManager.AllZones.Any((Zone z) => z.label == name) || Current.Game.CurrentMap.storageGroups.HasGroupWithName(name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}
	}
}
