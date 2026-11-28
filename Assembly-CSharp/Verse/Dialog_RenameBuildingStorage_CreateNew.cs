using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000672 RID: 1650
	public class Dialog_RenameBuildingStorage_CreateNew : Dialog_Rename<IRenameable>
	{
		// Token: 0x0600341F RID: 13343 RVA: 0x00150327 File Offset: 0x0014E527
		public Dialog_RenameBuildingStorage_CreateNew(IStorageGroupMember building) : base(null)
		{
			this.building = building;
			StorageGroup group = building.Group;
			this.curName = (((group != null) ? group.RenamableLabel : null) ?? string.Empty);
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x00150358 File Offset: 0x0014E558
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

		// Token: 0x06003421 RID: 13345 RVA: 0x001503E4 File Offset: 0x0014E5E4
		protected override void OnRenamed(string name)
		{
			StorageGroup storageGroup = this.building.Map.storageGroups.NewGroup(null);
			storageGroup.RenamableLabel = name;
			storageGroup.InitFrom(this.building);
			this.building.SetStorageGroup(storageGroup, true);
		}

		// Token: 0x04002212 RID: 8722
		private IStorageGroupMember building;
	}
}
