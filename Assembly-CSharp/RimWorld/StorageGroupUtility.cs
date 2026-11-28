using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A89 RID: 6793
	[StaticConstructorOnStartup]
	public static class StorageGroupUtility
	{
		// Token: 0x0600AB7C RID: 43900 RVA: 0x003C931E File Offset: 0x003C751E
		public static IEnumerable<Gizmo> StorageGroupMemberGizmos(IStorageGroupMember member)
		{
			StorageGroupUtility.<StorageGroupMemberGizmos>d__5 <StorageGroupMemberGizmos>d__ = new StorageGroupUtility.<StorageGroupMemberGizmos>d__5(-2);
			<StorageGroupMemberGizmos>d__.<>3__member = member;
			return <StorageGroupMemberGizmos>d__;
		}

		// Token: 0x0600AB7D RID: 43901 RVA: 0x003C9330 File Offset: 0x003C7530
		public static void DrawSelectionOverlaysFor(IStorageGroupMember member)
		{
			if (member.Group != null && !SelectionDrawer.DrawnStorageGroupThisFrame(member.Group))
			{
				foreach (IStorageGroupMember storageGroupMember in member.Group.members)
				{
					if (storageGroupMember.DrawConnectionOverlay)
					{
						SelectionDrawer.DrawSelectionBracketFor(storageGroupMember, StorageGroupUtility.GroupedMat);
					}
				}
				SelectionDrawer.Notify_DrawnStorageGroup(member.Group);
			}
		}

		// Token: 0x0600AB7E RID: 43902 RVA: 0x003C93B4 File Offset: 0x003C75B4
		public static void SetStorageGroup(this IStorageGroupMember member, StorageGroup newGroup, bool removeIfEmpty = true)
		{
			if (member.Group != null && member.Group == newGroup)
			{
				return;
			}
			if (member.Group != null)
			{
				StorageSettings storeSettings = member.Group.GetStoreSettings();
				member.Group.RemoveMember(member, removeIfEmpty);
				member.Group = null;
				member.StoreSettings.CopyFrom(storeSettings);
			}
			if (newGroup != null)
			{
				member.Group = newGroup;
				member.Group.members.Add(member);
			}
			IStoreSettingsParent storeSettingsParent = member as IStoreSettingsParent;
			if (storeSettingsParent != null)
			{
				storeSettingsParent.Notify_SettingsChanged();
			}
		}

		// Token: 0x0600AB7F RID: 43903 RVA: 0x003C9432 File Offset: 0x003C7632
		public static bool IsHopper(this Thing thing)
		{
			bool flag;
			if (thing == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingDef def = thing.def;
				flag = (((def != null) ? def.building : null) != null);
			}
			return flag && thing.def.building.isHopper;
		}

		// Token: 0x040063D0 RID: 25552
		private static readonly Texture2D LinkTex = ContentFinder<Texture2D>.Get("UI/Commands/LinkStorageSettings", true);

		// Token: 0x040063D1 RID: 25553
		private static readonly Texture2D UnlinkTex = ContentFinder<Texture2D>.Get("UI/Commands/UnlinkStorageSettings", true);

		// Token: 0x040063D2 RID: 25554
		private static readonly Texture2D SelectLinkedTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllLinked", true);

		// Token: 0x040063D3 RID: 25555
		public static readonly Material GroupedMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay, new Color(1f, 1f, 0f, 0.2f));

		// Token: 0x040063D4 RID: 25556
		private static List<IStorageGroupMember> tmpMembers = new List<IStorageGroupMember>();
	}
}
