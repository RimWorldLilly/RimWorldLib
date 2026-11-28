using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000670 RID: 1648
	public class Dialog_RenameZone : Dialog_Rename<Zone>
	{
		// Token: 0x0600341A RID: 13338 RVA: 0x001501D0 File Offset: 0x0014E3D0
		public Dialog_RenameZone(Zone zone) : base(zone)
		{
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x001501DC File Offset: 0x0014E3DC
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.renaming.Map.zoneManager.AllZones.Any((Zone z) => z != this.renaming && z.label == name) || Current.Game.CurrentMap.storageGroups.HasGroupWithName(name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x0015026F File Offset: 0x0014E46F
		protected override void OnRenamed(string name)
		{
			Messages.Message("ZoneGainsName".Translate(name), MessageTypeDefOf.TaskCompletion, false);
		}
	}
}
